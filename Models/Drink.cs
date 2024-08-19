using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class Drink : Meal
    {
        public Drink(int id, string name, decimal price, string description, string comments, double capacity, bool isAlcoholic) : base(id, name, price, description, comments)
        {
            Capacity = capacity;
            IsAlcoholic = isAlcoholic;
        }
        public double Capacity { get; set; }
        public bool IsAlcoholic { get; set; }

        public static Drink FromStringToDrinksObject(string line)
        {
            var data=line.Split(',');
            return new Drink(int.Parse(data[0]), data[1], decimal.Parse(data[2]), string.Empty, string.Empty, double.Parse(data[3]), bool.Parse(data[4]));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Drink other = (Drink)obj;
            return Id == other.Id &&
                   Name == other.Name &&
                   Price == other.Price &&
                   Description == other.Description &&
                   Comments == other.Comments &&
                   Capacity == other.Capacity &&
                   IsAlcoholic == other.IsAlcoholic;
        }
    }
}
