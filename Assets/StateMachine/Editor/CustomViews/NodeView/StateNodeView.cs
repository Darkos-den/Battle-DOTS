using UnityEditor.Experimental.GraphView;

namespace StateMachine {

    public class StateNodeView : BaseNodeView {

        public StateNodeView(StateNode node) : base(node) {
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
    }
}
