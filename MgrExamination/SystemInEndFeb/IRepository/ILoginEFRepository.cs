using EntityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface ILoginEFRepository
    {
        Task<string> CreateUserRoleAsync(User user);
        Task<User> LoginAsync(LoginRequest loginRequest);
    }
}
