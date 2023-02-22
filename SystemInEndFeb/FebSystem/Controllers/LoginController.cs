using EFCore.Db;
using EntityClass;
using FebSystem.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FebSystem.Controllers
{
    
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly EFProvider ctx;

        public LoginController(UserManager<User> userManager, EFProvider ctx)
        {
            this.userManager = userManager;
            this.ctx = ctx;
        }

        [HttpPost]
        [Route("api/Login/createUserRole")]
        public async Task<IActionResult> CreateUserRole([FromBody]User user)
        {
            var res = await ctx.Ids.CreateUserRoleAsync(user);
            if (res.Equals("failure"))
            {
                return BadRequest(res);
            }
            return Ok(res);

        }


        [HttpPost]
        [Route("api/Login/login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest req,
                                               [FromServices] IOptions<JwtOptions> jwtOpts)
        {
            var user = await ctx.Ids.LoginAsync(req);
            string userName = user.UserName;
            //string password = user.UserName;
            if (userName.Equals("null"))
            {
                return NotFound($"not found {req.UserName}");
            }
            
            if (userName.Equals("error"))
            {
                return BadRequest("login failure");
            }
            
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userName));
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            string jwtToken = BuildTokenHelper.GetToken(claims, jwtOpts.Value);
            return Ok(jwtToken);
        }


    }
}
