using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Domain.Services;
using IKT_BACKEND.Persistence.Context;
using IKT_BACKEND.Persistence.Repositories;
using IKT_BACKEND.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Configure postgres connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresConnection")
    )
);

// Configure Cotrollers
builder.Services.AddControllers();

// Configure Services
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRespository,  SalesRespository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();


app.Run();
