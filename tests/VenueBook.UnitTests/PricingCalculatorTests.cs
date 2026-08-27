using VenueBook.Domain.Services;

namespace VenueBook.UnitTests.Domain;

public class PricingCalculatorTests
{
    [Fact]
    public void CalculatePrice_ShouldReturnCorrectTotal_ForStandardBooking()
    {
        // Arrange
        var calculator = new PricingCalculator();
        decimal hourlyRate = 500m;
        
        DateTime startTime = new DateTime(2026, 8, 27, 14, 0, 0);
        DateTime endTime = new DateTime(2026, 8, 27, 17, 0, 0);
        
        var services = new List<decimal>(); 
        
        var totalPrice = calculator.Calculate(hourlyRate, startTime, endTime, services);
        
        Assert.Equal(1500m, totalPrice); 
    }
}