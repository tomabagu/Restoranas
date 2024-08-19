using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class Restaurant
    {
        public Restaurant(List<Table> tables, string name, string address)
        {
            Tables = tables;
            Name = name;
            Address = address;
        }

        public List<Table> Tables { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
