using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonModule : Singleton<JsonModule>
{
    public string FormatJsonString(string json)
    {
        JsonSerializer serializer = new JsonSerializer();
        TextReader tr = new StringReader(json);
        JsonTextReader jtr = new JsonTextReader(tr);
        object obj = serializer.Deserialize(jtr);
        if (obj != null)
        {
            StringWriter sw = new StringWriter();
            JsonTextWriter jtw = new JsonTextWriter(sw)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' ',
            };
            serializer.Serialize(jtw, obj);
            return sw.ToString();
        }
        return string.Empty;
    }

    public string Serialize<T>(T t)
    {
        try
        {
            if (t.Equals(null) || t.Equals("null"))
            {
#if UNITY_EDITOR
                Debug.LogErrorFormat("JsonModule Serialize T is Null");
#endif
                return string.Empty;
            }
            string json = JsonConvert.SerializeObject(t);
            return json;
        }
        catch (Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("JsonModule Serialize:{0} ex:{1}", t, ex);
#endif
        }
        return string.Empty;
    }

    public string Serialize<T>(List<T> t)
    {
        try
        {
            string json = JsonConvert.SerializeObject(t);
            return json;
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("JsonModule Serialize:{0} ex:{1}", t, ex);
        }
        return string.Empty;
    }

    public T Deserialize<T>(string json)
    {
        try
        {
            T t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("JsonModule Deserialize:{0} ex:{1}", json, ex);
        }
        return default(T);
    }

    public List<T> DeserializeList<T>(string json)
    {
        try
        {
            List<T> t = JsonConvert.DeserializeObject<List<T>>(json);
            return t;
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("JsonModule DeserializeList:{0} ex:{1}", json, ex);
        }
        return default(List<T>);
    }
}
