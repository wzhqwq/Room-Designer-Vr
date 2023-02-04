using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private const float _maxDistance = 10;
  private GameObject _gazedAtObject = null;
  private bool FOVSet = false;

  private IEnumerator gazeTimer = null;

  public void Start() {
    PlayerController.UpdatePlayer();
  }

  public void Update()
  {
    if (!FOVSet && DeepLinkManager.GetInstance().FOV != 0) {
      Camera.main.fieldOfView = DeepLinkManager.GetInstance().FOV;
      FOVSet = true;
    }
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
    {
      GameObject hitObject = hit.transform.parent.gameObject;
      if (hitObject.tag == "Operable")
      {
        if (_gazedAtObject != hitObject)
        {
          if (_gazedAtObject != null)
          {
            _gazedAtObject.SendMessage("OnPointerExit");
          }
          _gazedAtObject = hitObject;
          _gazedAtObject.SendMessage("OnPointerEnter");

          gazeTimer = StartGazeTimer(_gazedAtObject);
          StartCoroutine(gazeTimer);
        }
      }
      else
      {
        if (_gazedAtObject != null)
        {
          _gazedAtObject.SendMessage("OnPointerExit");
          _gazedAtObject = null;
          if (gazeTimer != null)
          {
            StopCoroutine(gazeTimer);
            gazeTimer = null;
          }
        }
      }
    }
    else
    {
      _gazedAtObject?.SendMessage("OnPointerExit");
      _gazedAtObject = null;
      if (gazeTimer != null)
      {
        StopCoroutine(gazeTimer);
        gazeTimer = null;
      }
    }

    // 点击检测
    if (Google.XR.Cardboard.Api.IsTriggerPressed)
    {
      if (_gazedAtObject != null)
      {
        _gazedAtObject.SendMessage("OnPointerClick");
      }
      else
      {
        RoomScene.UnselectFurniture();
      }
    }
  }

  private IEnumerator StartGazeTimer(GameObject gameObject)
  {
    yield return new WaitForSeconds(3);
    gameObject.SendMessage("OnPointerClick");
  }
}
