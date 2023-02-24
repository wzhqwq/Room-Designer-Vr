using Google.XR.Cardboard;
using UnityEngine;
using System;
using UnityEngine.XR;

public class VrManager : MonoBehaviour
{
  private const String deviceParams = "https://google.com/cardboard/cfg?p=CgZHb29nbGUSEkNhcmRib2FyZCBJL08gMjAxNR2ZuxY9JbbzfT0qEAAASEIAAEhCAABIQgAASEJYADUpXA89OgiCc4Y-MCqJPlAAYAM";

  public void Start()
  {
#if !UNITY_EDITOR
    // XRDevice.fovZoomFactor = 1.2f;
    Screen.sleepTimeout = SleepTimeout.NeverSleep;
    Screen.brightness = 1.0f;
    Api.SaveDeviceParams(deviceParams);
#endif
  }

  public void Update()
  {
#if !UNITY_EDITOR
    if (Api.IsCloseButtonPressed)
    {
      Application.Quit();
    }

    if (Api.IsGearButtonPressed)
    {
      Api.ScanDeviceParams();
    }

    if (Api.IsTriggerHeldPressed)
    {
      Api.Recenter();
    }

    if (Api.HasNewDeviceParams())
    {
      Api.ReloadDeviceParams();
    }

    Api.UpdateScreenParams();
#endif
  }
}
