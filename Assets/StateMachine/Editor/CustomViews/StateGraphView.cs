using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine {

    public class StateGraphView : GraphView {

        public new class UxmlFactory : UxmlFactory<StateGraphView, GraphView.UxmlTraits> { };

        private Vector2 _localMousePosition;
        private StateTree _tree;

        private bool _isEditMode = false;
        private int _lastActiveState = -1;

        public StateGraphView() {
            RegisterCallback<MouseDownEvent>(evt => { _localMousePosition = evt.localMousePosition; });

            AddGridBckground();
            AddStyles();
            AddManipulators();
        }

        #region Initialization

        private void AddGridBckground() {
            var bg = new GridBackground();
            bg.StretchToParentSize();
            Insert(0, bg);
        }

        private void AddStyles() {
            var styles = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StateMachine/Editor/StateMachine.uss");
            styleSheets.Add(styles);
        }

        private void AddManipulators() {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
        }

        #endregion

        #region Overrides

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
            if (graphViewChange.elementsToRemove != null) {
                foreach (var item in graphViewChange.elementsToRemove) {
                    if (item is StateNodeView view) {
                        _tree.DeleteNode(view.node);
                    }

                    if (item is Edge edge) {
                        StateNodeView parentView = edge.output.node as StateNodeView;
                        StateNodeView childView = edge.input.node as StateNodeView;

                        _tree.RemoveChild(parentView.node, childView.node);
                    }
                }
            }
            if (graphViewChange.edgesToCreate != null) {
                foreach (var item in graphViewChange.edgesToCreate) {
                    StateNodeView parentView = item.output.node as StateNodeView;
                    StateNodeView childView = item.input.node as StateNodeView;

                    _tree.AddChild(parentView.node, childView.node);
                }
            }
            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            return ports.ToList()
                .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node)
                .ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            //base.BuildContextualMenu(evt);

            if (_tree != null && _isEditMode) {
                evt.menu.AppendAction("Create Node", (_a) => CreateNode(_a.eventInfo.mousePosition));
            }
        }

        #endregion

        #region Private methods

        private void CreateNode(Vector2 initialPosition) {
            var node = _tree.CreateNode();
            CreateNodeView(node, initialPosition);
        }

        private void CreateNodeView(StateNode node) {
            if (_isEditMode) {
                var nodeView = new StateNodeView(node);
                AddElement(nodeView);
            } else {
                var nodeView = new PlayableStateNodeView(node);
                AddElement(nodeView);
            }
        }

        private void CreateNodeView(StateNode node, Vector2 initialPosition) {
            var nodeView = new StateNodeView(node);
            nodeView.style.left = initialPosition.x;
            nodeView.style.top = initialPosition.y;
            AddElement(nodeView);
        }

        private BaseNodeView FindNodeView(StateNode node) {
            return GetNodeByGuid(node.guid) as BaseNodeView;
        }

        private void OnActiveStateChanged(int stateId) {
            if (_isEditMode || _lastActiveState == stateId) {
                return;
            }

            foreach (var item in _tree.Nodes) {
                if (item.stateId == _lastActiveState) {
                    var nodeView = FindNodeView(item) as PlayableStateNodeView;
                    nodeView.Deactivate();
                }

                if (item.stateId == stateId) {
                    var nodeView = FindNodeView(item) as PlayableStateNodeView;
                    nodeView.Activate();
                }
            }
            _lastActiveState = stateId;
        }

        #endregion

        #region Public API

        public void PopulateView(StateTree tree) {
            if (tree == null) {
                return;
            }

            _tree = tree;
            _tree.OnActiveStateChanged += OnActiveStateChanged;

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

                    /*
                    MyEdge val = new MyEdge {
                        output = parentView.output,
                        input = childView.input,
                    };
                    parentView.output.Connect(val);
                    childView.input.Connect(val);
                    */

                    var val = parentView.output.ConnectTo(childView.input);
                    AddElement(val);
                }
            }
        }

        public void ClearTree() {
            if (_tree == null) {
                return;
            }
            _tree.OnActiveStateChanged -= OnActiveStateChanged;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            _tree = null;
        }

        public void ApplyEditMode(bool enabled) {
            var currentTree = _tree;
            ClearTree();

            _isEditMode = enabled;

            PopulateView(currentTree);
        }

        #endregion
    }
}
