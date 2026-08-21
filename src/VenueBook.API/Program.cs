using VenueBook.Domain.Services;
using Microsoft.EntityFrameworkCore;
using VenueBook.Infrastructure.Data;
using VenueBook.Application.Interfaces;
using VenueBook.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<VenueBookDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSingleton<IPricingCalculator, PricingCalculator>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

var app = builder.Build();
