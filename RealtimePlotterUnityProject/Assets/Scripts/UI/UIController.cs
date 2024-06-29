using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private WSClient client;
    private VoxelRenderer particleRenderer;

    private CameraController cameraController;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        client = FindAnyObjectByType<WSClient>();
        particleRenderer = FindAnyObjectByType<VoxelRenderer>();
        cameraController = FindAnyObjectByType<CameraController>();

        Button connectButton = root.Q<Button>("connect");
        Button loadButton = root.Q<Button>("load");
        Button findButton = root.Q<Button>("find-dron");

        VisualElement indicator = root.Q<VisualElement>("indicator");

        Label statusLabel = root.Q<Label>("status");

        TextField endpointTextfield = root.Q<TextField>("endpoint");

        Toggle watchDronToggle = root.Q<Toggle>("watch-dron-toggle");

        loadButton.SetEnabled(false);

        findButton.clicked += () =>
        {
            cameraController.dronToBeFound = true;
        };

        watchDronToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
        {
            cameraController.isWatchingDron = evt.newValue;
        });

        connectButton.clicked += () =>
        {
            client.endpoint = endpointTextfield.text;
            endpointTextfield.SetEnabled(false);
            client.ToggleConnection();
            connectButton.SetEnabled(false);
        };

        loadButton.clicked += () =>
        {
            particleRenderer.voxelsCount = 0; // clear
            var loader = FindAnyObjectByType<DataLoader>();

            StartCoroutine(loader.GetMeasurement("zaznam3"));
        };

        client.OnConnectionChanged += (status) =>
        {
            switch (status)
            {
                case WSStatus.Opened:
                    statusLabel.text = "Opened";
                    connectButton.text = "Disconnect";
                    indicator.style.backgroundColor = Color.green;

                    loadButton.SetEnabled(true);
                    break;

                case WSStatus.Error:
                    statusLabel.text = "Error";
                    indicator.style.backgroundColor = Color.red;

                    loadButton.SetEnabled(false);
                    endpointTextfield.SetEnabled(true);
                    break;

                case WSStatus.Closed:

                    statusLabel.text = "Closed";
                    connectButton.text = "Connect";
                    indicator.style.backgroundColor = Color.grey;

                    loadButton.SetEnabled(false);
                    endpointTextfield.SetEnabled(true);

                    // try to reconnect automatically
                    client.Connect();
                    break;
            }
            connectButton.SetEnabled(true);
        };
    }
}
