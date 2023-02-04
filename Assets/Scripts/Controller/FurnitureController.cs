using UnityEngine;

public class FurnitureController : OperableController
{
  private bool selected = false;
  private bool marked = false;
  private bool focused = false;
  private MeshRenderer selectionRenderer;
  private Furniture furniture;

  void Awake()
  {
    gameObject.tag = "Operable";
    selectionRenderer = transform.Find("Selection").GetComponent<MeshRenderer>();
    selectionRenderer.material = ResourceManager.normalMaterial;
    selectionRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    selectionRenderer.receiveShadows = false;
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
    transform.localRotation = Quaternion.Euler(0, furniture.rotation, 0);
    if (marked != furniture.marked)
    {
      if (furniture.marked) Mark();
      else UnMark();
    }
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
}