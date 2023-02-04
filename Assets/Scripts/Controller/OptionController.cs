using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class OptionController : OperableController {
  private MeshRenderer backgroundRenderer;
  private TextMeshProUGUI labelGUI;
  public EventHandler OnClick;

  void Awake()
  {
    gameObject.tag = "Operable";
    backgroundRenderer = transform.Find("Background").GetComponent<MeshRenderer>();
    backgroundRenderer.material = ResourceManager.optionUnselectedMaterial;
    labelGUI = transform.Find("Label").GetComponent<TextMeshProUGUI>();
    labelGUI.font = ResourceManager.blueFont;
  }

  void Update()
  {
    transform.LookAt(Camera.main.transform);
  }
  
  public void SetRadius(float radius)
  {
    transform.GetChild(0).localPosition = new Vector3(0, 0, radius);
    transform.GetChild(1).localPosition = new Vector3(0, 0, radius + -0.01f);
  }
  public void SetLabel(string label)
  {
    labelGUI.text = label;
  }

  override public void OnPointerEnter()
  {
    backgroundRenderer.material = ResourceManager.optionSelectedMaterial;
    labelGUI.font = ResourceManager.whiteFont;
  }
  override public void OnPointerExit()
  {
    backgroundRenderer.material = ResourceManager.optionUnselectedMaterial;
    labelGUI.font = ResourceManager.blueFont;
  }
  override public void OnPointerClick()
  {
    OnClick?.Invoke(this, EventArgs.Empty);
  }

  private IEnumerator ShowInCoroutine()
  {
    return Scale(0, 1, 0.3f);
  }
  private IEnumerator HideInCoroutine()
  {
    return Scale(1, 0, 0.3f);
  }
  private IEnumerator RemoveInCoroutine()
  {
    yield return HideInCoroutine();
    Destroy(gameObject);
  }
  public void Show()
  {
    StartCoroutine(ShowInCoroutine());
  }
  public void Remove()
  {
    StartCoroutine(RemoveInCoroutine());
  }

  private IEnumerator Scale(float from, float to, float duration)
  {
    float time = 0;
    while (time < duration)
    {
      time += Time.deltaTime;
      float scale = Mathf.Lerp(from, to, time / duration);
      transform.localScale = new Vector3(scale, scale, scale);
      yield return null;
    }
    transform.localScale = new Vector3(to, to, to);
  }
}