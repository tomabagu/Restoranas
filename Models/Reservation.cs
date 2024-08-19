using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Models
{
    public class Reservation
    {
        public Reservation(string customerName, DateTime reservationDate, int numberOfPeople, string email)
        {
            CustomerName = customerName;
            ReservationDate = reservationDate;
            NumberOfPeople = numberOfPeople;
            Email = email;
        }
        public string CustomerName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfPeople { get; set; }
        public string Email { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Reservation reservation &&
                   CustomerName == reservation.CustomerName &&
                   ReservationDate == reservation.ReservationDate &&
                   NumberOfPeople == reservation.NumberOfPeople &&
                   Email == reservation.Email;
        }

        public override string? ToString()
        {
            return $"Reservation date: {ReservationDate.ToString("yyyy-MM-dd HH:mm:ss")} customer name: {CustomerName} number of people: {NumberOfPeople}";
        }


    }
}
