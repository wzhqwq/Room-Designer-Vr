using UnityEngine;


public enum PlayerMode
{
  Steady,
  Correction,
  Free
}

public class PlayerController : MonoBehaviour
{
  private static PlayerController activePlayer;
  private static float correctionAngle = 0.0f;
  private Vector2? startingPoint;

  [SerializeField]
  private PlayerMode mode = PlayerMode.Steady;

  void Awake()
  {
    if (mode == PlayerMode.Free)
    {
      // 通过Camera Offset矫正相机角度
      activePlayer.transform.GetChild(0).rotation = Quaternion.Euler(0, correctionAngle, 0);
    }
  }

  public static void UpdatePlayer()
  {
    activePlayer = FindObjectOfType<PlayerController>();
  }

  public void UpdateLocation(float x, float z)
  {
    Vector3 position = transform.position;
    switch (mode)
    {
      case PlayerMode.Steady:
        return;
      case PlayerMode.Correction:
        if (startingPoint == null)
        {
          startingPoint = new Vector2(x, z);
        }
        else {
          float length = (startingPoint - new Vector2(x, z))?.magnitude ?? 0;
          position.z = length;
          correctionAngle = Mathf.Atan2(z - startingPoint.Value.y, x - startingPoint.Value.x) * Mathf.Rad2Deg;
        }
        break;
      case PlayerMode.Free:
        position.x = x;
        position.z = z;
        break;
    }
    transform.position = position;
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.name == "Destination")
      CorrectionScene.DestinationReached();
  }

  public static void UpdatePlayerLocation(float x, float z)
  {
    activePlayer.UpdateLocation(x, z);
  }
  public static void ClearStartingPoint()
  {
    activePlayer.startingPoint = null;
  }
  public static void StopCorrection()
  {
    activePlayer.mode = PlayerMode.Steady;
  }
}
