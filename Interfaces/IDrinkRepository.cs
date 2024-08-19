using Restoranas.Models;

namespace Restoranas.Interfaces
{
    public interface IDrinkRepository
    {
        List<Drink> GetAllDrinks();
    }
}