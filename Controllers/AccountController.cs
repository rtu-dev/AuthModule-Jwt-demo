using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorporativeSystem.Controllers.Models.Data;
using CorporativeSystem.Models.Data.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CorporativeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
             RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Login([FromBody] dynamic userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync((string)userInfo.Login, (string)userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {                
                JWT jwt = new JWT(_roleManager, _userManager);
                return await jwt.BuildTokenAsync((string)userInfo.Email, _configuration["jwt:key"]);
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }

        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> CreateUser([FromBody] dynamic patientViewModel)
        {
            string password = patientViewModel.Password;
            var identityUser = new IdentityUser { UserName = patientViewModel.UserName, Email = patientViewModel.Email };
            var result = await _userManager.CreateAsync(identityUser, password);

            if (result.Succeeded)
            {
                return Ok(new { resultMessage = result.ToString(), user = identityUser });
            }

            return BadRequest(new { resultMessage = result.ToString() });
        }





























        // GET: api/Account
        [HttpGet]
        [Authorize]
        [ClaimsAuthorize("Financeiro", "Delete")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    
        // GET: api/Account/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Account
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
