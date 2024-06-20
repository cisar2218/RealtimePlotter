using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/MainEditor")]
    public static void ShowExample()
    {
        MainEditor wnd = GetWindow<MainEditor>();
        wnd.titleContent = new GUIContent("MainEditor");
    }


    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/UI/MainEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        root.Add(m_VisualTreeAsset.Instantiate());

        //Call the event handler
        SetupButtonHandler();
    }


    void SetupButtonHandler() {
        VisualElement root = rootVisualElement;

        var buttons = root.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }

    private void RegisterHandler(Button button)
    {
        button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        Debug.Log("Button was clicked");
    }
}
