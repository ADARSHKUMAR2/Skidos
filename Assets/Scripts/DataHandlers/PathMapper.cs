using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class Locations
{
    [Header("All API's")]
    [SerializeField]
    public List<Location> allLocations;
}

[Serializable]
public class Location
{
    [SerializeField]
    public string apiName;
    [SerializeField]
    public string storageLocation;
}
    
[CreateAssetMenu(fileName = "LocationMapper", menuName = "ScriptableObjects/LocationMapper", order = 1)]
public class PathMapper : ScriptableObject
{
    [SerializeField] private Locations location;

    public Locations GetResource()
    {
        return location;
    }

    public void CreateNewLocation(string name,string path)
    {
        
        Debug.Log($"<color=white>{name} - {path}</color>");
        Location loc = new Location
        {
            apiName = name,
            storageLocation = path
        };
        
        location.allLocations.Add(loc);
        
        // BinaryFormatter bf = new BinaryFormatter();
        // var obj = ScriptableObject.CreateInstance(loc.apiName);
        // bf.Serialize(obj,loc);
        
    }
}