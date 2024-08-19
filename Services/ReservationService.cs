using Restoranas.Interfaces;
using Restoranas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Restoranas.Services
{
    public class ReservationService : IReservationService
    {
        private readonly List<ILoggerService> _loggers;
        private IEmailService EmailSenderService;

        public ReservationService(ILoggerService logger, IEmailService emailService)
        {
            _loggers = new List<ILoggerService> { logger };
            EmailSenderService = emailService;
        }

        public Table ReserveTable(Table table, DateTime reserveTime, string customerName, int numberOfPeople, string email)
        {
            if (table == null)
            {
                return table;
            }
            foreach (var reservation in table.Reservations)
            {
                if (Math.Abs((reservation.ReservationDate - reserveTime).TotalHours) < 2)
                {
                    return table; // Reservation not allowed within 2 hours of another reservation
                }
            }
            string reservationTime = reserveTime.ToString("yyyy-MM-dd HH:mm:ss");
            Log($"Table reserved {table.TableNumber} time: {reservationTime} customer name: {customerName} number of people {numberOfPeople}");
            table.Reservations.Add(new Reservation(customerName, reserveTime, numberOfPeople, email));
            if (!string.IsNullOrWhiteSpace(email))
            {
                EmailSenderService.SendEmail(email, "Table reservation", $"Table nr.: {table.TableNumber} reserved for time: {reservationTime} guest count: {numberOfPeople} name of reservation: {customerName}");
                Log($"Email sent to {email} for reservation. Table {table.TableNumber} reservation time: {reservationTime}");
            }
            return table;
        }
        public Table CancelReservation(Table table, int reservationIndex)
        {
            if (!(reservationIndex >= 0 && reservationIndex < table.Reservations.Count))
            {
                return table;
            }
            Reservation reservation = table.Reservations[reservationIndex];
            string reservationTime = reservation.ReservationDate.ToString("yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrWhiteSpace(reservation.Email))
            {
                EmailSenderService.SendEmail(reservation.Email, "Table reservation cancel for", $"Table nr.: {table.TableNumber} reserved for time: {reservationTime} guest count: {reservation.NumberOfPeople} name of reservation: {reservation.CustomerName}");
                Log($"Email sent to {reservation.Email} for reservation cancel. Table {table.TableNumber} reservation time: {reservationTime}");
            }
            Log($"Reservation canceled for table {table.Reservations[reservationIndex].ToString()}");
            table.Reservations.RemoveAt(reservationIndex);
            return table;
        }

        private void Log(string message) => _loggers.ForEach(logger => logger.Log(message));
    }
}
