using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text displayNameText;
    [SerializeField] private Text interestIdText;
    [SerializeField] private Text languageText;
    [SerializeField] private Image imageData;

    public void ShowItemData(string name,string displayName,int interest,string langId,
        Sprite imageInfo)
    {
        nameText.text = name;
        displayNameText.text = displayName;
        interestIdText.text = interest.ToString();
        languageText.text = langId;
        imageData.sprite = imageInfo;
    }
}
