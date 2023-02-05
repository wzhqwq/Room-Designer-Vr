using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectionScene : MonoBehaviour
{
  private static CorrectionScene activeInstance;
  private bool handled = false;

  void Awake()
  {
    activeInstance = this;
  }

  void OnDestroy()
  {
    activeInstance = null;
  }

  public static bool IsAlive()
  {
    return activeInstance != null;
  }

  public static void DestinationReached()
  {
    if (activeInstance == null || activeInstance.handled) return;
    activeInstance.handled = true;
    PlayerController.StopCorrection();
    activeInstance.StartCoroutine(activeInstance.DoTransition());
  }

  private IEnumerator DoTransition()
  {
    MeshRenderer ringRenderer = GameObject.Find("Ring").GetComponent<MeshRenderer>();
    Light topLight = GameObject.Find("Light For Cone Top").GetComponent<Light>();
    Light bottomLight = GameObject.Find("Light For Cone Bottom").GetComponent<Light>();
    Color startColor = ringRenderer.material.color;
    Color endColor = new Color(0.5518868f, 1.0f, 0.6005831f);
    float timer = 0.0f;

    while (timer < 0.5f)
    {
      float t = timer / 0.5f;
      ringRenderer.material.color = Color.Lerp(startColor, endColor, t);
      topLight.color = Color.Lerp(startColor, endColor, t);
      bottomLight.color = Color.Lerp(startColor, endColor, t);

      timer += Time.deltaTime;
      yield return null;
    }
    yield return new WaitForSeconds(1.0f);
    SceneTransitionManager.GetInstance().LoadScene("RoomScene");
  }
}
