using Microsoft.EntityFrameworkCore;
using car_rent_api2.Server.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5172") // Your frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Configuration.AddEnvironmentVariables();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_WYSZUKIWARKA");
builder.Services.AddDbContext<SearchEngineDbContext>(options =>
    options.UseSqlServer(connectionString));

var car_rent_company_api1 = Environment.GetEnvironmentVariable("DOTNET_CARRENT_API1");
builder.Services.AddSingleton<string>(car_rent_company_api1);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowFrontend"); // Use the defined CORS policy

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

await app.RunAsync();

