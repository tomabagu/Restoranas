using Restoranas.Models;

namespace Restoranas.Interfaces
{
    public interface IReservationService
    {
        Table CancelReservation(Table table, int reservationIndex);
        Table ReserveTable(Table table, DateTime reserveTime, string customerName, int numberOfPeople, string email);
    }
}