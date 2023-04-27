using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using Mono.Data;
using System.Data;
using System.IO;

public class InsertIntoDb : MonoBehaviour
{
    public string DataBaseName;
    public sendData sendData;

    public void InsertInto(float _latitude, float _longitude, double _timestamp, int _status)
    {
        int _timestampint = Mathf.RoundToInt((float)_timestamp);
        string conn = SetDataBaseClass.SetDataBase(DataBaseName+".db");
        IDbConnection dbcon;
        IDbCommand dbcmd;
        IDataReader reader;

        dbcon = new SqliteConnection(conn);
        dbcon.Open();
        dbcmd = dbcon.CreateCommand();
        /*        string SQLQuery = "Insert Into Users(Name,Family,PhoneNumber,Email) " +
                                  "Values('" + _NameInput + "','" + _FamilyInput + "', '" + _PhoneInput + "', '" + _EmailInput + "')";*/
        string SQLQuery = "Insert Into Location(Latitude,Longitude,Timestamp,Status) " +
                          "Values('" + _latitude + "','" + _longitude + "', '" + _timestampint + "', '" + _status + "')";
        dbcmd.CommandText = SQLQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null; 
    }

    public InputField IDInput;
    public List<string[]> records = new List<string[]>();

    public void FindItem()
    {
        records.Clear();
        string conn = SetDataBaseClass.SetDataBase(DataBaseName + ".db");
        IDbConnection dbcon;
        IDbCommand dbcmd;
        IDataReader reader;

        dbcon = new SqliteConnection(conn);
        dbcon.Open();
        dbcmd = dbcon.CreateCommand();
        string SQLQuery = "Select Id,Latitude,Longitude,Timestamp,Status FROM Location Where Status='" + 0 + "'";
        dbcmd.CommandText = SQLQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string[] record = new string[5];
            record[0] = reader.GetInt32(0).ToString();//Id
            record[1] = reader.GetString(1); // Latitude
            record[2] = reader.GetString(2); // Longitude
            record[3] = reader.GetInt32(3).ToString(); // Timestamp
            record[4] = reader.GetInt32(4).ToString(); // Status

            records.Add(record);
        }
        // Access the records array to use the data as needed.
        foreach (string[] record in records)
        {
                Debug.Log("sadajsndkbacbzxjbczx: " + record[4]);
                Debug.Log("sadajsndkbacbzxjbczx: " + record[0]);
                sendData.SendTheData(float.Parse(record[1]), float.Parse(record[2]), double.Parse(record[3]));
                UpdateData(int.Parse(record[0]), float.Parse(record[1]), float.Parse(record[2]), double.Parse(record[3]), 1);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }

    public void UpdateData(int id, float _latitude, float _longitude, double _timestamp, int _status)
    {
        int _timestampint = Mathf.RoundToInt((float)_timestamp);
        string conn = SetDataBaseClass.SetDataBase(DataBaseName + ".db");
        IDbConnection dbcon;
        IDbCommand dbcmd;
        IDataReader reader;

        dbcon = new SqliteConnection(conn);
        dbcon.Open();
        dbcmd = dbcon.CreateCommand();

        string SQLQuery = "UPDATE Location SET Latitude='" + _latitude + "', Longitude='" + _longitude + "', Timestamp='" + _timestampint + "', Status='" + _status + "' WHERE Id='" + id + "'";
        dbcmd.CommandText = SQLQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

        }
        Debug.Log("Updated");
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbcon.Close();
        dbcon = null;
    }
}
