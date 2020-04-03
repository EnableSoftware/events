using Events.Data.Model;
using Events.Server.Services.Authentication;
using Events.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Events.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbctx;

        public AccountController(ApplicationDbContext dbctx)
        {
            _dbctx = dbctx;
        }

        [HttpGet("get-self")]
        public async Task<IActionResult> GetSelf()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userModel = new UserModel
            {
                Name = User.FindFirstValue(CustomClaimTypes.Name),
                Email = User.FindFirstValue(ClaimTypes.Upn),
                Role = User.FindFirstValue(ClaimTypes.Role)
            };

            if (!string.IsNullOrEmpty(userModel.Email))
            {
                userModel.Email = userModel.Email.ToLower();
                var user = await _dbctx.Users.FirstOrDefaultAsync(o => o.Email == userModel.Email);

                if (user == null)
                {
                    return Unauthorized();
                }

                userModel.Id = user.Id;
            }

            return Ok(userModel);
        }
    }
}
