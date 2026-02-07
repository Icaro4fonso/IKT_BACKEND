var builder = WebApplication.CreateBuilder(args);

// Configure Cotrollers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();


app.Run();
