using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeepLinkManager : MonoBehaviour
{
  private static DeepLinkManager instance;
  public string serverUrl;
  // public float FOV;
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      Application.deepLinkActivated += OnDeepLinkActivated;
      if (!string.IsNullOrEmpty(Application.absoluteURL))
      {
        OnDeepLinkActivated(Application.absoluteURL);
      }
      else
      {
        serverUrl = "ws://10.10.10.52:7799/device1";
        // FOV = 80;
      }
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public static DeepLinkManager GetInstance()
  {
    return instance;
  }

  private void OnDeepLinkActivated(string url)
  {
    string paramsStr = url.Substring(url.IndexOf("?") + 1);
    string[] paramsArr = paramsStr.Split('&');
    Dictionary<string, string> paramsDict = new Dictionary<string, string>();
    foreach (string param in paramsArr)
    {
      string[] paramArr = param.Split('=');
      paramsDict.Add(paramArr[0], UnityWebRequest.UnEscapeURL(paramArr[1]));
    }
    serverUrl = paramsDict["serverUrl"] ?? "ws://192.168.101.2:5656";
    // FOV = float.Parse(paramsDict["FOV"] ?? "80");
  }
}
