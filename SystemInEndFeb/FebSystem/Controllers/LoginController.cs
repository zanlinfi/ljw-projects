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
        [Route("Login/login")]
        public async Task<LoginResult> Login([FromBody]LoginRequest req,
                                               [FromServices] IOptions<JwtOptions> jwtOpts)
        {
            LoginResult result;
            var user = await ctx.Ids.LoginAsync(req);
            string userName = user.UserName;
            //string password = user.UserName;
            if (userName.Equals("null"))
            {
                result = new LoginResult() { Success=false, content="not found", Message="not found",State=404};
                return result;
            }
            
            if (userName.Equals("error"))
            {
                result = new LoginResult() { Success = false, content = "login failure", Message = "login failure", State = 400 };
                return result;
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
            result = new LoginResult() { Success = true, content = $"{{\"access_token\":\"Bearer {jwtToken}\", \"token_type\":\"bearer\"}}" , Message = "login success", State = 200 };
            return result;
        }


    }
}
