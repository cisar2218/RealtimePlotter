using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;  // Import the Unity Networking namespace

public class DataLoader : MonoBehaviour
{

    string baseUrl = "http://localhost:3000/loadmeasurement";

    public IEnumerator GetMeasurement(string measurementId)
    {
        string url = String.Format("{0}/{1}", baseUrl, measurementId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the request and wait for the response
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    // Handle the response data (assuming the response is text-based)
                    Debug.Log("Received Len: " + webRequest.downloadHandler.nativeData.Length);
                    Debug.Log("Received Data: " + webRequest.downloadHandler.text);
                    // You can process the text here depending on the data structure
                    // For example, JSON parsing can be done if the server returns JSON
                    break;
                default:
                    Debug.Log(webRequest.result.ToString());
                    break;
            }
        }
    }
}
