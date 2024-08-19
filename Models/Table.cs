using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class Table
    {
        public Table(int tableNumber, int numberOfSeats, bool isReserved, List<Reservation> reservations, Order order, string waiter)
        {
            TableNumber = tableNumber;
            NumberOfSeats = numberOfSeats;
            IsReserved = isReserved;
            Reservations = reservations;
            Order = order;
            Waiter = waiter;
        }

        public int TableNumber { get; set; }
        public int NumberOfSeats { get; set; }
        public bool IsReserved { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public Order Order { get; set; }
        public string Waiter { get; set; }

    }
}
