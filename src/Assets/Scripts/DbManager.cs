using UnityEngine;
using System;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;

public class DBManager : MonoBehaviour
{
    private string dbName = "AdminApp.sqlite";
    private static IDbConnection connection;

    void Start()
	{
        string dbPath = Path.Combine(Application.persistentDataPath, dbName);
        Debug.Log("DB path: " + dbPath);

        string connString = "URI=file:" + dbPath;

        connection = new SqliteConnection(connString);

        connection.Open();

        Seeder.CreateTables(connection);

        if (IsTableEmpty(connection, "Employees"))
        {
            Seeder.InsertIntoTables(connection);
        } else
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

    // Called once per frame
    void Update()
	{
	}

    public static IDbConnection GetDbConnection()
    {
        return connection;
    }

}

