using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDemo : MonoBehaviour
{
    public Button buttonLogin;

    void Start()
    {
        buttonLogin.onClick.AddListener(ButtonLoginClick);
    }

    private void ButtonLoginClick()
    {
        string url = "http://127.0.0.1:8249/Login/";
        string json = HttpModule.Instance.GetHttpSyncResponse(url, new HttpLoginData
        {
            Username = "yangyang",
            Password = "123"
        });
        Debug.LogFormat("json = {0}", json);
    }
}
