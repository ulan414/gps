using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InternetAccess : MonoBehaviour
{
    private string works;
    public Text test;
    public InsertIntoDb InsertIntoDb;
    public float updateDelay = 3f;

    void Update()
    {
        if (updateDelay < 0)
        {
            updateDelay = 3f;
            CheckInternet();
            test.text = works;
        }
        updateDelay -= Time.deltaTime;
    }
    void CheckInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection");
            works = "No internet connection";
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                 Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            InsertIntoDb.FindItem();

            Debug.Log("Internet connected");
            works = "Internet connected";
            // You can send data here
        }
    }
}
