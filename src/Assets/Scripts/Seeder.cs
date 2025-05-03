using UnityEngine;
using System;
using System.Data;

public class Seeder
{

    private static readonly string CREATE_EMPLOYEES_TABLE = "CREATE TABLE IF NOT EXISTS \"Employees\" (\n\t\"Id\" INTEGER NOT NULL UNIQUE PRIMARY KEY,\n\t\"Name\" TEXT NOT NULL CHECK (LENGTH(Name) <= 70),\n\t\"PositionId\" INTEGER REFERENCES Positions ON DELETE SET NULL ON UPDATE CASCADE,\n\t\"Salary\" REAL CHECK (Salary >= 0),\n\t\"Email\" TEXT CHECK (LENGTH(Email) <= 255),\n\t\"Photo\" BLOB,\n\t\"StoreId\" INTEGER NOT NULL REFERENCES Stores ON DELETE SET NULL ON UPDATE CASCADE\n);";

    private static readonly string CREATE_PRODUCTS_TABLE = "CREATE TABLE IF NOT EXISTS \"Products\" (\n\t\"Id\" INTEGER NOT NULL UNIQUE PRIMARY KEY,\n\t\"Name\" TEXT NOT NULL,\n\t\"CategoryId\" INTEGER REFERENCES Categories ON DELETE SET NULL ON UPDATE CASCADE,\n\t\"Price\" REAL NOT NULL CHECK (Price >= 0),\n\t\"Stock\" INTEGER NOT NULL CHECK (Stock >= 0)\n);";

    private static readonly string CREATE_CUSTOMERS_TABLE = "CREATE TABLE IF NOT EXISTS \"Customers\" (\n\t\"Id\" INTEGER NOT NULL UNIQUE PRIMARY KEY,\n\t\"Name\" TEXT CHECK (LENGTH(Name) <= 70),\n\t\"Email\" TEXT CHECK (LENGTH(Email) <= 254),\n\t\"PhoneNumber\" TEXT CHECK (LENGTH(PhoneNumber) <= 15),\n\t\"Address\" TEXT CHECK (LENGTH(Address) <= 200)\n);\n";

    private static readonly string CREATE_ORDERS_TABLE = "CREATE TABLE IF NOT EXISTS \"Orders\" (\n\t\"Id\" INTEGER NOT NULL UNIQUE PRIMARY KEY,\n\t\"Date\" DATE NOT NULL,\n\t\"CustomerId\" INTEGER REFERENCES Customers ON DELETE SET NULL ON UPDATE CASCADE,\n\t\"TotalAmount\" REAL CHECK(TotalAmount >= 0)\n);\n";

    private static readonly string CREATE_PRODUCT_ORDERS_TABLE = "CREATE TABLE IF NOT EXISTS \"ProductOrders\" (\n\t\"OrderId\" INTEGER NOT NULL REFERENCES Orders,\n\t\"ProductId\" INTEGER NOT NULL REFERENCES Products,\n\t\"Quantity\" INTEGER NOT NULL,\n    \"PriceAtOrder\" REAL NOT NULL,\n    PRIMARY KEY (\"OrderId\", \"ProductId\")\n);\n";

    private static readonly string CREATE_CATEGORIES_TABLE = "CREATE TABLE IF NOT EXISTS \"Categories\" (\n\t\"Id\" INTEGER NOT NULL UNIQUE PRIMARY KEY,\n\t\"Name\" TEXT NOT NULL UNIQUE CHECK(LENGTH(Name) <= 25)\n);\n";

    private static readonly string CREATE_STORES_TABLE = "CREATE TABLE IF NOT EXISTS \"Stores\" (\n\t\"Id\" INTEGER NOT NULL UNIQUE PRIMARY KEY,\n\t\"Address\" TEXT CHECK (LENGTH(Address) <= 200),\n\t\"ManagerId\" INTEGER REFERENCES Employees\n);";

    private static readonly string CREATE_POSITIONS_TABLE = "CREATE TABLE IF NOT EXISTS \"Positions\" (\n\t\"Id\" INTEGER NOT NULL UNIQUE PRIMARY KEY,\n\t\"Name\" TEXT NOT NULL UNIQUE CHECK(LENGTH(Name) <= 25)\n);";

    public static void CreateTables(IDbConnection connection)
    {
        Debug.Log("Creating tables...");
        ExecuteNonQuery(connection, CREATE_EMPLOYEES_TABLE);
        ExecuteNonQuery(connection, CREATE_PRODUCTS_TABLE);
        ExecuteNonQuery(connection, CREATE_CUSTOMERS_TABLE);
        ExecuteNonQuery(connection, CREATE_ORDERS_TABLE);
        ExecuteNonQuery(connection, CREATE_PRODUCT_ORDERS_TABLE);
        ExecuteNonQuery(connection, CREATE_CATEGORIES_TABLE);
        ExecuteNonQuery(connection, CREATE_STORES_TABLE);
        ExecuteNonQuery(connection, CREATE_POSITIONS_TABLE);
        Debug.Log("Finished creating tables...");

    }

    private static void ExecuteNonQuery(IDbConnection connection, string query)
    {
        try {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        } catch (Exception e) {
            Debug.LogError("Error executing query: " + e.Message + "\nQuery: " + query);
        }
    }

}