using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTreeView treeView;
    InspectorView inspectorView;

    ObjectField objectField;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/BehaviourTreeEditor")]
    public static void ShowExample()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);

        // Instantiate UXML
        //VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        //root.Add(labelFromUXML);
        m_VisualTreeAsset.CloneTree(root);

        var styles = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Project/Editor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styles);

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();
        objectField = root.Q<ObjectField>();

        objectField.RegisterValueChangedCallback(OnObjectSelected);
    }

    private void OnObjectSelected(ChangeEvent<Object> e) {
        if (e.newValue == null) {
            treeView.ClearTree();
        } else {
            var value = e.newValue as BehaviourTree;
            if (value) {
                treeView.PopulateView(value);
            }
        }
    }
}
