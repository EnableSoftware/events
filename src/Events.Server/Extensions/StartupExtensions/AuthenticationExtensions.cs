using Events.Data.Model;
using Events.Server.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Events.Server.Extensions.StartupExtensions
{
    public static class AuthenticationExtensions
    {
        public static void AddADAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(AzureADDefaults.OpenIdScheme)
                .AddAzureAD(options => configuration.Bind("AzureAd", options));

            // TODO auth is currently hacky / simple - explore MSAL instead
            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme,
                options =>
                {
                    options.Authority += "/v2.0/";
                    options.TokenValidationParameters.ValidateIssuer = true;
                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = async ctx =>
                            {
                                var emailClaim = ctx.Principal.FindFirst(ClaimTypes.Upn).Value.ToLower();
                                var nameClaim = ctx.Principal.FindFirst(CustomClaimTypes.Name).Value;

                                var db = ctx.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
                                var user = await db.Users.Where(o => o.Email == emailClaim).FirstOrDefaultAsync();

                                if (user == null)
                                {
                                    user = new User
                                    {
                                        Email = emailClaim,

                                        // TODO reconsider this long term
                                        IsAdmin = true
                                    };
                                    db.Users.Add(user);
                                }

                                user.Name = nameClaim;

                                await db.SaveChangesAsync();

                                if (user.IsAdmin)
                                {
                                    var appIdentity = new ClaimsIdentity(new List<Claim>
                                        {
                                        new Claim(ClaimTypes.Role, "Admin")
                                        });

                                    ctx.Principal.AddIdentity(appIdentity);
                                }
                            }
                    };
                });
        }
    }
}
