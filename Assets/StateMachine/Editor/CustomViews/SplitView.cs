using UnityEngine.UIElements;

namespace StateMachine {

    public class SplitView : TwoPaneSplitView {

        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { };
    }

}