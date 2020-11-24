using Events.Data.Model;
using Events.Data.Postgres;
using Events.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Events.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly ApplicationDbContext _dbctx;
        public EventController(ApplicationDbContext dbctx)
        {
            _dbctx = dbctx;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await _dbctx.Events
                .Select(o => new EventModel
                {
                    Capacity = o.Capacity,
                    CategoryId = o.CategoryId,
                    Date = o.Date,
                    Id = o.Id,
                    LockedDate = o.LockedDate,
                    Attendees = o.UserSignUps.Where(q => q.Priority <= o.Capacity && q.Priority > 0).Select(q => new EventAttendeeModel() { Id = q.UserId, Name = $"{q.User.FirstName} {q.User.LastName}" }),
                    Location = o.Location,
                    SignedUpCount = o.UserSignUps.Count
                }).ToListAsync();

            return Ok(events);
        }

        [HttpGet("get-for-category/{id}")]
        public async Task<IActionResult> GetForCategory(int id)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Upn);

            if (emailClaim == null)
            {
                return NotFound();
            }

            var user = await _dbctx.Users.Where(o => o.Email == emailClaim.Value.ToLower()).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var today = DateTimeOffset.Now.Date;
            var events = await _dbctx.Events
                .Where(o => o.CategoryId == id)
                .Where(o => o.Date >= today)
                .Select(o => new EventModel
                {
                    Capacity = o.Capacity,
                    CategoryId = o.CategoryId,
                    Date = o.Date,
                    Id = o.Id,
                    Location = o.Location,
                    Attendees = o.UserSignUps.Where(q => q.Priority <= o.Capacity && q.Priority > 0).Select(q => new EventAttendeeModel() { Id = q.UserId, Name = $"{q.User.FirstName} {q.User.LastName}" }),
                    LockedDate = o.LockedDate,
                    SignedUp = o.UserSignUps.Any(o => o.UserId == user.Id),
                    SignedUpCount = o.UserSignUps.Count
                })
                .OrderBy(o => o.Date)
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dbEvent = await _dbctx.Events.Include(o => o.UserSignUps).FirstOrDefaultAsync(o => o.Id == id);

            if (dbEvent == null)
            {
                return NotFound();
            }

            var emailClaim = User.FindFirst(ClaimTypes.Upn);

            if (emailClaim == null)
            {
                return NotFound();
            }

            var user = await _dbctx.Users.Where(o => o.Email == emailClaim.Value.ToLower()).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var eventModel = new EventModel()
            {
                Capacity = dbEvent.Capacity,
                CategoryId = dbEvent.CategoryId,
                Date = dbEvent.Date,
                Id = dbEvent.Id,
                Attendees = dbEvent.UserSignUps.Where(q => q.Priority <= dbEvent.Capacity && q.Priority > 0).Select(q => new EventAttendeeModel() { Id = q.UserId, Name = $"{q.User.FirstName} {q.User.LastName}" }),
                LockedDate = dbEvent.LockedDate,
                Location = dbEvent.Location,
                SignedUp = dbEvent.UserSignUps.Any(o => o.UserId == user.Id),
                SignedUpCount = dbEvent.UserSignUps.Count
            };

            return Ok(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var dbEvent = new Event()
            {
                Capacity = model.Capacity,
                CategoryId = model.CategoryId,
                Date = model.Date,
                Location = model.Location,
            };

            _dbctx.Events.Add(dbEvent);

            await _dbctx.SaveChangesAsync();

            return Ok(dbEvent.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var dbEvent = await _dbctx.Events.FindAsync(id);

            if (dbEvent == null)
            {
                return NotFound();
            }

            dbEvent.Date = model.Date;
            dbEvent.Location = model.Location;
            dbEvent.Capacity = model.Capacity;
            await _dbctx.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbEvent = await _dbctx.Events.FindAsync(id);

            if (dbEvent == null)
            {
                return NotFound();
            }

            _dbctx.Events.Remove(dbEvent);

            await _dbctx.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("unlock-event/{id}")]
        public async Task<IActionResult> Unlock(int id)
        {
            var dbEvent = await _dbctx.Events.Include(o => o.UserSignUps).ThenInclude(o => o.User.CategoryTickets).FirstOrDefaultAsync(o => o.Id == id);

            if (dbEvent == null)
            {
                return NotFound();
            }

            foreach (var userSignUp in dbEvent.UserSignUps)
            {
                userSignUp.Priority = 0;
            }

            dbEvent.LockedDate = null;

            await _dbctx.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("lock-event/{id}")]
        public async Task<IActionResult> Lock(int id)
        {
            var dbEvent = await _dbctx.Events.Include(o => o.UserSignUps).ThenInclude(o => o.User.CategoryTickets).FirstOrDefaultAsync(o => o.Id == id);

            if (dbEvent == null)
            {
                return NotFound();
            }

            if (dbEvent.UserSignUps.Any(o => o.Priority > 0))
            {
                return BadRequest();
            }

            var orderedEventSignUps = dbEvent.UserSignUps
                .OrderByDescending(o => o.User.CategoryTickets.Where(q => q.CategoryId == dbEvent.CategoryId).Select(w => w.Penalty).FirstOrDefault())
                .ThenBy(o => Guid.NewGuid());

            var priority = 1;
            foreach (var signUp in orderedEventSignUps)
            {
                signUp.Priority = priority;
                var categoryTicket = signUp.User.CategoryTickets.FirstOrDefault(o => o.CategoryId == dbEvent.CategoryId);

                if (priority >= dbEvent.Capacity)
                {
                    categoryTicket.Penalty += 5;
                }

                priority++;
            }

            dbEvent.LockedDate = DateTimeOffset.Now;

            await _dbctx.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("sign-up/{id}")]
        public async Task<IActionResult> SignUp(int id, [FromBody] bool signup)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Upn);

            if (emailClaim == null)
            {
                return NotFound();
            }

            var user = await _dbctx.Users.Include(o => o.EventSignUps).Include(o => o.CategoryTickets).ThenInclude(q => q.Category).Where(o => o.Email == emailClaim.Value.ToLower()).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var dbEvent = await _dbctx.Events.FindAsync(id);

            if (dbEvent == null)
            {
                return NotFound();
            }

            var userCategoryTickets = user.CategoryTickets.Where(o => o.Category.Id == dbEvent.CategoryId).FirstOrDefault();

            if (userCategoryTickets == null)
            {
                userCategoryTickets = new UserCategoryTickets()
                {
                    CategoryId = dbEvent.CategoryId,
                    UserId = user.Id,
                    Penalty = 0
                };

                _dbctx.UserCategoryTickets.Add(userCategoryTickets);
            }

            if (signup && !user.EventSignUps.Any(o => o.EventId == id))
            {
                var signUpEventToAdd = new UserEventSignUp()
                {
                    EventId = id,
                    UserId = user.Id
                };

                _dbctx.UserEventSignUps.Add(signUpEventToAdd);
            }
            else
            {
                var signUpEventToDelete = await _dbctx.UserEventSignUps.Where(o => o.UserId == user.Id).Where(o => o.EventId == id).FirstOrDefaultAsync();

                if (signUpEventToDelete != null)
                {
                    var notice = (DateTimeOffset.Now.Date - dbEvent.Date.Date).Days;
                    var penalty = 15 - Math.Min(notice * 3, 15);
                    userCategoryTickets.Penalty += penalty;
                    _dbctx.UserEventSignUps.Remove(signUpEventToDelete);
                }
            }

            await _dbctx.SaveChangesAsync();

            return NoContent();
        }
    }
}
