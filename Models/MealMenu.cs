using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class MealMenu
    {
        public List<Food> Foods { get; set; } = new List<Food>();
        public List<Drink> Drinks { get; set; } = new List<Drink>();
    }
}
