using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using UnityEngine.UI;

public class SqliterDB : MonoBehaviour
{
    private string dbName;
    private IDbConnection dbConnection;
    public Text Test;

    void Start()
    {
        dbName = Application.persistentDataPath + "/Inventory.db";
        CreateDB();

        AddWeapon("Silver Sword", 30);
        DisplayWeapons();
    }

    void CreateDB()
    {
        dbConnection = (IDbConnection)new AndroidJavaObject("com.example.MySQLiteDatabase");
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS weapons (name VARCHAR(20), damage INT);";
        dbCommand.ExecuteNonQuery();

        dbCommand.Dispose();
        dbConnection.Close();
    }

    void AddWeapon(string weaponName, int weaponDamage)
    {
        dbConnection = (IDbConnection)new AndroidJavaObject("com.example.MySQLiteDatabase");
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "INSERT INTO weapons (name, damage) VALUES ('" + weaponName + "', '" + weaponDamage + "');";
        dbCommand.ExecuteNonQuery();

        dbCommand.Dispose();
        dbConnection.Close();
    }

    void DisplayWeapons()
    {
        dbConnection = (IDbConnection)new AndroidJavaObject("com.example.MySQLiteDatabase");
        dbConnection.Open();

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT * FROM weapons;";

        IDataReader reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log("Name: " + reader["name"] + "\tDamage: " + reader["damage"]);
            Test.text = "Name: " + reader["name"] + "\tDamage: " + reader["damage"];
        }
        reader.Close();

        dbCommand.Dispose();
        dbConnection.Close();
    }
}
