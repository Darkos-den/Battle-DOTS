using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class BehaviourTree : ScriptableObject {

    public Node rootNode;

    public Node.NodeState TreeState = Node.NodeState.Running;
    [SerializeField]
    private List<Node> nodes = new();
    public IEnumerable<Node> Nodes { get { return nodes; } }

    public Node.NodeState Update() {
        if (rootNode.State == Node.NodeState.Running) {
            TreeState = rootNode.Update();
        }
        return TreeState;
    }

    public void ValidateTree() { 
        if (nodes.Count == 0) {
            var root = CreateNode(typeof(RootNode));
            rootNode = root;
        }
    }

    public Node CreateNode(System.Type type) {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node) {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child) {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator != null) {
            decorator.Child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite != null) {
            if (!composite.Children.Contains(child)) {
                composite.Children.Add(child);
            }
        }

        RootNode root = parent as RootNode;
        if (root != null) {
            root.child = child;
        }

        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssets();
    }

    public void RemoveChild(Node parent, Node child) {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator != null) {
            decorator.Child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite != null) {
            if (composite.Children.Contains(child)) {
                composite.Children.Remove(child);
            }
        }

        RootNode root = parent as RootNode;
        if (root != null) {
            root.child = null;
        }

        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssets();
    }

    public List<Node> GetChildred(Node parent) {
        List<Node> children = new();

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator != null && decorator.Child != null) {
            children.Add(decorator.Child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite != null) {
            children.AddRange(composite.Children);
        }

        RootNode root = parent as RootNode;
        if (root != null && root.child != null) {
            children.Add(root.child);
        }

        return children;
    }
}
