using Unity.Entities;

namespace Darkos {
    public struct HealthComponent : IComponentData {
        public float MaxValue;
        public float Value;
    }
}
