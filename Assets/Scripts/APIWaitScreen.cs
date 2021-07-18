using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIWaitScreen : SingletonBase<APIWaitScreen>
{
    [SerializeField] private GameObject _objToRotate;
    [SerializeField] private GameObject _blocker;
    
    private void Update()
    {
        _objToRotate.transform.Rotate(Vector3.forward) ;
    }

    public void EnableLoadingIcon()
    {
        _blocker.SetActive(true);
        _objToRotate.SetActive(true);
    }
    public void DisableLoadingIcon()
    {
        _blocker.SetActive(false);
        _objToRotate.SetActive(false);
    }
}
