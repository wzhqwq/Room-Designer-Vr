using UnityEngine;
using System.Collections;

public class FurnitureController : OperableController
{
  private bool selected = false;
  private bool marked = false;
  private bool focused = false;
  private MeshRenderer selectionRenderer;
  private Furniture furniture;
  private const float animatingTime = 0.2f;

  void Awake()
  {
    gameObject.tag = "Operable";
    selectionRenderer = transform.Find("Selection").GetComponent<MeshRenderer>();
    selectionRenderer.material = ResourceManager.normalMaterial;
    selectionRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    selectionRenderer.receiveShadows = false;
  }

  void Start()
  {
    StartCoroutine(Drop());
  }

  override public void OnPointerEnter()
  {
    focused = true;
    UpdateMaterial();
  }
  override public void OnPointerExit()
  {
    focused = false;
    UpdateMaterial();
  }
  override public void OnPointerClick()
  {
    if (selected)
    {
      RoomScene.UnselectFurniture();
    }
    else
    {
      RoomScene.SelectFurniture(gameObject);
    }
  }
  public void UpdateFurniture(Furniture furniture)
  {
    this.furniture = furniture;
    transform.localPosition = new Vector3(furniture.x, 0, furniture.z);
    transform.localEulerAngles = new Vector3(0, furniture.rotation, 0);
    // if (marked != furniture.marked)
    // {
    //   if (furniture.marked) Mark();
    //   else UnMark();
    // }
  }
  
  public bool IsMarked()
  {
    return marked;
  }
  public void Mark()
  {
    marked = true;
    UpdateMaterial();
  }
  public void UnMark()
  {
    marked = false;
    UpdateMaterial();
  }
  
  public bool IsSelected()
  {
    return selected;
  }
  public void Select()
  {
    selected = true;
    UpdateMaterial();
  }
  public void Unselect()
  {
    selected = false;
    UpdateMaterial();
  }

  private void UpdateMaterial()
  {
    if (selected)
    {
      selectionRenderer.material = ResourceManager.selectedMaterial;
    }
    else if (marked)
    {
      selectionRenderer.material = ResourceManager.markedMaterial;
    }
    else if (focused)
    {
      selectionRenderer.material = ResourceManager.focusedMaterial;
    }
    else
    {
      selectionRenderer.material = ResourceManager.normalMaterial;
    }
  }

  public void OnRemove()
  {
    StartCoroutine(Remove());
  }
  private IEnumerator Remove()
  {
    float timer = 0.0f;
    float x = transform.localPosition.x;
    float z = transform.localPosition.z;
    while (timer < animatingTime)
    {
      float t = timer / animatingTime;
      transform.localScale = new Vector3(1 - t, 1 - t, 1 - t);
      transform.localPosition = new Vector3(x, t, z);
      timer += Time.deltaTime;
      yield return null;
    }
    Destroy(gameObject);
  }
  private IEnumerator Drop()
  {
    float timer = 0.0f;
    float x = transform.localPosition.x;
    float z = transform.localPosition.z;
    while (timer < animatingTime)
    {
      float t = timer / animatingTime;
      transform.localScale = new Vector3(t, t, t);
      transform.localPosition = new Vector3(x, 1 - t, z);
      timer += Time.deltaTime;
      yield return null;
    }
    transform.localScale = new Vector3(1, 1, 1);
    transform.localPosition = new Vector3(x, 0, z);
  }
}