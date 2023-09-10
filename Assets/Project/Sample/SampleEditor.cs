using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class SampleEditor : EditorWindow
{
    
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/SampleEditor")]
    public static void ShowExample()
    {
        SampleEditor wnd = GetWindow<SampleEditor>();
        wnd.titleContent = new GUIContent("SampleEditor");
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
    }
    

    /*
    Rect windowRect = new Rect(100 + 100, 100, 100, 100);
    Rect windowRect2 = new Rect(100, 100, 100, 100);

    SampleView sampleView;


    [MenuItem("Window/UI Toolkit/SampleEditor")]
    public static void ShowExample() {
        SampleEditor wnd = GetWindow<SampleEditor>();
        wnd.titleContent = new GUIContent("SampleEditor");
    }

    private void OnGUI() {
        Handles.BeginGUI();
        Handles.DrawBezier(windowRect.center, windowRect2.center, new Vector2(windowRect.center.x, windowRect.center.y), new Vector2(windowRect2.center.x, windowRect2.center.y), Color.red, null, 5f);
        Handles.EndGUI();

        BeginWindows();
        windowRect = GUI.Window(0, windowRect, WindowFunction, "Box1");
        windowRect2 = GUI.Window(1, windowRect2, WindowFunction, "Box2");

        EndWindows();

    }
    void WindowFunction(int windowID) {
        GUI.DragWindow();
    }
    */
}

public class SampleView: VisualElement {

    public new class UxmlFactory : UxmlFactory<SampleView, VisualElement.UxmlTraits> { };

    static readonly Vertex[] k_Vertices = new Vertex[4];
    static readonly ushort[] k_Indices = { 0, 1, 2, 2, 3, 0 };

    static SampleView() {
        k_Vertices[0].tint = Color.red;
        k_Vertices[1].tint = Color.red;
        k_Vertices[2].tint = Color.red;
        k_Vertices[3].tint = Color.red;
    }

    public SampleView() {
        generateVisualContent += OnGenerateVisualContent;
    }


    void OnGenerateVisualContent(MeshGenerationContext mgc) {
        /*
        Rect r = contentRect;
        if (r.width < 0.01f || r.height < 0.01f)
            return; // Skip rendering when too small.

        float left = 0;
        float right = 100;
        float top = 0;
        float bottom = 100;

        k_Vertices[0].position = new Vector3(left, bottom, Vertex.nearZ);
        k_Vertices[1].position = new Vector3(left, top, Vertex.nearZ);
        k_Vertices[2].position = new Vector3(right, top, Vertex.nearZ);
        k_Vertices[3].position = new Vector3(right, bottom, Vertex.nearZ);

        MeshWriteData mwd = mgc.Allocate(k_Vertices.Length, k_Indices.Length);

        Debug.Log(">>>>> daw line");

        // Since the texture may be stored in an atlas, the UV coordinates need to be
        // adjusted. Simply rescale them in the provided uvRegion.
        Rect uvRegion = mwd.uvRegion;
        k_Vertices[0].uv = new Vector2(0, 0) * uvRegion.size + uvRegion.min;
        k_Vertices[1].uv = new Vector2(0, 1) * uvRegion.size + uvRegion.min;
        k_Vertices[2].uv = new Vector2(1, 1) * uvRegion.size + uvRegion.min;
        k_Vertices[3].uv = new Vector2(1, 0) * uvRegion.size + uvRegion.min;

        mwd.SetAllVertices(k_Vertices);
        mwd.SetAllIndices(k_Indices);
        */

        var paint2D = mgc.painter2D;

        paint2D.lineWidth = 10.0f;
        paint2D.strokeColor = Color.white;
        paint2D.lineJoin = LineJoin.Round;
        paint2D.lineCap = LineCap.Round;

        paint2D.BeginPath();
        paint2D.MoveTo(new Vector2(100, 100));
        paint2D.LineTo(new Vector2(120, 120));
        paint2D.LineTo(new Vector2(140, 100));
        paint2D.LineTo(new Vector2(160, 120));
        paint2D.LineTo(new Vector2(180, 100));
        paint2D.LineTo(new Vector2(200, 120));
        paint2D.LineTo(new Vector2(220, 100));
        paint2D.Stroke();
    }
}