using Unity.Entities;
using Unity.Transforms;

namespace Darkos {

    public partial class RenderSelectedIndicatorSystem : SystemBase {
        protected override void OnUpdate() {
            var found = false;
            foreach ((var state, var transform) in 
                SystemAPI.Query<UnitStateComponent, LocalTransform>() 
            ) {
                if (state.Value == UnitState.Active) {
                    var selectionIndicator = SelectionIndicatorPooling.Instance.GetSelectionIndicator();
                    selectionIndicator.transform.position = transform.Position;
                    selectionIndicator.gameObject.SetActive(true);

                    found = true;
                    break;
                }
            }

            if (!found) {
                var selectionIndicator = SelectionIndicatorPooling.Instance.GetSelectionIndicator();
                selectionIndicator.gameObject.SetActive(false);
            }
        }
    }
}
