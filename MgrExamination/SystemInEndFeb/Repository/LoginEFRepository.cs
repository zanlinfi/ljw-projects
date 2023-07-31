using EntityClass;
using IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class LoginEFRepository : ILoginEFRepository
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;

        public LoginEFRepository(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<string> CreateUserRoleAsync(User user)
        {
            string rolestr = "admin";
            bool roleExists = await roleManager.RoleExistsAsync(nameof(rolestr));
            if (!roleExists)
            {
                Role role = new Role { Name = nameof(rolestr) };
                var r = await roleManager.CreateAsync(role);
                if (!r.Succeeded)
                {
                    return "failure";
                }
            }
            User userNew = await this.userManager.FindByNameAsync(user.UserName);
            if (userNew == null)
            {
                userNew = user;
                var u = await userManager.CreateAsync(userNew, "123456");
                if (!u.Succeeded)
                {
                    return "failure";
                }
                u = await userManager.AddToRoleAsync(userNew, nameof(rolestr));
                if (!u.Succeeded)
                {
                    return "failure";
                }
            }
            return "create success";
        }

        public async Task<User> LoginAsync(LoginRequest req)
        {
            string userName = req.Username;
            string password = req.Password;
            var user = await userManager.FindByNameAsync(userName);
            
            if (user == null)
            {
                return new User { UserName="null"};
            }
            var success = await userManager.CheckPasswordAsync(user, password);
            if (!success)
            {
                return new User { UserName="error"};
            }

            return user;
        }
    }
}
