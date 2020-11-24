using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Events.Data.Model;
using Events.Data.Postgres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;

namespace Events.Server.Extensions.StartupExtensions
{
    public static class AuthenticationExtensions
    {
        public static void AddADAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMicrosoftIdentityWebApiAuthentication(configuration, "AzureAd");

            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var existingOnTokenValidatedHandler = options.Events.OnTokenValidated;
                options.Events ??= new JwtBearerEvents();
                options.Events.OnTokenValidated += OnTokenValidatedFunc;
            });
        }

        private static async Task OnTokenValidatedFunc(TokenValidatedContext context)
        {
            var emailClaim = context.Principal.FindFirst(ClaimTypes.Upn).Value.ToLower();
            var givenNameClaim = context.Principal.FindFirst(ClaimTypes.GivenName).Value;
            var surnameClaim = context.Principal.FindFirst(ClaimTypes.Surname).Value;
            var db = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            var user = await db.Users.Where(o => o.Email == emailClaim).FirstOrDefaultAsync().ConfigureAwait(false);

            if (user == null)
            {
                user = new User
                {
                    Email = emailClaim,
                };
                db.Users.Add(user);
            }

            user.FirstName = givenNameClaim;
            user.LastName = surnameClaim;

            await db.SaveChangesAsync().ConfigureAwait(false);
        }   
    }
}
