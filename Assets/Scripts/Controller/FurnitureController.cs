using UnityEngine;

public class FurnitureController : OperableController
{
  private bool selected = false;
  private bool marked = false;
  private MeshRenderer selectionRenderer;

  void Start()
  {
    selectionRenderer = transform.Find("Selection").GetComponent<MeshRenderer>();
  }

  override public void OnPointerEnter()
  {
    if (!selected)
    {
      selectionRenderer.material = RoomScene.focusedMaterial;
    }
  }
  override public void OnPointerExit()
  {
    if (!selected)
    {
      selectionRenderer.material = RoomScene.normalMaterial;
    }
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
  public void Mark()
  {
    marked = true;
    selectionRenderer.material = RoomScene.markedMaterial;
  }
  public void UnMark()
  {
    marked = false;
    selectionRenderer.material = RoomScene.normalMaterial;
  }
  public void Select()
  {
    selected = true;
    selectionRenderer.material = RoomScene.selectedMaterial;
  }
  public void Unselect()
  {
    selected = false;
    selectionRenderer.material = RoomScene.normalMaterial;
  }
}