using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private WSClient client;
    private VoxelRenderer particleRenderer;


    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        client = FindAnyObjectByType<WSClient>();
        particleRenderer = FindAnyObjectByType<VoxelRenderer>();

        Button connectButton = root.Q<Button>("connect");        
        Button loadButton = root.Q<Button>("load");
        Button renderButton = root.Q<Button>("render");
        
        Label statusLabel = root.Q<Label>("status");

        connectButton.clicked += () => {
            statusLabel.text = "Connecting ...";
            Debug.Log("attempting connection");
            client.Connect();
        };

        renderButton.clicked += () => {
            particleRenderer.InvokeRendering();
            Debug.Log("rendering invoked");
        };


        loadButton.clicked += () => {
            var loader = FindAnyObjectByType<DataLoader>();

            StartCoroutine(loader.GetMeasurement("zaznam3"));
        };

        client.OnConnectionChanged += (status) => {
            switch (status) {
                case WSStatus.Opened:
                    statusLabel.text = "Opened";
                    break;
                case WSStatus.Error:
                    statusLabel.text = "Error";
                    break;
                case WSStatus.Closed:
                    statusLabel.text = "Closed";
                    break;
            }
        };
    }
}
