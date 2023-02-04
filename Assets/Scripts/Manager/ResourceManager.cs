using TMPro;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
  public static Material normalMaterial;
  public static Material selectedMaterial;
  public static Material markedMaterial;
  public static Material focusedMaterial;
  public static Material optionUnselectedMaterial;
  public static Material optionSelectedMaterial;
  public static TMP_FontAsset whiteFont;
  public static TMP_FontAsset blueFont;
  public static GameObject optionPrefab;

  void Awake()
  {
    normalMaterial = Resources.Load<Material>("Materials/Transparent");
    selectedMaterial = Resources.Load<Material>("Materials/Selected");
    markedMaterial = Resources.Load<Material>("Materials/Marked");
    focusedMaterial = Resources.Load<Material>("Materials/Focused");
    optionUnselectedMaterial = Resources.Load<Material>("Materials/OptionUnselected");
    optionSelectedMaterial = Resources.Load<Material>("Materials/OptionSelected");
    whiteFont = Resources.Load<TMP_FontAsset>("Fonts/Font White");
    blueFont = Resources.Load<TMP_FontAsset>("Fonts/Font Blue");
    optionPrefab = Resources.Load<GameObject>("Prefabs/Option");
  }
}
