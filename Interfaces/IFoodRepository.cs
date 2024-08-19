using Restoranas.Models;

namespace Restoranas.Interfaces
{
    public interface IFoodRepository
    {
        List<Food> GetAllFood();
    }
}