public class Product 

{
    public int Id;
    public string Name;
    public int CategoryId;
    public float Price;
    public int Stock;

    public Product(int id, string name, int categoryId, float price, int stock)
    {
        Id = id;
        Name = name;
        CategoryId = categoryId;
        Price = price;
        Stock = stock;
    }

    public Product(string name, int categoryId, float price, int stock)
    {
        Name = name;
        CategoryId = categoryId;
        Price = price;
        Stock = stock;
    }
}
