using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DotvvmMvcIntegration.DotVVM.ViewModels
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AdminPageViewModel : DotvvmViewModelBase
    {
        private readonly IOptions<CustomJwtOptions> jwtOptions;

        public string Token { get; set; }

        public AdminPageViewModel(IOptions<CustomJwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions;
        }

        public void GenerateJwt()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "apiuser"),
                new Claim(ClaimTypes.Name, "apiuser")
            };
            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Value.Key)), SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(jwtOptions.Value.Issuer, jwtOptions.Value.Audience, claims, 
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: credentials);
            Token = tokenHandler.WriteToken(token);
        }

        public async Task SignOut()
        {
            await Context.GetAuthentication().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Context.RedirectToRoute("DotvvmSample");
        }
    }
}

