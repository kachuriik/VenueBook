using VenueBook.Domain.Services;
var builder = WebApplication.CreateBuilder(args);

// Реєструємо калькулятор як Singleton, оскільки він не має стану й виконує лише математичні операції
builder.Services.AddSingleton<IPricingCalculator, PricingCalculator>();

var app = builder.Build();
