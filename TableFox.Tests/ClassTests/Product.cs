using TableFox.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableFox.Test.ClassTests
{
    public class Product
    {
        [PrimaryKey]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public BusinessProduct BusinessProduct { get; set; }

        public List<ComponentTer> Components { get; set; }
    }
}
