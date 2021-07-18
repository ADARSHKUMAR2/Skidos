using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler : SingletonBase<FileHandler>
{
    [SerializeField] private ItemDetails _itemPrefab;
    [SerializeField] private Transform _parentItem;
    [SerializeField] private GameObject _dataScreen;

    public void ShowData()
    {
        var fileName = "AmazonData.txt";
        var path = PathLoader.Instance.GetPath("AmazonData");
        var completePath = OfflineDataHandler.Instance._GetLocation(path+fileName);
        if (File.Exists(completePath))
        {
            OfflineDataHandler.Instance._ReadData<List<AmazonData>>(path+fileName,
                successResponse =>
                {
                    Debug.Log($"Read data");
                    DisplayData(successResponse);
                },
                failureResponse => { });
        }
        else
        {
            Debug.Log($"File not found - {completePath}");
        }
    }

    private void DisplayData(List<AmazonData> successResponse)
    {
        var path = PathLoader.Instance.GetPath("Image");
        Debug.Log($"Total items - {successResponse.Count}");
        for (int i = 0; i < successResponse.Count; i++)
        {
            ItemDetails newItem = Instantiate(_itemPrefab, _parentItem.transform.position, Quaternion.identity);
            newItem.transform.SetParent(_parentItem);
            var itemIndex = successResponse[i];
            
            var fileName = $"Image-{i}.txt";
            OfflineDataHandler.Instance._ReadData<string>(path + fileName, success =>
                {
                    newItem.ShowItemData(itemIndex.Name,itemIndex.DisplayName,itemIndex.InterestID,itemIndex.Language, _ImageConverter(success));
                },
                failure =>
                {

                });
           
        }
    }
    
    private Sprite _ImageConverter(string imgDesc)
    {
        Sprite image;
        byte[] imageBytes = Convert.FromBase64String(imgDesc.Substring(imgDesc.LastIndexOf(',') + 1));
        Texture2D tex = new Texture2D(600, 243);
        tex.LoadImage(imageBytes);
        image = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        return image;
    }

    public void EnableScreen()
    {
        _dataScreen.gameObject.SetActive(true);
    }

    public void DisableScreen()
    {
        // Use Object Pooling
        _dataScreen.gameObject.SetActive(false);
        foreach (Transform child in _parentItem) {
            Destroy(child.gameObject);
        }
    }
}
