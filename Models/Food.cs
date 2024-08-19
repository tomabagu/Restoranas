using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class Food : Meal
    {
        public Food(int id, string name, decimal price, string description, string comments, bool isVegetarian) : base(id, name, price, description, comments)
        {
            IsVegetarian = isVegetarian;
        }

        public bool IsVegetarian { get; set; }

        public static Food FromStringToFoodObject(string line)
        {
            var data=line.Split(',');
            return new Food(int.Parse(data[0]), (data[1]), decimal.Parse(data[2]), string.Empty, string.Empty, bool.Parse(data[3]));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Food other = (Food)obj;
            return Id == other.Id &&
                   Name == other.Name &&
                   Price == other.Price &&
                   Description == other.Description &&
                   Comments == other.Comments &&
                   IsVegetarian == other.IsVegetarian;
        }

    }
}
