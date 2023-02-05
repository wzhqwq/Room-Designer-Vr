using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private const float _maxDistance = 10;
  private GameObject _gazedAtObject = null;
  private bool FOVSet = false;

  private IEnumerator gazeTimer = null;
  private MeshRenderer indicatorRenderer;

  void Awake() {
    indicatorRenderer = GameObject.Find("Indicator").GetComponent<MeshRenderer>();
  }

  void Start() {
    PlayerController.UpdatePlayer();
  }

  void Update()
  {
    if (!FOVSet && DeepLinkManager.GetInstance().FOV != 0) {
      Camera.main.fieldOfView = DeepLinkManager.GetInstance().FOV;
      FOVSet = true;
    }
    GameObject hitObject;
    if (TryHit(out hitObject))
    {
      Debug.Log(hitObject.name);
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
      StopGazeTimer();
      if (_gazedAtObject != null)
        _gazedAtObject.SendMessage("OnPointerClick");
      else
        RoomScene.UnselectFurniture();
    }
  }

  private bool TryHit(out GameObject hitObject)
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
    {
      hitObject = hit.transform.parent.gameObject;
      return hitObject.tag == "Operable";
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

    if (gameObject != null) gameObject.SendMessage("OnPointerClick");
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
}
