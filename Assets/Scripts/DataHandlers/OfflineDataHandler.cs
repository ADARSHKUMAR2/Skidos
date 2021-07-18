using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class OfflineDataHandler : SingletonBase<OfflineDataHandler>
{
    private string location;
    private string path;
    private string extension = ".txt";
    private readonly char separator = Path.DirectorySeparatorChar;

    private void SetDefaultLocation()
    {
        location = Path.Combine(Application.persistentDataPath, $"Resources");
    }
    public void _SaveProjectsToFile<T>(T data, string apiName, string fileName,DirectoryType directoryType)
        where T : class
    {
        SetDefaultLocation();
        path = Path.Combine(location, apiName);
        if (!Directory.Exists(path))
        {
            Debug.Log($"{path}");
            Directory.CreateDirectory(path);
        }

        var stringData = "";
        if (typeof(T) == typeof(string))
            stringData = data.ToString();
        else
            stringData = JsonConvert.SerializeObject(data);

        //Create Files and Add Data
        path = Path.Combine(path, fileName + ".txt");
        File.WriteAllText(path, stringData);
        PathLoader.Instance.AddResource(apiName, apiName, fileName + extension,directoryType);
    }
    public string _GetLocation(string loc)
    {
        SetDefaultLocation();
        return location + separator + loc;
    }
    public void _ReadData<T>(string loc,Action<T> success, Action<string> failure)
    {
        SetDefaultLocation();
        string finalPath = location + separator + loc;
        Debug.Log($"<color=white> {finalPath} </color>");

        using (StreamReader stream = new StreamReader(finalPath))
        {
            string dataRead = stream.ReadToEnd();
            // var result = JsonUtility.FromJson<T>(dataRead);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(dataRead);
            stream.Close();
            if (result != null)
                success.Invoke(result);
            else
            {
                Debug.Log($"Failed");
                failure.Invoke("Fail");
            }

        }
    }
    
    public void _DeleteFile(string loc)
    {
        SetDefaultLocation();
        string finalPath = location + separator + loc;
        if (File.Exists(finalPath))
            File.Delete(finalPath);
    }
}
