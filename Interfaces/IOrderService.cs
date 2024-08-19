using Restoranas.Models;

namespace Restoranas.Interfaces
{
    public interface IOrderService
    {
        Table DeleteMealFromOrder(Table table, int index);
        Table OrderMeal(Table table, Meal meal);
        Table PayOrder(Table table, string email, Restaurant restaurant);
    }
}