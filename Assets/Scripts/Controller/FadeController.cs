using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour
{
  private static Renderer fadeRenderer;
  private static Color fadeColor;
  public const float fadeTime = 1.0f;

  void Awake()
  {
    fadeRenderer = GetComponent<Renderer>();
    fadeColor = fadeRenderer.material.color;
  }

  public static IEnumerator FadeInCoroutine()
  {
    return FadeCoroutine(0.0f);
  }

  public static IEnumerator FadeOutCoroutine()
  {
    return FadeCoroutine(Mathf.PI);
  }

  private static IEnumerator FadeCoroutine(float cosinePhase)
  {
    float timer = 0.0f;
    while (timer < fadeTime)
    {
      float t = timer / fadeTime;
      fadeColor.a = Mathf.Cos(t * Mathf.PI + cosinePhase) * 0.5f + 0.5f;
      fadeRenderer.material.color = fadeColor;
      timer += Time.deltaTime;
      yield return null;
    }
  }
}