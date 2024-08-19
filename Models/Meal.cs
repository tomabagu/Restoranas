using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class Meal
    {
        public Meal(int id, string name, decimal price, string description, string comments)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            Comments = comments;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }

        public override string? ToString()
        {
            return $"Id: {Id} Name: {Name} Price: {Price} Description {Description} Comments {Comments}";
        }
    }
}
