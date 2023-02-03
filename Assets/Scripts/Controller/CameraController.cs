using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private const float _maxDistance = 10;
  private GameObject _gazedAtObject = null;
  private bool FOVSet = false;

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
      // 指针指向了新的物体
      GameObject hitObject = hit.transform.parent.gameObject;
      if (_gazedAtObject != hitObject)
      {
        _gazedAtObject?.SendMessage("OnPointerExit");
        if (hitObject.tag == "Operable")
        {
          _gazedAtObject = hitObject;
          _gazedAtObject.SendMessage("OnPointerEnter");
        }
        else
        {
          _gazedAtObject = null;
        }
      }
    }
    else
    {
      // 啥都没看
      _gazedAtObject?.SendMessage("OnPointerExit");
      _gazedAtObject = null;
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
}
