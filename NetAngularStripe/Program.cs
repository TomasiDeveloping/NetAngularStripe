using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data;
using NetAngularStripe.Helper;
using NetAngularStripe.Interfaces;
using NetAngularStripe.Repositories;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
StripeConfiguration.ApiKey = builder.Configuration["StripeSettings:PrivateKey"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

builder.Services.AddDbContext<ApplicationContext>(options => { options.UseInMemoryDatabase("StripeTestDb"); });

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ILicenseTypeRepository, LicenseTypeRepository>();
builder.Services.AddScoped<IStripeCheckoutRepository, StripeCheckoutRepository>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    //3. Get the instance of BoardGamesDBContext in our services layer
    var services = scope.ServiceProvider;
    //4. Call the DataGenerator to create sample data
    DataGenerator.Initialize(services);
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();