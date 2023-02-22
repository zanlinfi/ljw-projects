using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityClass
{
    public class JwtOptions
    {
        public string SigningKey { get; set; }
        public int ExpireSeconds { get; set; }
    }
}
