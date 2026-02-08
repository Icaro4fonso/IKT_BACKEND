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
builder.Services.AddScoped<IProductService, ProductService>();

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesRespository,  SalesRespository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Allow cors for frontend development

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();

app.Run();
