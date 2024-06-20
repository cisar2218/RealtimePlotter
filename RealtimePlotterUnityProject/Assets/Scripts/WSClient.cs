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

    public event Action<WSStatus> OnConnectionChanged;

    private CesiumGeoreference map;

    private VoxelRenderer particleRenderer;

    void Start()
    {

        websocket = new WebSocket("ws://127.0.0.1:3000/ws");
        map = FindObjectOfType<CesiumGeoreference>();
        particleRenderer = FindObjectOfType<VoxelRenderer>();

        
        // TODO error messages
        Assert.IsNotNull(particleRenderer);
        Assert.IsNotNull(map);

        websocket.OnOpen += () =>
        {
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
                if (data.type.ToMessageType() == MessageType.Value)
                {
                    Debug.Log("Data as String: "+ data);

                    if (!isMapPositionSet) { // TODO separate map setting
                        map.latitude = data.lat;
                        map.longitude = data.lon;
                        map.height = data.alt;
                        isMapPositionSet = true;
                    } else {
                        double3 earthFixed = CesiumWgs84Ellipsoid.LongitudeLatitudeHeightToEarthCenteredEarthFixed(new double3(data.lon, data.lat, data.alt));
                        double3 coords = map.TransformEarthCenteredEarthFixedPositionToUnity(earthFixed);
                        Debug.Log(coords);

                        particleRenderer.AddPoint(new Vector3((float)coords.x, (float)coords.y, (float)coords.z), data.value);
                    }

                } else if  (data.type.ToMessageType() == MessageType.Clear) {
                    Debug.Log("clear");
                }
            }
            catch (ArgumentException)
            {
                Debug.Log("ArgumentException! " + message);
            }
        };
    }

    async public void Connect()
    {
        // waiting for messages
        await websocket.Connect();
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

public enum WSStatus {
    Opened,
    Closed,
    Error,
}