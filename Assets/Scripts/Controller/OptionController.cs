using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class OptionController : OperableController {
  private MeshRenderer backgroundRenderer;
  private TMP_Text labelText;
  public EventHandler OnClick;

  void Awake()
  {
    gameObject.tag = "Operable";
    Transform offset = transform.GetChild(0);
    backgroundRenderer = offset.Find("Background").GetComponent<MeshRenderer>();
    backgroundRenderer.material = ResourceManager.optionUnselectedMaterial;
    labelText = offset.Find("Label").GetComponent<TMP_Text>();
    labelText.font = ResourceManager.blueFont;
  }

  void Start()
  {
    Show();
  }

  void Update()
  {
    transform.LookAt(Camera.main.transform);
  }
  
  public void SetRadius(float radius)
  {
    transform.GetChild(0).localPosition = new Vector3(-radius, 0, radius);
  }
  public void SetLabel(string label)
  {
    labelText.text = label;
  }

  override public void OnPointerEnter()
  {
    backgroundRenderer.material = ResourceManager.optionSelectedMaterial;
    labelText.font = ResourceManager.whiteFont;
  }
  override public void OnPointerExit()
  {
    backgroundRenderer.material = ResourceManager.optionUnselectedMaterial;
    labelText.font = ResourceManager.blueFont;
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