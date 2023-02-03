using System.Collections.Generic;
using UnityEngine;

public class RoomScene : MonoBehaviour
{
  private static RoomScene activeInstance;
  private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> furnitureDic = new Dictionary<string, GameObject>();

  public static Material normalMaterial;
  public static Material selectedMaterial;
  public static Material markedMaterial;
  public static Material focusedMaterial;

  private GameObject selectedFurniture;

  void Awake()
  {
    activeInstance = this;
    if (normalMaterial == null)
    {
      normalMaterial = Resources.Load<Material>("Materials/Transparent");
      selectedMaterial = Resources.Load<Material>("Materials/Selected");
      markedMaterial = Resources.Load<Material>("Materials/Marked");
      focusedMaterial = Resources.Load<Material>("Materials/Focused");
    }
  }
  void OnDestroy()
  {
    activeInstance = null;
  }

  void Start()
  {
    Furniture furniture = new Furniture();
    furniture.id = "1";
    furniture.model = "chair_1";
    furniture.x = "0";
    furniture.z = "4";
    furniture.angle = "0";
    AddFurniture(furniture);
  }

  public static void AddPrefab(string name)
  {
    GameObject prefab = Resources.Load<GameObject>("Furniture/Prefabs/" + name);
    prefabs.Add(name, prefab);
  }

  public static GameObject GetWrappedFurniture(string name)
  {
    GameObject wrapper = new GameObject();
    if (!prefabs.ContainsKey(name))
    {
      AddPrefab(name);
    }
    GameObject furniture = Instantiate(prefabs[name]);
    GameObject selection = GameObject.CreatePrimitive(PrimitiveType.Cube);
    furniture.transform.parent = wrapper.transform;
    selection.transform.parent = wrapper.transform;
    selection.name = "Selection";

    BoxCollider collider = furniture.GetComponent<BoxCollider>();
    if (collider != null)
    {
      Bounds bounds = collider.bounds;
      selection.transform.localScale = bounds.size;
      selection.transform.localPosition = new Vector3(0, bounds.size.y / 2, 0);
    }
    MeshRenderer renderer = selection.GetComponent<MeshRenderer>();
    renderer.material = normalMaterial;
    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    renderer.receiveShadows = false;

    return wrapper;
  }

  public static void AddFurniture(Furniture furniture)
  {
    string name = furniture.model;
    GameObject parent = activeInstance.gameObject;
    GameObject child = GetWrappedFurniture(name);

    child.transform.parent = parent.transform;
    child.transform.localPosition = new Vector3(float.Parse(furniture.x), 0, float.Parse(furniture.z));
    child.transform.localRotation = Quaternion.Euler(0, float.Parse(furniture.angle), 0);
    child.name = "F_" + furniture.id;
    child.tag = "Operable";
    child.AddComponent<FurnitureController>();
    activeInstance.furnitureDic.Add(furniture.id, child);
  }
  public static void RemoveFurniture(string id)
  {
    GameObject furniture = activeInstance.furnitureDic[id];
    activeInstance.furnitureDic.Remove(id);
    if (furniture == activeInstance.selectedFurniture)
    {
      activeInstance.selectedFurniture = null;
    }
    Destroy(furniture);
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
}
