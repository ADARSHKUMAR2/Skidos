using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AmazonData
{
    public string Name { get; set; }

    public string PictureUrl { get; set; }

    public string DisplayName { get; set; }

    public string Language { get; set; }

    public int InterestID { get; set; }
}

[Serializable]
public class Data
{

}

public enum DirectoryType { Folder, SingleFile }