namespace AuthMicro.Product;

public class Product
{
    public string Model {get; set;} = string.Empty;
    public string Brand {get; set;} = string.Empty;
    public float Price {get; set;}

    public Product(string model, string brand, float price)
    {
        Model = model;
        Brand = brand;
        Price = price;
    }
}
