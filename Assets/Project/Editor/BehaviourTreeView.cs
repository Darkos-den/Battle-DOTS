using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using static UnityEditor.Experimental.GraphView.GraphView;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BehaviourTreeView : GraphView {

    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { };

    BehaviourTree tree;

    Vector2 localMousePosition;

    public BehaviourTreeView() {
        RegisterCallback<MouseDownEvent>(evt => { localMousePosition = evt.localMousePosition; });

        AddGridBckground();
        AddStyles();
        AddManipulators();
    }

    private void AddGridBckground() {
        var bg = new GridBackground();
        bg.StretchToParentSize();
        Insert(0, bg);
    }

    private void AddStyles() {
        var styles = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Project/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styles);
    }

    private void AddManipulators() {
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        //this.AddManipulator(new RectangleSelector());
    }

    public void PopulateView(BehaviourTree tree) {
        tree.ValidateTree();
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        foreach (var item in tree.Nodes) {
            CreateNodeView(item);
        }

        foreach (var parent in tree.Nodes) {
            var children = tree.GetChildred(parent);
            var parentView = FindNodeView(parent);

            foreach (var child in children) {
                var childView = FindNodeView(child);

                MyEdge val = new MyEdge {
                    output = parentView.output,
                    input = childView.input,
                };
                parentView.output.Connect(val);
                childView.input.Connect(val);

                //var edge = parentView.output.ConnectTo(childView.input);
                val.isGhostEdge = true;
                AddElement(val);
            }
        }
    }

    NodeView FindNodeView(Node node) {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
        return ports.ToList()
            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node)
            .ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
        if (graphViewChange.elementsToRemove != null) {
            foreach (var item in graphViewChange.elementsToRemove) {
                NodeView view = item as NodeView;
                if (view != null) {
                    tree.DeleteNode(view.node);
                }

                Edge edge = item as Edge;
                if (edge != null) {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    tree.RemoveChild(parentView.node, childView.node);
                }
            }
        }
        if (graphViewChange.edgesToCreate != null) {
            foreach (var item in graphViewChange.edgesToCreate) {
                NodeView parentView = item.output.node as NodeView;
                NodeView childView = item.input.node as NodeView;

                tree.AddChild(parentView.node, childView.node);
            }
        }
        return graphViewChange;
    }

    public void ClearTree() {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
        tree = null;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
        //base.BuildContextualMenu(evt);

        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types) {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, localMousePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types) {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, localMousePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types) {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, localMousePosition));
            }
        }
    }

    void CreateNode(System.Type type, Vector2 initialPosition) {
        var node = tree.CreateNode(type);
        CreateNodeView(node, initialPosition);
    }

    void CreateNodeView(Node node) {
        var nodeView = new NodeView(node);
        AddElement(nodeView);
    }

    void CreateNodeView(Node node, Vector2 initialPosition) {
        Debug.Log(">> new pos: " + initialPosition);
        var nodeView = new NodeView(node);
        nodeView.style.left = initialPosition.x;
        nodeView.style.top = initialPosition.y;
        AddElement(nodeView);
    }
}
