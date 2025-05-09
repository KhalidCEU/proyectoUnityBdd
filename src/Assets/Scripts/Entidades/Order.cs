public class Order
{
    public int Id;
    public string Date; // Almacenar como string por simplicidad
    public int CustomerId;
    public float TotalAmount;

    public Order(int id, string date, int customerId, float totalAmount)
    {
        Id = id;
        Date = date;
        CustomerId = customerId;
        TotalAmount = totalAmount;
    }

    public Order(string date, int customerId, float totalAmount)
    {
        Date = date;
        CustomerId = customerId;
        TotalAmount = totalAmount;
    }
}
