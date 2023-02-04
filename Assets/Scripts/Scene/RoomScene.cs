using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomScene : MonoBehaviour
{
  private static RoomScene activeInstance;
  private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> furnitureDic = new Dictionary<string, GameObject>();

  private GameObject selectedFurniture;
  private List<OptionController> shownOptions = new List<OptionController>();

  void Awake()
  {
    activeInstance = this;
  }
  void OnDestroy()
  {
    activeInstance = null;
  }

  void Start()
  {
    WsManager.GetInstance().ListFurniture();
  }

  public static void AddPrefab(string name)
  {
    GameObject prefab = Resources.Load<GameObject>("Furniture/Prefabs/" + name);
    prefabs.Add(name, prefab);
  }

  public static GameObject GetWrappedFurniture(string name)
  {
    GameObject wrapper = new GameObject();

    if (!prefabs.ContainsKey(name)) AddPrefab(name);
    GameObject furniture = Instantiate(prefabs[name]);
    furniture.transform.SetParent(wrapper.transform);
    furniture.name = "Furniture";
    furniture.layer = 2;

    GameObject selection = GameObject.CreatePrimitive(PrimitiveType.Cube);
    selection.transform.SetParent(wrapper.transform);
    selection.name = "Selection";

    BoxCollider collider = furniture.GetComponent<BoxCollider>();
    if (collider != null)
    {
      Bounds bounds = collider.bounds;
      selection.transform.localScale = bounds.size;
      selection.transform.localPosition = new Vector3(0, bounds.size.y / 2 + 0.01f, 0);
    }

    return wrapper;
  }

  public static void AddFurniture(Furniture furniture)
  {
    string name = furniture.model;
    GameObject parent = activeInstance.gameObject;
    GameObject child = GetWrappedFurniture(name);

    child.transform.parent = parent.transform;
    child.name = "F_" + furniture.id;
    child.AddComponent<FurnitureController>();
    child.GetComponent<FurnitureController>().UpdateFurniture(furniture);
    activeInstance.furnitureDic.Add(furniture.id, child);
  }
  public static void RemoveFurniture(string id)
  {
    GameObject furniture = activeInstance.furnitureDic[id];
    if (furniture == null) return;

    if (furniture == activeInstance.selectedFurniture)
      UnselectFurniture();
    activeInstance.furnitureDic.Remove(id);
    Destroy(furniture);
  }
  public static void ClearFurniture()
  {
    UnselectFurniture();
    foreach (GameObject furniture in activeInstance.furnitureDic.Values)
    {
      Destroy(furniture);
    }
    activeInstance.furnitureDic.Clear();
  }
  public static void UpdateFurniture(Furniture furniture)
  {
    GameObject child = activeInstance.furnitureDic[furniture.id];
    child?.GetComponent<FurnitureController>().UpdateFurniture(furniture);
  }

  public static void SelectFurniture(GameObject furniture)
  {
    activeInstance.selectedFurniture?.GetComponent<FurnitureController>().Unselect();
    activeInstance.selectedFurniture = furniture;
    furniture.GetComponent<FurnitureController>().Select();
  }
  public static void UnselectFurniture()
  {
    if (activeInstance == null) return;
    ClearOptions();
    activeInstance.selectedFurniture?.GetComponent<FurnitureController>().Unselect();
    activeInstance.selectedFurniture = null;
  }
  public static void MarkSelected()
  {
    activeInstance.selectedFurniture?.GetComponent<FurnitureController>().Mark();
    WsManager.GetInstance().Mark(activeInstance.selectedFurniture.name.Substring(2));
  }
  public static void UnMarkSelected()
  {
    activeInstance.selectedFurniture?.GetComponent<FurnitureController>().UnMark();
    WsManager.GetInstance().UnMark(activeInstance.selectedFurniture.name.Substring(2));
  }

  private static void ShowOptions(GameObject target)
  {
    float top = target.transform.Find("Selection").localScale.y / 2 + 0.5f;
    bool marked = target.GetComponent<FurnitureController>().IsMarked();

    GameObject optionMark = Instantiate(ResourceManager.optionPrefab);
    optionMark.transform.SetParent(target.transform);
    optionMark.transform.localPosition = new Vector3(0, top, 0);
    optionMark.AddComponent<OptionController>();

    OptionController markController = optionMark.GetComponent<OptionController>();
    markController.SetRadius(target.transform.Find("Selection").localScale.x / 2 * Mathf.Sqrt(2) + 0.5f);
    markController.SetLabel(marked ? "去除标记" : "标记");
    markController.OnClick = (s, e) =>
    {
      if (marked)
        UnMarkSelected();
      else
        MarkSelected();
      UnselectFurniture();
    };
    activeInstance.shownOptions.Add(markController);
  }
  private static void ClearOptions()
  {
    activeInstance.shownOptions.ForEach(o => o.Remove());
    activeInstance.shownOptions.Clear();
  }
}
