using Restoranas.Interfaces;
using Restoranas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Services
{
    public class OrderService : IOrderService
    {
        private readonly List<ILoggerService> _loggers;
        private IEmailService EmailSenderService;

        public OrderService(ILoggerService logger, IEmailService emailService)
        {
            _loggers = new List<ILoggerService> { logger };
            EmailSenderService = emailService;
        }

        public Table OrderMeal(Table table, Meal meal)
        {
            if (meal == null)
            {
                return table;
            }
            table.Order.Meals.Add(meal);
            table.IsReserved = true;
            Log($"Meal ordered: {meal.Id} {meal.Name} {meal.Price} {meal.Comments} Table: {table.TableNumber} Waiter: {table.Waiter}");
            return table;
        }

        public Table DeleteMealFromOrder(Table table, int index)
        {
            if (!(index >= 0 && index < table.Order.Meals.Count)) {
                return table;
            }
            Log($"Meal removed from table: {table.Order.Meals[index].ToString()} Table: {table.TableNumber} Waiter: {table.Waiter}");
            table.Order.Meals.RemoveAt(index);
            /*if (table.Order.Meals.Count == 0)
            {
                table.IsReserved = false;
            }*/
            return table;
        }

        public Table PayOrder(Table table, string email, Restaurant restaurant)
        {
            if (table == null)
            {
                throw new ArgumentNullException();
            }
            string orderDetails = GetOrderDetails(table, restaurant);
            if (!string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Sending email for client");
                EmailSenderService.SendEmail(email, "Payment check", orderDetails);
            }
            Console.WriteLine("Sending email for restaurant");
            EmailSenderService.SendEmail("restoranascodeacademy@gmail.com", "Restaurant check", GetOrderDetails(table, null));
            Log(orderDetails);
            table.Order = new Order();
            table.IsReserved = false;
            return table;
        }

        private string GetOrderDetails(Table table, Restaurant? restaurant)
        {
            var sb = new StringBuilder();
            decimal totalPrice = 0;
            sb.AppendLine("----------------------------------------------------------------------------------");
            if (restaurant != null)
            {
                sb.AppendLine(restaurant.Name + " " + restaurant.Address);
            }
            sb.AppendLine("Order paid");
            sb.AppendLine($"Table number: {table.TableNumber} waiter: {table.Waiter}");
            sb.AppendLine();

            foreach (var meal in table.Order.Meals)
            {
                sb.AppendLine($"{meal.Name} - {meal.Price} eur.");
                totalPrice += meal.Price;
            }
            sb.AppendLine();
            sb.AppendLine($"Total price: {totalPrice} eur.");
            sb.AppendLine("----------------------------------------------------------------------------------");
            return sb.ToString();
        }

        private void Log(string message) => _loggers.ForEach(logger => logger.Log(message));
    }
}
