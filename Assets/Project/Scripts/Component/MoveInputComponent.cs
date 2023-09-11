using Unity.Entities;
using Unity.Physics;

namespace Darkos {

    public struct MoveInputComponent : IBufferElementData {

        public RaycastInput Value;
    }
}
