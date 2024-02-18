using AuthMicro.Product;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddAuthorizationBuilder();

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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

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
