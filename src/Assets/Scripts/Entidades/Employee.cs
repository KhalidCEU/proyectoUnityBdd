public class Employee
{
    public int Id;
    public string Name;
    public int PositionId;
    public float Salary;
    public string Email;
    public byte[] Photo;
    public int StoreId;

    public Employee(int id, string name, int positionId, float salary, string email, byte[] photo, int storeId)
    {
        Id = id;
        Name = name;
        PositionId = positionId;
        Salary = salary;
        Email = email;
        Photo = photo;
        StoreId = storeId;
    }

    public Employee(int id, string name, int positionId, float salary, string email, int storeId)
    {
        Id = id;
        Name = name;
        PositionId = positionId;
        Salary = salary;
        Email = email;
        StoreId = storeId;
    }
}
