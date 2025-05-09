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
            cmd.CommandText = "SELECT * FROM Employees";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Employee(
                        reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2),
                        reader.GetFloat(3), reader.GetString(4), null, reader.GetInt32(6)
                    ));
                }
            }
        }
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
            cmd.CommandText = "SELECT * FROM Products";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Product(
                        reader.GetInt32(0), reader.GetString(1),
                        reader.GetInt32(2), reader.GetFloat(3), reader.GetInt32(4)
                    ));
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
            cmd.CommandText = "SELECT * FROM Customers";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Customer(
                        reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                        reader.GetString(3), reader.GetString(4)
                    ));
                }
            }
        }
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
            cmd.CommandText = "SELECT * FROM Orders";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new Order(
                        reader.GetInt32(0), reader.GetString(1),
                        reader.GetInt32(2), reader.GetFloat(3)
                    ));
                }
            }
        }
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
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.IsDBNull(2) ? -1 : reader.GetInt32(2)
                    ));
                }
            }
        }
        return list;
    }
}
