using System;

[Serializable]
public struct GenericWsMessage
{
  public string type;
}

public class MessageType
{
  public const string POSITION = "updatePosition";
  public const string FURNITURE_LIST = "listAllFurniture";
  public const string ADD_FURNITURE = "addFurniture";
  public const string DELETE_FURNITURE = "deleteFurniture";
  public const string UPDATE_FURNITURE = "updateFurniture";
  public const string MARK_FURNITURE = "markFurniture";
  public const string UNMARK_FURNITURE = "unmarkFurniture";
}

[Serializable]
public struct OperationResultMessage
{
  public string type;
  public string data;
}

[Serializable]
public struct PositionMessage
{
  public string type;
  public Position data;
}

[Serializable]
public struct FurnitureListMessage
{
  public string type;
  public Furniture[] data;
}

[Serializable]
public class ListFurnitureMessage
{
  public string type = "listAllFurniture";
  public object data = null;

  public static ListFurnitureMessage Create()
  {
    ListFurnitureMessage message = new ListFurnitureMessage();
    return message;
  }
}

[Serializable]
public class AddFurnitureMessage
{
  public string type = "addFurniture";
  public Furniture data;

  public static AddFurnitureMessage Create(string id, string model, string x, string z, string angle)
  {
    AddFurnitureMessage message = new AddFurnitureMessage();
    message.data = new Furniture();
    message.data.id = id;
    message.data.model = model;
    return message;
  }
}

[Serializable]
public class DeleteFurnitureMessage
{
  public string type = "deleteFurniture";
  public JustId data;

  public static DeleteFurnitureMessage Create(string id)
  {
    DeleteFurnitureMessage message = new DeleteFurnitureMessage();
    message.data = new JustId();
    message.data.id = id;
    return message;
  }
}

[Serializable]
public class UpdateFurnitureMessage
{
  public string type = "updateFurniture";
  public Furniture data;

  public static UpdateFurnitureMessage Create(string id, string model, string x, string z, string angle)
  {
    UpdateFurnitureMessage message = new UpdateFurnitureMessage();
    message.data = new Furniture();
    message.data.id = id;
    message.data.model = model;
    return message;
  }
}

[Serializable]
public class MarkFurnitureMessage
{
  public string type = "markFurniture";
  public JustId data;

  public static MarkFurnitureMessage Create(string id)
  {
    MarkFurnitureMessage message = new MarkFurnitureMessage();
    message.data = new JustId();
    message.data.id = id;
    return message;
  }
}

[Serializable]
public class UnMarkFurnitureMessage
{
  public string type = "unmarkFurniture";
  public JustId data;

  public static UnMarkFurnitureMessage Create(string id)
  {
    UnMarkFurnitureMessage message = new UnMarkFurnitureMessage();
    message.data = new JustId();
    message.data.id = id;
    return message;
  }
}