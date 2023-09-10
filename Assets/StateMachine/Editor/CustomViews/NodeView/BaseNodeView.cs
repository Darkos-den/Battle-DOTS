using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace StateMachine {

    public abstract class BaseNodeView : Node {

        public StateNode node;
        public Port input;
        public Port output;

        public BaseNodeView(StateNode node) {
            this.node = node;
            this.title = node.title;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        public override void SetPosition(Rect newPos) {
            base.SetPosition(newPos);

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
        }

        protected abstract void CreateInputPorts();

        protected abstract void CreateOutputPorts();
    }
}
