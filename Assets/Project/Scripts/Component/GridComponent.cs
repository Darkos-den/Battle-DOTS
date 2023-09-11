using Unity.Entities;

namespace Darkos {

    public struct GridComponent : IComponentData {

        public int Width;
        public int Height;
        public float CellSize;
    }
}
