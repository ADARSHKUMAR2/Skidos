using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace UIFUnctionality
{
    public class GameHandler :MonoBehaviour
    {
        [SerializeField] private Button _fetchButton;
        [SerializeField] private Button _displayDataButton;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _buttonsPanel;
        
        private int index = 0;
        private string fileName = "AmazonData";
        private string linkUrl = "https://testinterest.s3.amazonaws.com/interest.json";
        private void OnEnable()
        {
            AddButtonListeners();
        }

        private void Start()
        {
            CheckIfDataExists();
        }

        private void CheckIfDataExists()
        {
            
            var fileName = "AmazonData.txt";
            var path = PathLoader.Instance.GetPath("AmazonData");
            var completePath = OfflineDataHandler.Instance._GetLocation(path+fileName);
            if (File.Exists(completePath))
            {
                Debug.Log($"Main file found");
                _displayDataButton.interactable = true;
            }
            else
            {
                Debug.Log($"Main file not found - {completePath}");
                _displayDataButton.interactable = false;
            }
            
        }

        private void AddButtonListeners()
        {
            _fetchButton.onClick.AddListener(CallAPI);
            _deleteButton.onClick.AddListener(DeleteFiles);
            _displayDataButton.onClick.AddListener(DisplayData);
            _backButton.onClick.AddListener(ShowButtonsPanel);
        }

        private void CallAPI()
        {
            APIWaitScreen.Instance.EnableLoadingIcon();
            AmazonAPIResponse.Instance.CallAmazonApi<List<AmazonData>>(linkUrl,AddDataToFile);
        }

        private void AddDataToFile(List<AmazonData> obj)
        {
            OfflineDataHandler.Instance._SaveProjectsToFile(obj,fileName,fileName,DirectoryType.SingleFile);
            index = 0;
            DownloadImagesData();
        }

        private void DownloadImagesData()
        {
            var fileName = "AmazonData.txt";
            var path = PathLoader.Instance.GetPath("AmazonData");
            var completePath = OfflineDataHandler.Instance._GetLocation(path+fileName);
            if (File.Exists(completePath))
            {
                OfflineDataHandler.Instance._ReadData<List<AmazonData>>(path+fileName,
                    successResponse =>
                    {
                        if (index < successResponse.Count)
                        {
                            Debug.Log($"Read data");
                            var url = successResponse[index].PictureUrl;
                            AmazonAPIResponse.Instance.DownloadImageData(url,AddImagesData);  
                        }
                        else
                        {
                            Debug.Log($"All images saved");
                            APIWaitScreen.Instance.DisableLoadingIcon();
                            _displayDataButton.interactable = true;
                            index = 0;
                        }
                    },
                    failureResponse => { });
            }
            
        }

        private void AddImagesData(byte[] imageData)
        {
            OfflineDataHandler.Instance._SaveProjectsToFile(imageData,"Image",$"Image-{index}",DirectoryType.Folder);
            index++;
            DownloadImagesData();
        }

        private void DeleteFiles()
        {
            var path = PathLoader.Instance.GetPath(fileName);
            OfflineDataHandler.Instance._DeleteFile(path);
        }
        
        private void DisplayData()
        {
            _buttonsPanel.SetActive(false);
            FileHandler.Instance.EnableScreen();
            FileHandler.Instance.ShowData();
        }

        private void ShowButtonsPanel()
        {
            _buttonsPanel.SetActive(true);
            FileHandler.Instance.DisableScreen();
        }

        private void OnDisable()
        {
            RemoveButtonListerens();
        }

        private void RemoveButtonListerens()
        {
            _fetchButton.onClick.RemoveListener(CallAPI);
            _deleteButton.onClick.RemoveListener(DeleteFiles);
            _displayDataButton.onClick.RemoveListener(DisplayData);
            _backButton.onClick.RemoveListener(ShowButtonsPanel);
        }
    }
}