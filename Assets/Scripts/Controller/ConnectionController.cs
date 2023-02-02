using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ConnectionController : MonoBehaviour {
  [SerializeField]
  private TMP_Text statusText;
  private static ConnectionController activeInstance;

  void Awake()
  {
    activeInstance = this;
  }

  public static void changeStatusToConnecting(string url)
  {
    activeInstance.statusText.text = "正在连接" + url;
  }
  public static void changeStatusToFailed()
  {
    activeInstance.statusText.text = "连接失败";
  }
}