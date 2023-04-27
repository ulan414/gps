using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class ConvertDataBaseToMultiPlatform : MonoBehaviour
{
    public string DataBaseName;

    public void Awake()
    {
        GenerateConnectionString(DataBaseName + ".db");
    }

    public void GenerateConnectionString(string DatabaseName)
    {
#if UNITY_EDITOR
        string dbPath = Application.dataPath + "/StreamingAssets/" + DatabaseName;
#else
        // Check if file exists in Application.persistentDataPath
        string filepath = Application.persistentDataPath + "/" + DatabaseName;

        if (!File.Exists(filepath) || new System.IO.FileInfo(filepath).Length == 0)
        {
            // If it doesn't exist, load it from StreamingAssets directory
            string loadDbPath;
            if (Application.platform == RuntimePlatform.Android)
            {
                loadDbPath = "jar:file://" + Application.dataPath + "!/assets/" + DatabaseName; // This is the path to your StreamingAssets in Android
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                loadDbPath = Application.dataPath + "/Raw/" + DatabaseName; // This is the path to your StreamingAssets in iOS
            }
            else
            {
                loadDbPath = Application.dataPath + "/StreamingAssets/" + DatabaseName; // This is the path to your StreamingAssets in other platforms
            }

            // Load the db using UnityWebRequest
            UnityWebRequest request = UnityWebRequest.Get(loadDbPath);
            request.SendWebRequest();

            // Wait until the request is done
            while (!request.isDone) { }

            // Check if there's any error
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError(request.error);
                return;
            }

            // Save to Application.persistentDataPath
            File.WriteAllBytes(filepath, request.downloadHandler.data);
        }

        var dbPath = filepath;
#endif
    }
}
