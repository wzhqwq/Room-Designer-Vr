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
  private Vector2 firstPosition;

  [SerializeField]
  private PlayerMode mode = PlayerMode.Steady;

  public static void UpdatePlayer()
  {
    activePlayer = FindObjectOfType<PlayerController>();
  }

  public void UpdatePosition(float x, float z)
  {
    Vector3 position = transform.position;
    switch (mode)
    {
      case PlayerMode.Steady:
        return;
      case PlayerMode.Correction:
        if (firstPosition == null)
        {
          firstPosition = new Vector2(x, z);
        }
        else {
          float length = (firstPosition - new Vector2(x, z)).magnitude;
          position.z = length;
        }
        break;
      case PlayerMode.Free:
        position.x = x;
        position.z = z;
        break;
    }
    transform.position = position;
  }

  public static void UpdatePlayerPosition(float x, float z)
  {
    activePlayer.UpdatePosition(x, z);
  }
  public static void SetCorrectionAngle(float angle)
  {
    if (activePlayer != null)
    {
      activePlayer.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
  }
}
