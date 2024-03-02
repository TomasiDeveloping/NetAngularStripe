using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Helper;
using NetAngularStripe.Interfaces;
using NetAngularStripe.Repositories;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Set Stripe API key from configuration
StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:PrivateKey"];

// Add services to the container.
builder.Services.AddControllers();

// Add support for Swagger/OpenAPI documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HttpContextAccessor to access HttpContext in services
builder.Services.AddHttpContextAccessor();

// Configure Stripe settings from configuration
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

// Configure CORS to allow requests from any origin
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

// Configure in-memory database for application context
builder.Services.AddDbContext<ApplicationContext>(options => { options.UseInMemoryDatabase("StripeTestDb"); });

// Register repositories with dependency injection container
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ILicenseTypeRepository, LicenseTypeRepository>();
builder.Services.AddScoped<IStripeCheckoutRepository, StripeCheckoutRepository>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI for development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    // Access services within a scoped context
    var services = scope.ServiceProvider;

    // Initialize sample data using DataGenerator
    DataGenerator.Initialize(services);
}

// Enable CORS middleware
app.UseCors();

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable authorization middleware
app.UseAuthorization();

// Map controllers to HTTP requests
app.MapControllers();

// Start the application
app.Run();