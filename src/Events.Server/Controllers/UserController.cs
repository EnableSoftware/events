using Events.Data.Postgres;
using Events.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbctx;

        public UserController(ApplicationDbContext dbctx)
        {
            _dbctx = dbctx;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var models = await _dbctx.Users.Select(o => new UserModel
            {
                Id = o.Id,
                Name = $"{o.FirstName} {o.LastName}",
                Email = o.Email
            }).ToListAsync();

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _dbctx.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userModel = new UserModel()
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email
            };

            return Ok(userModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var user = await _dbctx.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _dbctx.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _dbctx.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _dbctx.Users.Remove(user);

            await _dbctx.SaveChangesAsync();

            return NoContent();
        }
    }
}
