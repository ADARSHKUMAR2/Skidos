using System;
using System.Collections.Generic;
using UnityEngine;

public class PathLoader : SingletonBase<PathLoader>
{
    [SerializeField] private PathMapper pathMapper;
    
    private Dictionary<string, string> _masterMapper;
    
    private void Awake()
    {
        _masterMapper = new Dictionary<string, string>();
        InitializeResources();
    }

    private void InitializeResources()
    {
        foreach (var item in pathMapper.GetResource().allLocations)
        {
            if (!_masterMapper.ContainsKey(item.apiName))
            {
                _masterMapper.Add(item.apiName, item.storageLocation);
            }
        }
    }

    public void AddResource(string apiName, string folderLoc,string fileName,DirectoryType directoryType)
    {
        if (directoryType == DirectoryType.Folder)
            fileName = "";
        
        if (!_masterMapper.ContainsKey(apiName))
        {
            _masterMapper.Add(apiName, folderLoc);
            pathMapper.CreateNewLocation(apiName, folderLoc+"/"+fileName);
            
        }

    }
    
    public string GetPath(string key)
    {
        string path;
        if (_masterMapper.TryGetValue(key,out path))
        {
            return path;
        }

        return null;
    }

}