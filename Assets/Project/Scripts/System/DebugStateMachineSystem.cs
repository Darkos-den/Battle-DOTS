using Unity.Entities;

namespace Darkos {

    public partial class DebugStateMachineSystem : SystemBase {

        protected override void OnCreate() {
            RequireForUpdate<DebuggubleState>();
        }

        protected override void OnUpdate() {
            
        }
    }

    public class DebuggubleState: IComponentData {
        public StateMachineController Controller;
    }
}
