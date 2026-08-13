namespace VenueBook.Domain.Services;

public interface IPricingCalculator
{
    // Розрахунок загальної вартості бронювання з урахуванням часу та додаткових послуг
    decimal Calculate(decimal baseHourlyRate, DateTime startTime, DateTime endTime, IEnumerable<decimal> servicePrices);
}