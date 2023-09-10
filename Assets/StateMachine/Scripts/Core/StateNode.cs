using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {

    public class StateNode : ScriptableObject {

        public string guid;
        public Vector2 position;

        public string title;
        public int stateId;

        public bool horizontalInput = true;
        public bool horizontalOutput = true;

        private List<StateNode> _children = new();

        public IEnumerable<StateNode> Children { get { return _children; } }

        public void AddChild(StateNode child) {
            if (!_children.Contains(child)) {
                _children.Add(child);
            }
        }

        public void RemoveChild(StateNode child) {
            if (_children.Contains(child)) {
                _children.Remove(child);
            }
        }
    }
}
