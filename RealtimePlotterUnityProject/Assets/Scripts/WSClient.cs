using System;
using UnityEngine;
using NativeWebSocket;
using CesiumForUnity;
using Unity.Mathematics;
using UnityEngine.Assertions;

public class WSClient : MonoBehaviour
{
    WebSocket websocket;

    bool isMapPositionSet = false;

    public bool IsConnected = false;

    public event Action<WSStatus> OnConnectionChanged;

    private CesiumGeoreference map;

    private VoxelRenderer particleRenderer;
    public Transform dron;

    void Start()
    {

        websocket = new WebSocket("ws://127.0.0.1:3000/ws");
        map = FindObjectOfType<CesiumGeoreference>();
        particleRenderer = FindObjectOfType<VoxelRenderer>();


        // TODO error messages
        Assert.IsNotNull(particleRenderer);
        Assert.IsNotNull(map);
        Assert.IsNotNull(dron);

        websocket.OnOpen += () =>
        {
            IsConnected = true;

            Debug.Log("Connection open!");
            OnConnectionChanged?.Invoke(WSStatus.Opened);
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
            OnConnectionChanged?.Invoke(WSStatus.Error);
        };

        websocket.OnClose += (e) =>
        {
            IsConnected = false;
            Debug.Log("Connection closed!");
            OnConnectionChanged?.Invoke(WSStatus.Closed);
        };

        websocket.OnMessage += (bytes) =>
        {
            // getting the message as a string
            string message = System.Text.Encoding.UTF8.GetString(bytes);

            try
            {
                Message data = JsonUtility.FromJson<Message>(message);
                var msgType = data.type.ToMessageType();

                switch (msgType)
                {
                    case MessageType.Value:
                        {
                            if (!isMapPositionSet) // TODO separate map setting
                            {
                                map.latitude = data.lat;
                                map.longitude = data.lon;
                                map.height = data.alt;
                                isMapPositionSet = true;
                            }

                            Vector3 cartCoords = map.GeoToCartesian(data);
                            particleRenderer.AddPoint(cartCoords, data.value);
                            break;
                        }
                    case MessageType.Pos:
                        {
                            Vector3 cartCoords = map.GeoToCartesian(data);

                            // dron.position = cartCoords; TODO this one is correct
                            dron.position = new Vector3(data.lon, data.alt, data.lat);  // TODO remove this line

                            Debug.Log("Dron position set to: " + dron.position);
                            break;
                        }
                    case MessageType.Att:
                        {
                            // float deg = "radians_value" * Mathf.Rad2Deg; TODO remove if not needed
                            dron.Rotate(data.roll, data.pitch, data.yaw);
                            Debug.Log(String.Format("Dron rotation set to pitch:{0} yaw:{1} roll{2}", data.pitch, data.yaw, data.roll));
                            break;
                        }
                    case MessageType.Clear:
                        {
                            Debug.Log("clear");
                            break;
                        }
                    default:
                        Debug.LogWarning("unknown message received: " + message);
                        break;
                }
            }
            catch (ArgumentException)
            {
                Debug.Log("ArgumentException! " + message);
            }
        };
    }

    async public void ToggleConnection()
    {
        if (IsConnected)
        {
            await websocket.Close();
        }
        else
        {
            await websocket.Connect();
        }
    }

    async public void Connect()
    {
        await websocket.Connect();
    }

    async public void Close()
    {
        await websocket.Close();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            // await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}

public enum WSStatus
{
    Opened,
    Closed,
    Error,
}