public class Store //store entidades

{
    public int Id;
    public string Address;
    public int ManagerId;

    public Store(int id, string address, int managerId)
    {
        Id = id;
        Address = address;
        ManagerId = managerId;
    }

    public Store(string address, int managerId)
    {
        Address = address;
        ManagerId = managerId;
    }
}

