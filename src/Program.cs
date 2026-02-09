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

// Configure Cors for frontend development
var frontendUrl = builder.Configuration["FRONTEND_URL"];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();

app.Run();
