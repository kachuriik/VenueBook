namespace VenueBook.Domain.Services;

public class PricingCalculator : IPricingCalculator
{
    public decimal Calculate(decimal baseHourlyRate, DateTime startTime, DateTime endTime, IEnumerable<decimal> servicePrices)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Час початку має бути раніше за час завершення.");

        decimal totalRoomCost = 0;
        DateTime currentPointer = startTime;

        // Крок 1. Розрахунок вартості оренди залу по годинах
        while (currentPointer < endTime)
        {
            DateTime nextHour = currentPointer.Date.AddHours(currentPointer.Hour + 1);
            DateTime segmentEnd = nextHour < endTime ? nextHour : endTime;

            decimal segmentDuration = (decimal)(segmentEnd - currentPointer).TotalHours;

            decimal multiplier = GetTimeMultiplier(currentPointer.Hour);

            totalRoomCost += segmentDuration * baseHourlyRate * multiplier;

            currentPointer = segmentEnd;
        }

        // Крок 2. Додавання вартості послуг
        decimal totalServicesCost = servicePrices.Sum();

        return totalRoomCost + totalServicesCost;
    }

    // Повертає ціновий множник залежно від години дня
    private decimal GetTimeMultiplier(int hour)
    {
        return hour switch
        {
            >= 6 and < 9   => 0.90m, // Ранкові години (06:00 - 08:59): знижка 10%
            >= 12 and < 14 => 1.15m, // Пікові години (12:00 - 13:59): націнка 15%
            >= 9 and < 18  => 1.00m, // Стандартні години (09:00 - 11:59, 14:00 - 17:59): базова вартість
            >= 18 and < 23 => 0.80m, // Вечірні години (18:00 - 22:59): знижка 20%
            _              => 1.00m  // Нічний час (за замовчуванням, якщо дозволено)
        };
    }
}
