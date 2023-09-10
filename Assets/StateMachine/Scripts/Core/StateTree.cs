using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine {

    [CreateAssetMenu(menuName = "StateMachine/StateTree", fileName = "NewStateTree")]
    public class StateTree : ScriptableObject {

        [SerializeField] private List<StateNode> nodes = new();

        public IEnumerable<StateNode> Nodes { get { return nodes; } }

        #region Event

        public UnityAction<int> OnActiveStateChanged;

        #endregion

        #region Public API

        public StateNode CreateNode() {
            StateNode node = ScriptableObject.CreateInstance<StateNode>();
            node.title = "Sample Node";
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(StateNode node) {
            nodes.Remove(node);

            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(StateNode parent, StateNode child) {
            parent.AddChild(child);

            EditorUtility.SetDirty(parent);
            AssetDatabase.SaveAssets();
        }

        public void RemoveChild(StateNode parent, StateNode child) {
            parent.RemoveChild(child);

            EditorUtility.SetDirty(parent);
            AssetDatabase.SaveAssets();
        }

        public IEnumerable<StateNode> GetChildred(StateNode parent) {
            return parent.Children;
        }

        #endregion
    }
}
