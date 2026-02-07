using IKT_BACKEND.Domain.Services;
using IKT_BACKEND.Persistence.Context;
using IKT_BACKEND.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure postgres connection
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresConnection")
    )
);

// Configure Cotrollers
builder.Services.AddControllers();

// Configure Services
builder.Services.AddScoped<ISalesService, SalesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();


app.Run();
