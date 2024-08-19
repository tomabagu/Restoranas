using Restoranas.Interfaces;
using Restoranas.Models;
using Restoranas.Repository;
using Restoranas.Services;
using System.Runtime.CompilerServices;

namespace Restoranas
{
    public class Program
    {
        static void Main(string[] args)
        {
            ILoggerService logger = new LoggerService("../../../logs.txt");
            IFoodRepository foodRepository = new FoodRepository("../../../food.txt");
            IDrinkRepository drinkRepository = new DrinkRepository("../../../drinks.txt");
            IEmailService emailService = new EmailService("smtp.gmail.com", 587, "restoranascodeacademy@gmail.com", "rcwv xlkk zmsf komd"); //pašto serverio konfigūracija
            IReservationService reservationService = new ReservationService(logger, emailService);
            IOrderService orderService = new OrderService(logger, emailService);
           
            ProjectRestaurant restoranas = new ProjectRestaurant(foodRepository, drinkRepository, reservationService, orderService);
            restoranas.Run();
        }
    }
}
