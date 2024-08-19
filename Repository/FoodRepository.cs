using Restoranas.Interfaces;
using Restoranas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly string _filePathFood;

        public FoodRepository(string filePathFood)
        {
            _filePathFood = filePathFood;
        }
        public List<Food> GetAllFood()
        {
            List<Food> food = new List<Food>();
            try
            {
                string[] foodLines = File.ReadAllLines(_filePathFood);
                foreach (string line in foodLines)
                {
                    food.Add(Food.FromStringToFoodObject(line));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return food;
        }
    }
}
