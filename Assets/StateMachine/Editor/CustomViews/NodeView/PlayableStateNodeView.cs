using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;

namespace StateMachine {

    public class PlayableStateNodeView : BaseNodeView {

        public PlayableStateNodeView(StateNode node) : base(node) {
            RegisterCallback<GeometryChangedEvent>(GeometryChangedCallback);
        }

        protected override void CreateInputPorts() {
            var orientation = Orientation.Vertical;
            if (node.horizontalInput) {
                orientation = Orientation.Horizontal;
            }

            input = InstantiatePort(orientation, Direction.Input, Port.Capacity.Multi, typeof(bool));
            input.portName = "";
            inputContainer.Add(input);
        }

        protected override void CreateOutputPorts() {
            var orientation = Orientation.Vertical;
            if (node.horizontalOutput) {
                orientation = Orientation.Horizontal;
            }

            output = InstantiatePort(orientation, Direction.Output, Port.Capacity.Multi, typeof(bool));
            output.portName = "";
            outputContainer.Add(output);
        }

        private void GeometryChangedCallback(GeometryChangedEvent evt) {
            UnregisterCallback<GeometryChangedEvent>(GeometryChangedCallback);

            //todo
        }

        #region Public API

        public void Activate() {
            var result = this.Query(name = "Title").ToList().First();
            result.style.backgroundColor = Color.red;
        }

        public void Deactivate() {
            var result = this.Query(name = "Title").ToList().First();
            var color = new Color(0.247f, 0.247f, 0.247f, 0.804f);
            result.style.backgroundColor = color;
        }

        #endregion
    }
}
