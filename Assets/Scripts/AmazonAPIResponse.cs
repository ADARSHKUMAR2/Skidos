using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class AmazonAPIResponse : SingletonBase<AmazonAPIResponse>
{
    public void CallAmazonApi<T>(string url,Action<T> callback)
    {
        StartCoroutine(ApiCaller(url,callback));
    }
    
    private IEnumerator ApiCaller<T>(string url, Action<T> callback)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError)
        {
            Debug.LogError(req.error);
        }
        else
        {
            var data = req.downloadHandler.text;
            var _finalData = JsonConvert.DeserializeObject<T>(data);
            if (callback != null)
                callback(_finalData);
           
            Debug.Log($"Success call");
        }
    }

    public void DownloadImageData(string url,Action<byte[]> callback = null)
    {
        StartCoroutine(DownloadImage(url, callback));
    }
    
    private IEnumerator DownloadImage(string url, Action<byte[]> callback = null)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                Debug.Log("Success");
                Texture myTexture = DownloadHandlerTexture.GetContent(uwr);
                var results = uwr.downloadHandler.data;
                if (callback != null)
                    callback(results);
                
            }
        }
    }
    
    
}
