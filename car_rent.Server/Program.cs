<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;

=======
>>>>>>> origin/master
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
<<<<<<< HEAD
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<car_rent_api2.Server.Database.SearchEngineDbContext>(options =>
    options.UseSqlServer(connectionString));
=======

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

>>>>>>> origin/master
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

<<<<<<< HEAD
app.Run();
=======
await app.RunAsync();

>>>>>>> origin/master
