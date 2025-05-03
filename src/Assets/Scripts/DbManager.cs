using UnityEngine;
using System;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;

public class DBManager : MonoBehaviour
{
    private string dbName = "AdminApp.sqlite";

    void Start()
	{
        string dbPath = Path.Combine(Application.persistentDataPath, dbName);
        Debug.Log("DB path: " + dbPath);
        string connString = "URI=file:" + dbPath;

        var connection = new SqliteConnection(connString);

        connection.Open();

        Seeder.CreateTables(connection);
    }

    // Update is called once per frame
    void Update()
	{
	}

}

