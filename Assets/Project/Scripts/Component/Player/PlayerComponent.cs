using Unity.Entities;

namespace Darkos {
    public struct PlayerComponent : IComponentData {

        public int Id;
        public int PlayerLayerIndex;
    }
}
