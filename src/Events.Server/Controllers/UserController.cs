using Events.Data.Model;
using Events.Shared.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var models = await _dbctx.Users.Select(o => new UserModel
            {
                Id = o.Id,
                Name = o.Name,
                Email = o.Email,
                IsAdmin = o.IsAdmin
            }).ToListAsync();

            return Ok(models);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
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
                Name = user.Name,
                Email = user.Email,
                IsAdmin = user.IsAdmin
            };

            return Ok(userModel);
        }

        // Only AD auth, this has no use
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Post(UserModel model)
        //{
        //    throw new NotImplementedException();
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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

            user.IsAdmin = model.IsAdmin;
            await _dbctx.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
