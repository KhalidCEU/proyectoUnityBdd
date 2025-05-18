using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private static string dbName = "store.db";
    private static string dbPath;

    void Awake()
    {
        dbPath = Path.Combine(Application.persistentDataPath, dbName);
        if (!File.Exists(dbPath))
        {
            TextAsset dbAsset = Resources.Load<TextAsset>("store");
            File.WriteAllBytes(dbPath, dbAsset.bytes);
        }
    }

    private static IDbConnection GetConnection()
    {
        string conn = "URI=file:" + dbPath;
        return new SqliteConnection(conn);
    }

    private static void AddParameter(IDbCommand cmd, string name, object value)
    {
        IDbDataParameter param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }

    // ======================
    // EMPLOYEES
    // ======================

    public static List<Employee> GetAllEmployees()
{
    List<Employee> list = new List<Employee>();

    using (IDbConnection db = GetConnection())
    {
        db.Open();
        var cmd = db.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, PositionId, Salary, Email, StoreId FROM Employees";

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                try
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    int positionId = reader.GetInt32(2);
                    float salary = reader.GetFloat(3);
                    string email = reader.GetString(4);
                    int storeId = reader.GetInt32(5);

                    var emp = new Employee(id, name, positionId, salary, email, null, storeId);
                    Debug.Log("[DB] Empleado leído: " + emp);
                    list.Add(emp);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(" Error leyendo un empleado: " + ex.Message);
                }
            }
        }
    }

    Debug.Log(" Total empleados leídos: " + list.Count);
    return list;
}



    public static void AddEmployee(Employee e)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"INSERT INTO Employees (Name, PositionId, Salary, Email, StoreId)
                                VALUES (@Name, @PositionId, @Salary, @Email, @StoreId)";
            AddParameter(cmd, "@Name", e.Name);
            AddParameter(cmd, "@PositionId", e.PositionId);
            AddParameter(cmd, "@Salary", e.Salary);
            AddParameter(cmd, "@Email", e.Email);
            AddParameter(cmd, "@StoreId", e.StoreId);
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteEmployee(int id)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = "DELETE FROM Employees WHERE Id = @Id";
            AddParameter(cmd, "@Id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static void UpdateEmployee(Employee e)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"UPDATE Employees SET Name = @Name, PositionId = @PositionId,
                                Salary = @Salary, Email = @Email, StoreId = @StoreId WHERE Id = @Id";
            AddParameter(cmd, "@Name", e.Name);
            AddParameter(cmd, "@PositionId", e.PositionId);
            AddParameter(cmd, "@Salary", e.Salary);
            AddParameter(cmd, "@Email", e.Email);
            AddParameter(cmd, "@StoreId", e.StoreId);
            AddParameter(cmd, "@Id", e.Id);
            cmd.ExecuteNonQuery();
        }
    }

    // ======================
    // PRODUCTS
    // ======================
    public static List<Product> GetAllProducts()
{
    List<Product> list = new List<Product>();

    using (var db = GetConnection())
    {
        db.Open();
        var cmd = db.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, CategoryId, Price, Stock FROM Products";

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                try
                {
                    var product = new Product(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetFloat(3),
                        reader.GetInt32(4)
                    );
                    list.Add(product);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error reading product: " + ex.Message);
                }
            }
        }
    }

    return list;
}

    public static void AddProduct(Product p)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"INSERT INTO Products (Name, CategoryId, Price, Stock)
                                VALUES (@Name, @CategoryId, @Price, @Stock)";
            AddParameter(cmd, "@Name", p.Name);
            AddParameter(cmd, "@CategoryId", p.CategoryId);
            AddParameter(cmd, "@Price", p.Price);
            AddParameter(cmd, "@Stock", p.Stock);
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteProduct(int id)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = "DELETE FROM Products WHERE Id = @Id";
            AddParameter(cmd, "@Id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static void UpdateProduct(Product p)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"UPDATE Products SET Name = @Name, CategoryId = @CategoryId,
                                Price = @Price, Stock = @Stock WHERE Id = @Id";
            AddParameter(cmd, "@Name", p.Name);
            AddParameter(cmd, "@CategoryId", p.CategoryId);
            AddParameter(cmd, "@Price", p.Price);
            AddParameter(cmd, "@Stock", p.Stock);
            AddParameter(cmd, "@Id", p.Id);
            cmd.ExecuteNonQuery();
        }
    }

    // ======================
    // CUSTOMERS
    // ======================
   public static List<Customer> GetAllCustomers()
{
    List<Customer> list = new List<Customer>();

    using (var db = GetConnection())
    {
        db.Open();
        var cmd = db.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Email, PhoneNumber, Address FROM Customers";

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                try
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string email = reader.GetString(2);
                    string phone = reader.GetString(3);
                    string address = reader.GetString(4);

                    var customer = new Customer(id, name, email, phone, address);
                    Debug.Log("Cliente leído: " + customer);
                    list.Add(customer);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(" Error al leer un cliente: " + ex.Message);
                }
            }
        }
    }

    Debug.Log("Total clientes leídos: " + list.Count);
    return list;
}


    public static void AddCustomer(Customer c)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"INSERT INTO Customers (Name, Email, PhoneNumber, Address)
                                VALUES (@Name, @Email, @PhoneNumber, @Address)";
            AddParameter(cmd, "@Name", c.Name);
            AddParameter(cmd, "@Email", c.Email);
            AddParameter(cmd, "@PhoneNumber", c.PhoneNumber);
            AddParameter(cmd, "@Address", c.Address);
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteCustomer(int id)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = "DELETE FROM Customers WHERE Id = @Id";
            AddParameter(cmd, "@Id", id);
            cmd.ExecuteNonQuery();
        }
    }

     public static void UpdateCustomer(Customer c)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"UPDATE Customers SET Name = @Name, Email = @Email,
                                PhoneNumber = @PhoneNumber, Address = @Address WHERE Id = @Id";
            AddParameter(cmd, "@Name", c.Name);
            AddParameter(cmd, "@Email", c.Email);
            AddParameter(cmd, "@PhoneNumber", c.PhoneNumber);
            AddParameter(cmd, "@Address", c.Address);
            AddParameter(cmd, "@Id", c.Id);
            cmd.ExecuteNonQuery();
        }
    }

    // ======================
    // ORDERS
    // ======================
    public static List<Order> GetAllOrders()
{
    List<Order> list = new List<Order>();

    using (var db = GetConnection())
    {
        db.Open();
        var cmd = db.CreateCommand();
        cmd.CommandText = "SELECT Id, Date, CustomerId, TotalAmount FROM Orders";

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                try
                {
                    int id = reader.GetInt32(0);
                    string date =  reader[1].ToString();
                    int customerId = reader.GetInt32(2);
                    float totalAmount = reader.GetFloat(3);

                    var order = new Order(id, date, customerId, totalAmount);
                    Debug.Log("s Orden leída: " + order);
                    list.Add(order);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error al leer una orden: " + ex.Message);
                }
            }
        }
    }

    Debug.Log("Total órdenes leídas: " + list.Count);
    return list;
}


    public static void AddOrder(Order o)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"INSERT INTO Orders (Date, CustomerId, TotalAmount)
                                VALUES (@Date, @CustomerId, @TotalAmount)";
            AddParameter(cmd, "@Date", o.Date);
            AddParameter(cmd, "@CustomerId", o.CustomerId);
            AddParameter(cmd, "@TotalAmount", o.TotalAmount);
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteOrder(int id)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = "DELETE FROM Orders WHERE Id = @Id";
            AddParameter(cmd, "@Id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static void UpdateOrder(Order o)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"UPDATE Orders SET Date = @Date, CustomerId = @CustomerId,
                                TotalAmount = @TotalAmount, WHERE Id = @Id";
            AddParameter(cmd, "@date", o.Date);
            AddParameter(cmd, "@CustomerId", o.CustomerId);
            AddParameter(cmd, "@TotalAmount", o.TotalAmount);
            AddParameter(cmd, "@Id", o.Id);
            cmd.ExecuteNonQuery();
        }
    }

    // ======================
    // STORES
    // ======================
    public static List<Store> GetAllStores()
{
    List<Store> list = new List<Store>();
    using (var db = GetConnection())
    {
        db.Open();
        var cmd = db.CreateCommand();
        cmd.CommandText = "SELECT * FROM Stores";

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                list.Add(new Store(
                    reader.GetInt32(0),                         // Id
                    reader.GetString(1),                        // Address
                    reader.IsDBNull(2) ? -1 : reader.GetInt32(2) // ManagerId (puede ser NULL)
                ));
            }
        }
    }
    return list;
}


    public static void AddStore(Store s)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"INSERT INTO Orders (Address, ManagerId)
                                VALUES (@Address, @ManagerId)";
            AddParameter(cmd, "@Address", s.Address);
            AddParameter(cmd, "@CustomerId", s.ManagerId);
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteStore(int id)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = "DELETE FROM Stores WHERE Id = @Id";
            AddParameter(cmd, "@Id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static void UpdateStore(Store s)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"UPDATE Stores SET Address = @Address, ManagerId = @ManagerId";
            AddParameter(cmd, "@Address", s.Address);
            AddParameter(cmd, "@ManagerId", s.ManagerId);
            AddParameter(cmd, "@Id", s.Id);
            cmd.ExecuteNonQuery();
        }
    }
}
