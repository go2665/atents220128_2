using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TestHttp : MonoBehaviour
{
    public Button button;
    readonly string url = "https://b91bf070-1e36-4535-999b-f4505efa9e2a.usrfiles.com/ugd/b91bf0_2c6d0a2c63e5482588c36e1fdffcbe4b.txt";
    readonly string url2 = "https://b91bf070-1e36-4535-999b-f4505efa9e2a.usrfiles.com/ugd/b91bf0_f9ee4795f2a9473d8e7a1d40f9c9ece5.txt";

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        StartCoroutine(RequestHTTP());
    }

    IEnumerator RequestHTTP()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if( www.result != UnityWebRequest.Result.Success )
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
