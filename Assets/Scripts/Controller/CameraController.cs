using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private const float _maxDistance = 10;
  private GameObject _gazedAtObject = null;
  private IEnumerator gazeTimer = null;
  private MeshRenderer indicatorRenderer;
  private Transform indicatorTransform;

  void Awake() {
    GameObject indicator = GameObject.Find("Indicator");
    indicatorRenderer = indicator.GetComponent<MeshRenderer>();
    indicatorTransform = indicator.transform;
  }

  void Update()
  {
    GameObject hitObject;
    if (TryHit(out hitObject))
    {
      // Debug.Log(hitObject.name);
      if (_gazedAtObject != hitObject)
      {
        if (_gazedAtObject != null)
          _gazedAtObject.SendMessage("OnPointerExit");
        _gazedAtObject = hitObject;
        _gazedAtObject.SendMessage("OnPointerEnter");

        StopGazeTimer();
      }
      if (gazeTimer == null)
      {
        gazeTimer = StartGazeTimer(_gazedAtObject);
        StartCoroutine(gazeTimer);
      }
    }
    else
      PointerExit();

    // 点击检测
    if (Google.XR.Cardboard.Api.IsTriggerPressed)
    {
      if (CorrectionScene.IsAlive()) {
        Google.XR.Cardboard.Api.Recenter();
        PlayerController.ClearStartingPoint();
        return;
      }
      StopGazeTimer();
      if (_gazedAtObject != null)
        PointerClick(_gazedAtObject);
      else
        RoomScene.UnselectFurniture();
    }
  }

  private bool TryHit(out GameObject hitObject)
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
    {
      hitObject = hit.transform.gameObject;
      if (hitObject.tag == "Operable") return true;
      hitObject = hit.transform.parent?.gameObject;
      return hitObject?.tag == "Operable";
    }
    else
    {
      hitObject = null;
      return false;
    }
  }

  private void PointerExit()
  {
    if (_gazedAtObject == null) return;

    _gazedAtObject.SendMessage("OnPointerExit");
    _gazedAtObject = null;

    StopGazeTimer();
  }

  private IEnumerator StartGazeTimer(GameObject gameObject)
  {
    if (gameObject == null) yield break;

    float timer = 0;
    while (timer < 3)
    {
      timer += Time.deltaTime;
      indicatorRenderer.material.Lerp(ResourceManager.indicatorNormal, ResourceManager.indicatorActive, timer / 3);
      yield return null;
    }

    if (gameObject != null) PointerClick(gameObject);
    indicatorRenderer.material = ResourceManager.indicatorNormal;
    gazeTimer = null;
  }
  private void StopGazeTimer()
  {
    if (gazeTimer == null) return;

    StopCoroutine(gazeTimer);
    gazeTimer = null;
    indicatorRenderer.material = ResourceManager.indicatorNormal;
  }
  private void PointerClick(GameObject clickObject)
  {
    clickObject.SendMessage("OnPointerClick");
    StartCoroutine(ClickAnimation());
  }
  private IEnumerator ClickAnimation()
  {
    float timer = 0;
    Vector3 originScale = indicatorTransform.localScale;
    while (timer < 0.2f)
    {
      timer += Time.deltaTime;
      indicatorTransform.localScale = Vector3.Lerp(originScale, originScale * 0.5f, timer / 0.2f);
      yield return null;
    }
    timer = 0;
    while (timer < 0.2f)
    {
      timer += Time.deltaTime;
      indicatorTransform.localScale = Vector3.Lerp(originScale * 0.5f, originScale, timer / 0.2f);
      yield return null;
    }
    indicatorTransform.localScale = originScale;
  }
}
