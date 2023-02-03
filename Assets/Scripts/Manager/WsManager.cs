using UnityEngine;
using WebSocketSharp;
using System.Collections;
using System.Collections.Concurrent;

public class WsManager : Singleton<WsManager>
{
  private WebSocket ws;
  private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
  void Start()
  {
    LinkStart();
  }

  void Update()
  {
    if (ws == null) LinkStart();
    if (messageQueue.TryDequeue(out string message))
    {
      switch (message)
      {
        case "OPEN":
          OnOpen();
          break;
        case "ERROR":
          OnError();
          break;
        case "CLOSED":
          StartCoroutine(OnClose());
          break;
        default:
          OnMessage(message);
          break;
      }
    }
  }

  void OnDestroy()
  {
    if (ws != null) ws.Close();
  }

  private void LinkStart()
  {
    string url = DeepLinkManager.GetInstance().serverUrl;
    if (url == null) return;
    ConnectionController.changeStatusToConnecting(url);
    ws = new WebSocket(url);
    ws.OnOpen += (sender, e) =>
    {
      Debug.Log("WebSocket Open");
      messageQueue.Enqueue("OPEN");
    };
    ws.OnMessage += (sender, e) =>
    {
      messageQueue.Enqueue(e.Data);
    };
    ws.OnError += (sender, e) =>
    {
      Debug.Log("WebSocket Error Message: " + e.Message);
      messageQueue.Enqueue("ERROR");
    };
    ws.OnClose += (sender, e) =>
    {
      Debug.Log("WebSocket Close");
      messageQueue.Enqueue("CLOSED");
    };
    ws.Connect();
  }

  private void OnMessage(string message)
  {
    GenericWsMessage parsed = JsonUtility.FromJson<GenericWsMessage>(message);

    switch (parsed.type)
    {
      case MessageType.POSITION:
        {
          PositionMessage positionMessage = JsonUtility.FromJson<PositionMessage>(message);
          float x = float.Parse(positionMessage.data.x);
          float z = float.Parse(positionMessage.data.z);
          PlayerController.UpdatePlayerPosition(x, z);
          break;
        }
      case MessageType.FURNITURE_LIST:
        FurnitureListMessage furnitureListMessage = JsonUtility.FromJson<FurnitureListMessage>(message);
        break;
      case MessageType.ADD_FURNITURE:
        AddFurnitureMessage addFurnitureMessage = JsonUtility.FromJson<AddFurnitureMessage>(message);
        break;
      case MessageType.DELETE_FURNITURE:
        DeleteFurnitureMessage deleteFurnitureMessage = JsonUtility.FromJson<DeleteFurnitureMessage>(message);
        break;
      default:
        OperationResultMessage operationResultMessage = JsonUtility.FromJson<OperationResultMessage>(message);
        break;
    }
  }

  private IEnumerator OnClose()
  {
    yield return SceneTransitionManager.GetInstance().LoadSceneCoroutine("StartUpScene");
    ws = null;
  }

  private void OnError()
  {
    ConnectionController.changeStatusToFailed();
    ws.Close();
  }

  private void OnOpen()
  {
    SceneTransitionManager.GetInstance().LoadScene("CorrectionScene");
  }

  public void Mark(string id)
  {
    MarkFurnitureMessage message = MarkFurnitureMessage.Create(id);
    ws.Send(JsonUtility.ToJson(message));
  }
  public void UnMark(string id)
  {
    UnMarkFurnitureMessage message = UnMarkFurnitureMessage.Create(id);
    ws.Send(JsonUtility.ToJson(message));
  }
  public void ListFurniture()
  {
    ListFurnitureMessage message = ListFurnitureMessage.Create();
    ws.Send(JsonUtility.ToJson(message));
  }
}