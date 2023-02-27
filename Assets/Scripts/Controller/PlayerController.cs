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
  private Vector3 positionNow = new Vector3(0, 0, 0);

  void Awake()
  {
    if (mode == PlayerMode.Free)
    {
      // 通过Camera Offset矫正相机角度
      transform.GetChild(0).rotation = Quaternion.Euler(0, -correctionAngle, 0);
    }
  }

  void Update()
  {
    transform.position = positionNow;
  }

  void Start()
  {
    activePlayer = GetComponent<PlayerController>();
  }
  void OnDestroy()
  {
    if (activePlayer == this)
      activePlayer = null;
  }

  public void UpdateLocation(float x, float z)
  {
    switch (mode)
    {
      case PlayerMode.Steady:
        return;
      case PlayerMode.Correction:
        if (startingPoint == null)
        {
          startingPoint = new Vector2(z, x);
        }
        else
        {
          float length = (startingPoint - new Vector2(z, x))?.magnitude ?? 0;
          positionNow.z = length;
          correctionAngle = Mathf.Atan2(x - startingPoint.Value.y, z - startingPoint.Value.x) * Mathf.Rad2Deg;
        }
        break;
      case PlayerMode.Free:
        positionNow.x = x;
        positionNow.z = z;
        break;
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.name == "Destination")
      CorrectionScene.DestinationReached();
  }

  public static void UpdatePlayerLocation(float x, float z)
  {
    if (activePlayer == null) return;
    activePlayer.UpdateLocation(x, z);
  }
  public static void ClearStartingPoint()
  {
    if (activePlayer == null) return;
    activePlayer.startingPoint = null;
    activePlayer.positionNow = new Vector3(0, 0, 0);
  }
  public static void StopCorrection()
  {
    if (activePlayer == null) return;
    activePlayer.mode = PlayerMode.Steady;
  }
}
