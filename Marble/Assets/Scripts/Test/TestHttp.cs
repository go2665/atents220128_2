using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

[Serializable]
class WebData
{
    public string name;
    public int str;
    public int dex;
    public int wis;
}

public class TestHttp : MonoBehaviour
{
    public TextMeshProUGUI charName;
    public TextMeshProUGUI str;
    public TextMeshProUGUI dex;
    public TextMeshProUGUI wis;
    public Button button;
    //readonly string url = "https://b91bf070-1e36-4535-999b-f4505efa9e2a.usrfiles.com/ugd/b91bf0_2c6d0a2c63e5482588c36e1fdffcbe4b.txt";
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
        UnityWebRequest www = UnityWebRequest.Get(url2);
        yield return www.SendWebRequest();

        if( www.result != UnityWebRequest.Result.Success )
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            WebData data = JsonUtility.FromJson<WebData>(www.downloadHandler.text);
            charName.text = data.name;
            str.text = data.str.ToString();
            dex.text = data.dex.ToString();
            wis.text = data.wis.ToString();
        }
    }
}
