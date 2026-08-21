using VenueBook.Domain.Services;
using Microsoft.EntityFrameworkCore;
using VenueBook.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VenueBookDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSingleton<IPricingCalculator, PricingCalculator>();

var app = builder.Build();
