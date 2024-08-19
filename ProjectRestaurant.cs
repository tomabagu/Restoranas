using Restoranas.Interfaces;
using Restoranas.Models;
using Restoranas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas
{
    public class ProjectRestaurant
    {
        private IFoodRepository foodRepository;
        private IDrinkRepository drinkRepository;
        private IReservationService reservationService;
        private IOrderService orderService;

        private Table? selectedTable;
        private MealMenu? mealMenu;

        public ProjectRestaurant(IFoodRepository foodRepository, IDrinkRepository drinkRepository, IReservationService reservationService, IOrderService orderService)
        {
            this.foodRepository = foodRepository;
            this.drinkRepository = drinkRepository;
            this.reservationService = reservationService;
            this.orderService = orderService;
        }

        public void Run()
        {
            List<Table> tables = GetAllTablesConfiguration(); // stalelio konfiguracija
            mealMenu = GetMealMenu(); // išsaugome į globalų kintamajį ir galėsime prie maisto meniu prieiti iš bet kurios kodo vietos
            Restaurant restaurant = new Restaurant(tables, "Restaurant project", "Restorano g. 5, Vilnius"); // deklaruojamas ir inicijuojamas restorano objektas
            var choice = 0;
            while (true) // pagrindinis meniu
            {
                do
                {
                    Console.Clear();
                    PrintSelectedTable();
                    Console.WriteLine();

                    Console.WriteLine("1. Select table");
                    Console.WriteLine("2. Order meal");
                    Console.WriteLine("3. Order drink");
                    Console.WriteLine("4. Reserve table");
                    Console.WriteLine("5. List table reservations");
                    Console.WriteLine("6. Cancel reservation");
                    Console.WriteLine("7. Table orders");
                    Console.WriteLine("8. Remove table order");
                    Console.WriteLine("9. Table order payment");
                } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 9);
                switch (choice)
                {
                    case 1:
                        ShowAllTables(restaurant.Tables);
                        selectedTable = SelectTable(restaurant.Tables);
                        break;
                    case 2:
                        OrderFood(restaurant.Tables);
                        break;
                    case 3:
                        OrderDrinks(restaurant.Tables);
                        break;
                    case 4:
                        TableReservation(restaurant.Tables);
                        break;
                    case 5:
                        ShowAllTables(restaurant.Tables);
                        ShowTableReservations(SelectTable(restaurant.Tables));
                        break;
                    case 6:
                        CancelReservation(restaurant.Tables);
                        break;
                    case 7:
                        GetCurrentTableOrder(selectedTable);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case 8:
                        RemoveMealItemFromTableOrder(selectedTable);
                        break;
                    case 9:
                        PayForTableOrder(selectedTable, restaurant);
                        break;
                    default:
                        break;
                }
            }
        }

        private List<Table> GetAllTablesConfiguration() // Restorano staleliu nustatymai
        {
            List<Table> tables = new List<Table>{
                new Table(1, 6, false, new List<Reservation>(), new Order(), "Toma"),
                new Table(2, 6, false, new List<Reservation>(), new Order(), "Toma"),
                new Table(3, 12, false, new List<Reservation>(), new Order(), "Toma"),
                new Table(4, 6, false, new List<Reservation>(), new Order(), "Simona"),
                new Table(5, 6, false, new List<Reservation>(), new Order(), "Simona"),
                new Table(6, 6, false, new List<Reservation>(), new Order(), "Simona"),
                new Table(7, 12, false, new List<Reservation>(), new Order(), "Simona"),
                new Table(8, 6, false, new List<Reservation>(), new Order(), "Petras"),
                new Table(9, 6, false, new List<Reservation>(), new Order(), "Petras"),
                new Table(10, 12, false, new List<Reservation>(), new Order(), "Petras"),
            };
            return tables;
        }

        private MealMenu GetMealMenu() //Sukeliami maistas ir gėrimai iš failų į MealMenu objektą
        {
            MealMenu mealMenu = new MealMenu();
            mealMenu.Foods = foodRepository.GetAllFood();
            mealMenu.Drinks = drinkRepository.GetAllDrinks();
            return mealMenu;
        }

        private void PrintFoodMenu() //Atspausdinti maisto meniu
        {
            List<Food> foodList = mealMenu.Foods;
            Console.WriteLine("------Food menu------");
            foreach (Food foodItem in foodList)
            {
                Console.WriteLine($"{foodItem.Id} {foodItem.Name} {foodItem.Price}eur");
            }
        }

        private void PrintDrinksMenu() //Atspausdinti gėrimų meniu
        {
            List<Drink> drinkList = mealMenu.Drinks;
            Console.WriteLine("------Drinks menu------");
            foreach (Drink drink in drinkList)
            {
                Console.WriteLine($"{drink.Id} {drink.Name} {drink.Capacity}l {drink.Price}eur");
            }
        }

        private void RemoveMealItemFromTableOrder(Table table) //pašalinti maisto, gerimų užsakymą staleliui
        {
            GetCurrentTableOrder(table);
            if (table != null && table.Order.Meals.Count > 0)
            {
                int input = InputFromKeyboard("Select order number from list to remove", table.Order.Meals.Count);
                orderService.DeleteMealFromOrder(table, input - 1);
            }
        }

        private void GetCurrentTableOrder(Table table) //Pasirinkto stalelio užsakymai
        {
            if (table == null)
            {
                Console.WriteLine("Table not selected");
                return;
            }
            Console.WriteLine($"Table number {table.TableNumber}, Current order list:");
            int index = 1;
            foreach (Meal orderItem in table.Order.Meals)
            {
                Console.WriteLine($"{index} {orderItem.Name} {orderItem.Price}eur {orderItem.Comments}");
                index++;
            }
        }

        private double GetTotalPayForOrder(Table table) // Salelio užsakymų suma
        {
            double price = 0;
            foreach (var item in table.Order.Meals)
            {
                price += ((double)item.Price);
            }
            return price;
        }

        private void ShowAllTables(List<Table> tables) //Išspausdina visus stalelius
        {
            foreach (Table table in tables)
            {
                string reserved = table.IsReserved ? "Reserved" : "Not Reserved";
                Console.WriteLine($"Table number: {table.TableNumber} number of seats: {table.NumberOfSeats} status: {reserved} waiter:  {table.Waiter} | reservation count: {table.Reservations.Count} | meals ordered: {table.Order.Meals.Count}");
            }
        }

        private string FormatDateTime(DateTime? dateTime) //Datos formatavimas yyyy-MM-dd HH:mm:ss
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            } else
            {
                return string.Empty;
            }
        }

        private int InputFromKeyboard(string message, int maxVal) //Įvestis iš klavieturos su patikrinimu, kai įvestis turi būti skaičius
        {
            int choice = 0;
            do
            {
                Console.WriteLine(message);
            } while (!int.TryParse(Console.ReadLine(), out choice) || maxVal == 0 || (choice <= 0 || choice > maxVal));
            return choice;
        }

        private DateTime DateInputFromKeyboard(string message) //Datos įvedimas iš klavietūros su patikrinimu ar įvesta validi data
        {
            DateTime date;
            do
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();

                if (DateTime.TryParse(input, out date))
                {
                    if (date > DateTime.Now)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("The entered date and time must be after the current date and time. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date and time format. Please try again.");
                }
            } while (true);

            return date;
        }

        private string EmailInputFromKeiboard(string message) //El pašto įvestis su patikrinimu ar validus
        {
            string email;
            do
            {
                Console.WriteLine(message);
                email = Console.ReadLine();
            } while (!string.IsNullOrEmpty(email) && !EmailService.IsValidEmail(email));
            return email;
        }

        public void TableReservation(List<Table> tables) { //Stalelio rezervacija
            ShowAllTables(tables);
            int choice = InputFromKeyboard("Please choose table number for reservation", tables.Count);
            Table table = tables.FirstOrDefault(t => t.TableNumber == choice);
            DateTime date;
            do
            {
                date = DateInputFromKeyboard("Enter a date and time (yyyy-MM-dd HH:mm): ");
            } while (!CheckReservationDate(table, date));
            CheckReservationDate(table, date);
            Console.WriteLine("Enter name for reservation");
            string customerName = Console.ReadLine();
            int numberOfPeople = InputFromKeyboard("Enter number of people for table", 1000);
            string email = EmailInputFromKeiboard("Enter email");
            if (numberOfPeople > table.NumberOfSeats)
            {
                Console.WriteLine("Number of people exceeds max seats, please reserve one more table or bigger table");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            reservationService.ReserveTable(table, date, customerName, numberOfPeople, email);
        }

        private bool CheckReservationDate(Table table, DateTime date) // stalelio rezervacijos validacija, negali būti tarpas mažesnis nei 2 val nuo kitų rezervacijų to stalelio
        {
            foreach (var reservation in table.Reservations)
            {
                if (Math.Abs((reservation.ReservationDate - date).TotalHours) < 2)
                {
                    Console.WriteLine("Reservation time must be two hours after previous reservation");
                    return false;
                }
            }
            return true;
        }

        public void ShowTableReservations(Table table) //Stalelio rezervacijos išspausdinimas konsolėje
        {
            Console.WriteLine($"Table number {table.TableNumber}");
            int count = 1;
            foreach (var reservation in table.Reservations)
            {
                Console.WriteLine($"{count}. Date: {FormatDateTime(reservation.ReservationDate)} Reservation name: {reservation.CustomerName} Number of people: {reservation.NumberOfPeople}");
                count++;
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public void PrintSelectedTable() //Pasirinkto stalelio atvaizdavimas konsolėje
        {
            if (selectedTable != null)
            {
                Console.WriteLine($"Table selected: {selectedTable.TableNumber}");
            }
        }

        public Table? SelectTable(List<Table> tables) //Pasirinkti stalelį
        {
            int input = InputFromKeyboard("Select table number", tables.Count);
            Table table = tables.Where(t => t.TableNumber == input).SingleOrDefault() ;
            table.IsReserved = true;
            if (selectedTable != null && selectedTable.TableNumber != table.TableNumber && selectedTable != null && selectedTable.Order.Meals.Count == 0)
            {
                selectedTable.IsReserved = false;
            }
            return table;
        }

        public void OrderFood(List<Table> tables) //Maisto užsakymas
        {
            if (selectedTable == null)
            {
                selectedTable = SelectTable(tables);
            }
            Console.Clear();
            PrintSelectedTable();
            PrintFoodMenu();
            int input = InputFromKeyboard("Please select meal number from menu", mealMenu.Foods.Count);
            Food food = mealMenu.Foods.Where(f => f.Id == input).SingleOrDefault();
            orderService.OrderMeal(selectedTable, food);
        }

        public void OrderDrinks(List<Table> tables) //Gėrimo užsakymas
        {
            if (selectedTable == null)
            {
                selectedTable = SelectTable(tables);
            }
            Console.Clear();
            PrintSelectedTable();
            PrintDrinksMenu();
            int input = InputFromKeyboard("Please select drink number", mealMenu.Drinks.Count);
            Drink drink = mealMenu.Drinks.Where(d => d.Id == input).SingleOrDefault();
            orderService.OrderMeal(selectedTable, drink);
            
         }

        public void CancelReservation(List<Table> tables) //Rezervacijos atšaukimas
        {
            ShowAllTables(tables);
            int choice = InputFromKeyboard("Please choose table number for reservation cancel", tables.Count);
            Table table = tables.FirstOrDefault(t => t.TableNumber == choice);
            if (table.Reservations.Count == 0)
            {
                Console.WriteLine("Nothing to cancel for this table");
                Console.ReadKey();
                return;
            }
            ShowTableReservations(table);
            int choiceReservation = InputFromKeyboard("Please choose reservation number to cancel", table.Reservations.Count);
            reservationService.CancelReservation(table, choiceReservation - 1);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private Table? PayForTableOrder(Table? selectedTable, Restaurant restaurant) //Apmokėjimas stalelio
        {
            if (selectedTable != null)
            {
                if (selectedTable.Order.Meals.Count == 0)
                {
                    Console.WriteLine("Order list empty nothing to pay for");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return selectedTable;
                }
                GetCurrentTableOrder(selectedTable);
                Console.WriteLine($"Total amount to pay: {GetTotalPayForOrder(selectedTable)}eur");
                string email = EmailInputFromKeiboard("Enter email for recipe");
                Console.WriteLine("Press any key to PAY");
                Console.ReadKey();
                return orderService.PayOrder(selectedTable, email, restaurant);
            }
            else
            {
                Console.WriteLine("Table not selected");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();

            }
            return selectedTable;
        }
    }
}
