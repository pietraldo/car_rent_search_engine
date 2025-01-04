using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using car_rent_api2.Server.Database;
using car_rent.Server.Model;
using car_rent.Server.Notifications;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Allow all origins
                  .AllowAnyHeader() // Allow any header
                  .AllowAnyMethod(); // Allow any method
        });
});

builder.Configuration.AddEnvironmentVariables();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_WYSZUKIWARKA") ?? throw new InvalidOperationException("Missing connection string");
builder.Services.AddDbContext<SearchEngineDbContext>(options =>
    options.UseSqlServer(connectionString));

var configuration = builder.Configuration;

var sessionCookieLifetime = builder.Configuration.GetValue("SessionCookieLifetimeMinutes", 60);

// Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_ID2") ?? throw new InvalidOperationException("Missing Google API client ID");
        googleOptions.ClientSecret = Environment.GetEnvironmentVariable("AUTHENTICATION_GOOGLE_SECRET2") ?? throw new InvalidOperationException("Missing Google API secret");
        googleOptions.CallbackPath = "/api/Identity/signin-google";
    });

builder.Services.AddAuthorization();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<SearchEngineDbContext>()
    .AddDefaultTokenProviders().AddApiEndpoints();

builder.Services.AddHttpLogging(o => { });

var car_rent_company_api1 = Environment.GetEnvironmentVariable("DOTNET_CARRENT_API1")?? throw new InvalidOperationException("Missing car rent company API URL");
builder.Services.AddSingleton<string>(car_rent_company_api1);
builder.Services.AddSingleton<INotificationService, MailGunService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseHttpLogging();

await app.RunAsync();