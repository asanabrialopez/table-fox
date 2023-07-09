using TableFox.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableFox.Test.ClassTests
{
    public class User
    {
        [PrimaryKey]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public BusinessUser Business { get; set; }
        public List<Product> Products { get; set; }
    }
}
