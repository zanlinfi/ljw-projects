using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityClass
{
    public class BookRequest
    {
        public string? Title { get; set; }
        
        public double? Price { get; set; }
        
        public string? AuthorName { get; set; }
    }
}
