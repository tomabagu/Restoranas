using Restoranas.Interfaces;
using Restoranas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Repository
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly string _filePathDrinks;

        public DrinkRepository(string filePathDrinks)
        {
            _filePathDrinks = filePathDrinks;
        }

        public List<Drink> GetAllDrinks()
        {
            List<Drink> drinks = new List<Drink>();
            try
            {
                string[] drinksLines = File.ReadAllLines(_filePathDrinks);
                foreach (string drinkLine in drinksLines)
                {
                    drinks.Add(Drink.FromStringToDrinksObject(drinkLine));
                }
                return drinks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
