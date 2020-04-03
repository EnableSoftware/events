using Events.Data.Model;
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
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbctx;
        public CategoryController(ApplicationDbContext dbctx)
        {
            _dbctx = dbctx;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
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

            var now = DateTimeOffset.Now;
            var categories = await _dbctx.Categories.Select(o => new CategoryModel
            {
                Id = o.Id,
                Name = o.Name,
                UpcomingEvents = o.Events.Count(o => o.Date >= now),
                NextEvent = o.Events.Where(o => o.UserSignUps.Any(q => q.UserId == user.Id)).Where(q => q.Date > now).OrderBy(q => q.Date).Select(q => q.Date).FirstOrDefault()

            }).ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _dbctx.Categories.Include(o => o.Events).ThenInclude(o => o.UserSignUps).FirstOrDefaultAsync(o => o.Id == id);

            if (category == null)
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
            var now = DateTimeOffset.Now;
            var categoryModel = new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                UpcomingEvents = category.Events.Count(o => o.Date >= now),
                NextEvent = category.Events.Where(o => o.UserSignUps.Any(q => q.UserId == user.Id)).Where(o => o.Date > now).OrderBy(o => o.Date).Select(o => o.Date).FirstOrDefault()
            };

            return Ok(categoryModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post(CategoryModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var category = new Category() { Name = model.Name };
            _dbctx.Categories.Add(category);
            await _dbctx.SaveChangesAsync();
            return Ok(category.Id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var category = await _dbctx.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            await _dbctx.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _dbctx.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _dbctx.Categories.Remove(category);

            await _dbctx.SaveChangesAsync();

            return NoContent();
        }
    }
}
