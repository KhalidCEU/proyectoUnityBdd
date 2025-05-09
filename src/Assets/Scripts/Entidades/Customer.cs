public class Customer 
{
    public int Id;
    public string Name;
    public string Email;
    public string PhoneNumber;
    public string Address;

    public Customer(int id, string name, string email, string phoneNumber, string address)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public Customer(string name, string email, string phoneNumber, string address)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
    }
}