using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;

public class gps : MonoBehaviour
{
    public Text gpsOut;
    public Text gpsLong;
    public Text gpsAlt;
    public Text gpsTime;
    public InsertIntoDb InsertIntoDb;
    public sendData sendData;
    public float saveToDbDelay = 300f;
    public bool isUpdating;
    public Text test;
    private void Start()
    {
        saveToDbDelay = -1f;
    }
    private void Update()
    {
        if (!isUpdating)
        {
            StartCoroutine(GetLocation());
            isUpdating = !isUpdating;
        }
        if(saveToDbDelay > 0)
        {
            saveToDbDelay -= Time.deltaTime;
        }
    }
    IEnumerator GetLocation()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield return new WaitForSeconds(10);

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            gpsOut.text = "Timed out";
            gpsLong.text = "Timed out";
            gpsAlt.text = "Timed out";
            gpsTime.text = "Timed out";
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            gpsOut.text = "Unable to determine device location";
            gpsLong.text = "Unable to determine device location";
            gpsAlt.text = "Unable to determine device location";
            gpsTime.text = "Unable to determine device location";
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            double timestamp = Input.location.lastData.timestamp;
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToLocalTime();
            string dateTimeString = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            Debug.Log(dateTimeString);
            gpsOut.text = "Latitude: " + Input.location.lastData.latitude;
            gpsLong.text = "Longitude: " + Input.location.lastData.longitude;
            gpsAlt.text = "Altitude: " + Input.location.lastData.altitude + 100f;
            gpsTime.text = "Time: " + dateTimeString;
            // Access granted and location value could be retrieved
            if (saveToDbDelay < 0)
            {
                saveToDbDelay = 300f;
                InsertIntoDb.InsertInto(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.timestamp, 0);
                //InsertIntoDb.FindItem();
                //sendData.SendTheData(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.timestamp);
                test.text = "location saved in local database";
            }
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        isUpdating = !isUpdating;
        Input.location.Stop();
    }
}