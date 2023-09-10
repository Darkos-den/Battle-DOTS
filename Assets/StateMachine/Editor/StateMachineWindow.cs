using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using StateMachine;

public class StateMachineWindow : EditorWindow
{
    private StateGraphView _treeView;
    private ObjectField _objectField;
    private Toggle _toggle;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/StateMachine")]
    public static void ShowExample()
    {
        StateMachineWindow wnd = GetWindow<StateMachineWindow>();
        wnd.titleContent = new GUIContent("StateMachineWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        m_VisualTreeAsset.CloneTree(root);

        ApplyStyles(root);
        BindViews(root);
    }

    #region Initialization

    private void ApplyStyles(VisualElement root) {
        var styles = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StateMachine/Editor/StateMachine.uss");
        root.styleSheets.Add(styles);
    }

    private void BindViews(VisualElement root) {
        _treeView = root.Q<StateGraphView>();
        _objectField = root.Q<ObjectField>();
        _toggle = root.Q<Toggle>();

        _objectField.RegisterValueChangedCallback(OnObjectSelected);
        _toggle.RegisterValueChangedCallback(OnEditorModeValueChanged);
    }

    #endregion

    private void OnObjectSelected(ChangeEvent<Object> e) {
        if (e.newValue == e.previousValue) {
            return;
        }

        if (e.newValue == null) {
            _treeView.ClearTree();
        } else {
            var value = e.newValue as StateTree;
            if (value) {
                _treeView.PopulateView(value);
            }
        }
    }

    private void OnEditorModeValueChanged(ChangeEvent<bool> e) {
        if (e.newValue == e.previousValue) {
            return;
        }

        _treeView.ApplyEditMode(e.newValue);
    }
}
