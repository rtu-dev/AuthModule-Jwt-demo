using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CorporativeSystem.Controllers.Models.Data
{
    public class JWT
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public JWT(
             RoleManager<IdentityRole> roleManager,
             UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<UserToken> BuildTokenAsync(string login, string jwtKey)
        {
            var user = await _userManager.FindByEmailAsync(login);
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(await _userManager.GetClaimsAsync(user));


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,

                //Subject = new ClaimsIdentity(new[]
                //{
                //    new Claim(ClaimTypes.Name, user.Id)
                //}),

                //Issuer = _appSettings.Emissor,
                //Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                Expiration = tokenDescriptor.Expires,
                Authenticated = true
            };
        }
    }
}
