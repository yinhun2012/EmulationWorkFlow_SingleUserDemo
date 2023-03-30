using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class HttpModule : Singleton<HttpModule>
{
    public delegate void HttpDoneCallback(object o);

    public class HttpStateObject
    {
        public string Url;
        public HttpWebRequest Request;
        public HttpDoneCallback Callback;
    }

    public const int HTTP_TIME_OUT = 5 * 1000;

    public string GetHttpSyncResponse<T>(string url, T t)
    {
        string data = JsonModule.Instance.Serialize(t);
        return GetHttpSyncResponse(url, data);
    }

    public string GetHttpSyncResponse(string url, string data)
    {
        try
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Timeout = HTTP_TIME_OUT;
            request.ContentType = "application/json;charset=utf-8";
            request.Method = "POST";
            request.UserAgent = "Windows";
            byte[] bts = GetUtf8Bytes(data);
            using (Stream s = request.GetRequestStream())
            {
                s.Write(bts, 0, bts.Length);
            }
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            string json = string.Empty;
            using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }
            return json;
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("HttpModule GetHttpSyncResponse ex = {0}", ex);
        }
        return string.Empty;
    }

    public void GetHttpAsyncResponse(string url, HttpDoneCallback callback)
    {
        try
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Timeout = HTTP_TIME_OUT;
            request.ContentType = "application/json;charset=utf-8";
            request.Method = "POST";
            request.UserAgent = "windows";
            HttpStateObject state = new HttpStateObject();
            state.Url = url;
            state.Request = request;
            state.Callback = callback;
            request.BeginGetResponse(GetHttpResponseCallback, callback);
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("HttpModule GetHttpAsyncResponse ex = {0}", ex);
        }
    }

    private void GetHttpResponseCallback(IAsyncResult iar)
    {
        HttpStateObject state = (HttpStateObject)iar.AsyncState;
        HttpWebRequest request = state.Request;
        HttpWebResponse resp = (HttpWebResponse)request.EndGetResponse(iar);
        string json = string.Empty;
        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
        {
            json = sr.ReadToEnd();
        }
        state.Callback?.Invoke(json);
    }

    private string GetUtf8String(byte[] bts)
    {
        return Encoding.UTF8.GetString(bts);
    }

    private byte[] GetUtf8Bytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }
}
