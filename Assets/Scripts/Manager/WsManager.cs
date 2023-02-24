using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;

public class WsManager : Singleton<WsManager>
{
  private WebSocket ws;
  private Thread wsThread;
  private ConcurrentQueue<string> receiveQueue = new();
  private ConcurrentQueue<string> sendQueue = new();
  void Start()
  {
    LinkStart();
  }

  void Update()
  {
    if (ws == null) LinkStart();
    if (receiveQueue.TryDequeue(out string message))
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
          HandleMessage(message);
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
      receiveQueue.Enqueue("OPEN");
    };
    ws.OnMessage += (sender, e) =>
    {
      receiveQueue.Enqueue(e.Data);
    };
    ws.OnError += (sender, e) =>
    {
      Debug.Log("WebSocket Error Message: " + e.Message);
      receiveQueue.Enqueue("ERROR");
    };
    ws.OnClose += (sender, e) =>
    {
      Debug.Log("WebSocket Close");
      receiveQueue.Enqueue("CLOSED");
    };
    // WebSocketSharp会不负责任地在当前线程建立TCP连接，如果连不上服务器，会卡死主线程
    // 所以这里用一个新线程来建立连接
    wsThread = new Thread(() =>
    {
      ws.Connect();
      while (true)
        if (sendQueue.TryDequeue(out string messageToSend))
        {
          Debug.Log("send: " + messageToSend);
          ws.Send(messageToSend);
        }
    });
    wsThread.Start();
  }

  private void HandleMessage(string message)
  {
    Debug.Log("receive: " + message);
    GenericWsMessage parsed = JsonUtility.FromJson<GenericWsMessage>(message);

    switch (parsed.type)
    {
      case MessageType.LOCATION:
        LocationMessage locationMessage = JsonUtility.FromJson<LocationMessage>(message);
        PlayerController.UpdatePlayerLocation(locationMessage.data.x, locationMessage.data.z);
        break;
      case MessageType.FURNITURE_LIST:
        FurnitureListMessage furnitureListMessage = JsonUtility.FromJson<FurnitureListMessage>(message);
        RoomScene.ClearFurniture();
        foreach (Furniture furniture in furnitureListMessage.data)
        {
          RoomScene.AddFurniture(furniture);
        }
        break;
      case MessageType.ADD_FURNITURE:
        AddFurnitureMessage addFurnitureMessage = JsonUtility.FromJson<AddFurnitureMessage>(message);
        RoomScene.AddFurniture(addFurnitureMessage.data);
        break;
      case MessageType.DELETE_FURNITURE:
        DeleteFurnitureMessage deleteFurnitureMessage = JsonUtility.FromJson<DeleteFurnitureMessage>(message);
        RoomScene.RemoveFurniture(deleteFurnitureMessage.data.id);
        break;
      case MessageType.UPDATE_FURNITURE:
        UpdateFurnitureMessage updateFurnitureMessage = JsonUtility.FromJson<UpdateFurnitureMessage>(message);
        RoomScene.UpdateFurniture(updateFurnitureMessage.data);
        break;
      default:
        OperationResultMessage operationResultMessage = JsonUtility.FromJson<OperationResultMessage>(message);
        break;
    }
  }

  private IEnumerator OnClose()
  {
    if (SceneManager.GetActiveScene().name != "StartUpScene")
      yield return SceneTransitionManager.GetInstance().LoadSceneCoroutine("StartUpScene");
    else
      yield return new WaitForSeconds(1);
    wsThread.Abort();
    wsThread = null;
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

  public void Mark(int id)
  {
    if (ws == null) return;
    MarkFurnitureMessage message = MarkFurnitureMessage.Create(id);
    sendQueue.Enqueue(JsonUtility.ToJson(message));
  }
  public void UnMark(int id)
  {
    if (ws == null) return;
    UnMarkFurnitureMessage message = UnMarkFurnitureMessage.Create(id);
    sendQueue.Enqueue(JsonUtility.ToJson(message));
  }
  public void ListFurniture()
  {
    if (ws == null) return;
    ListFurnitureMessage message = ListFurnitureMessage.Create();
    Debug.Log("enqueue");
    sendQueue.Enqueue(JsonUtility.ToJson(message));
  }
}