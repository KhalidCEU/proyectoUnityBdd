using UnityEngine;
using System;
using System.Data;

public class Seeder
{

    public static void CreateTables(IDbConnection connection)
    {
        TextAsset sqlAsset = Resources.Load<TextAsset>("sql/create");

        if (sqlAsset == null)
        {
            Debug.LogError("Could not find 'create' file in Resources/sql !");
            return;
        }

        string sqlFileContent = sqlAsset.text;

        Debug.Log("Creating tables (if they don't exist)...");
        ExecuteNonQuery(connection, sqlFileContent);
        Debug.Log("Finished checking/creating tables...");
    }

    public static void InsertIntoTables(IDbConnection connection)
    {
        TextAsset sqlAsset = Resources.Load<TextAsset>("sql/insert");

        if (sqlAsset == null)
        {
            Debug.LogError("Could not find 'insert' file in Resources/sql !");
            return;
        }

        string sqlFileContent = sqlAsset.text;

        Debug.Log("Inserting into tables...");
        ExecuteNonQuery(connection, sqlFileContent);
        Debug.Log("Finished inserting into tables...");
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