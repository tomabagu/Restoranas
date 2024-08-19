using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class Order
    {
        public Order() { }
        public Order(int orderId, int numberOfPeople, DateTime orderDate, List<Meal> meals)
        {
            OrderId = orderId;
            NumberOfPeople = numberOfPeople;
            OrderDate = orderDate;
            Meals = meals;
        }

        public int OrderId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Meal> Meals { get; set; } = new List<Meal>();



    }
}
