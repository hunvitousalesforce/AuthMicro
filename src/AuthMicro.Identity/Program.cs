using AuthMicro.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddJwt(builder.Configuration);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AppIdentityDbContext>(options => { options.UseNpgsql(builder.Configuration.GetConnectionString("Default")); });
builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddScoped<IAuthentication, AuthenticationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddDefaultServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/api/v1/auth/login", async ([FromBody] LoginRequest request, IAuthentication authManager) =>
{
    if (request == null)
        return Results.BadRequest();
    var result = await authManager.LoginAsync(request);
    return Results.Ok(result);
});

app.MapPost("/api/v1/auth/register", async ([FromBody] RegisterRequest request, IAuthentication authManager) =>
{
    if (request == null)
        return Results.BadRequest();
    var result = await authManager.RegisterAsync(request);
    return Results.Ok(result);
});

var products = new List<Product>()
{
    new Product("Apple", "M1 Air", 1200.0f),
    new Product("MSI", "TF 200", 700f)
};

app.MapGet("/products", () =>
{
    return products;
}).RequireAuthorization();


app.Run();



public class Product
{
    public string Model { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public float Price { get; set; }

    public Product(string model, string brand, float price)
    {
        Model = model;
        Brand = brand;
        Price = price;
    }
}


