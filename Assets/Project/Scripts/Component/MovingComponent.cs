using Unity.Entities;

namespace Darkos {

    public struct MovingComponent : IComponentData {

        public int DestinationX;
        public int DestinationY;
    }
}
