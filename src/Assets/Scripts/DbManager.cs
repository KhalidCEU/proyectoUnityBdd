using UnityEngine;
using System;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class DbManager : MonoBehaviour
{
    private string dbName = "AdminApp.sqlite";
    private string connString;
    private IDbConnection connection;

    void Awake()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, dbName);
        Debug.Log("DB path: " + dbPath);

        connString = "URI=file:" + dbPath;

        connection = new SqliteConnection(connString);
        connection.Open();

        Seeder.CreateTables(connection);

        if (IsTableEmpty(connection, "Employees"))
        {
            Seeder.InsertIntoTables(connection);
        }
        else
        {
            Debug.Log("DB is already seeded !");
        }
    }

    private bool IsTableEmpty(IDbConnection connection, string tableName)
    {
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = $"SELECT COUNT(*) FROM {tableName}";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0;
        }
    }

    private IDbConnection GetConnection()
    {
        return new SqliteConnection(connString);
    }

    private void AddParameter(IDbCommand cmd, string name, object value)
    {
        IDbDataParameter param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }

    // ================= EMPLOYEES =================
    public List<Employee> GetAllEmployees()
    {
        List<Employee> list = new List<Employee>();
        using (var db = GetConnection())
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

    public void AddEmployee(Employee e)
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

    public void DeleteEmployee(int id)
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

    public void UpdateEmployee(Employee e)
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

    // ================= PRODUCTS =================
    public List<Product> GetAllProducts()
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

    public void AddProduct(Product p)
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

    public void DeleteProduct(int id)
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

    public void UpdateProduct(Product p)
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

    // ================= CUSTOMERS =================
    public List<Customer> GetAllCustomers()
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

    public void AddCustomer(Customer c)
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

    public void DeleteCustomer(int id)
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

    public void UpdateCustomer(Customer c)
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

    // ================= ORDERS =================
    public List<Order> GetAllOrders()
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
                        reader.GetInt32(0), reader[1].ToString(),
                        reader.GetInt32(2), reader.GetFloat(3)
                    ));
                }
            }
        }
        return list;
    }

    public void AddOrder(Order o)
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

    public void DeleteOrder(int id)
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

    public void UpdateOrder(Order o)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"UPDATE Orders SET Date = @Date, CustomerId = @CustomerId,
                                TotalAmount = @TotalAmount WHERE Id = @Id";
            AddParameter(cmd, "@Date", o.Date);
            AddParameter(cmd, "@CustomerId", o.CustomerId);
            AddParameter(cmd, "@TotalAmount", o.TotalAmount);
            AddParameter(cmd, "@Id", o.Id);
            cmd.ExecuteNonQuery();
        }
    }

    // ================= STORES =================
    public List<Store> GetAllStores()
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
                        reader.GetInt32(0), reader.GetString(1),
                        reader.IsDBNull(2) ? -1 : reader.GetInt32(2)
                    ));
                }
            }
        }
        return list;
    }

    public void AddStore(Store s)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"INSERT INTO Stores (Address, ManagerId)
                                VALUES (@Address, @ManagerId)";
            AddParameter(cmd, "@Address", s.Address);
            AddParameter(cmd, "@ManagerId", s.ManagerId);
            cmd.ExecuteNonQuery();
        }
    }

    public void DeleteStore(int id)
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

    public void UpdateStore(Store s)
    {
        using (var db = GetConnection())
        {
            db.Open();
            var cmd = db.CreateCommand();
            cmd.CommandText = @"UPDATE Stores SET Address = @Address, ManagerId = @ManagerId WHERE Id = @Id";
            AddParameter(cmd, "@Address", s.Address);
            AddParameter(cmd, "@ManagerId", s.ManagerId);
            AddParameter(cmd, "@Id", s.Id);
            cmd.ExecuteNonQuery();
        }
    }
}
