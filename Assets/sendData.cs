using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RestSharp;
using System;
using UnityEngine.UI;
public class sendData : MonoBehaviour
{
    private int user_id = 0;
    private long _timestamp2 = 0;
    private int _latitude2 = 0;
    private int _longitude2 = 0;
    public Text test;

    public void SendTheData(float _latitude, float _longitude, double _timestamp)
    {
        _timestamp2 = (long)_timestamp;
        if (!PlayerPrefs.HasKey("user_id"))
        {
            // Generate a new random value and save it to PlayerPrefs
            user_id = (int)DateTime.Now.Ticks;
            PlayerPrefs.SetInt("user_id", user_id);
        }
        else
        {
            // Retrieve the random value from PlayerPrefs
            user_id = PlayerPrefs.GetInt("user_id");
        }
        _latitude2 = (int)Mathf.Round(_latitude * Mathf.Pow(10, 6));
        _longitude2 = (int)Mathf.Round(_longitude * Mathf.Pow(10, 6));
        //_latitude = _longitude*100f;
        //_longitude = _latitude*10000f;
        _timestamp2 = _timestamp2*1000;
        //_timestamp2 = _timestamp2 + 1682197200000;
        var client = new RestClient("https://location-f71a.restdb.io/rest/userlocation");
        var request = new RestRequest(Method.POST);
        request.AddHeader("cache-control", "no-cache");
        request.AddHeader("x-apikey", "06e84463a12387d0fa46863b2eb909a7f0cde");
        request.AddHeader("content-type", "application/json");
        request.AddParameter("application/json", "{\"id\":0,\"user\":" + user_id + ",\"latitude\":" + _latitude2 + ",\"longitude\":" + _longitude2 + ",\"timestamp\":" + _timestamp2 + "}", ParameterType.RequestBody);
        IRestResponse response = client.Execute(request);
        Debug.Log("data sent");
        test.text = "data sent to server";
    }
}
