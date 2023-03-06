using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityClass
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int State { get; set; }
        public string content { get; set; }
    }
}
