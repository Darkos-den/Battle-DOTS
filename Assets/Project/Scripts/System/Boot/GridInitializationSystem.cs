using Unity.Burst;
using Unity.Entities;

namespace Darkos {

    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct GridInitializationSystem : ISystem {

        void OnCreate(ref SystemState state) {
        }

        void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            state.EntityManager.CreateSingleton(new GridComponent {
                Width = 10,
                Height = 10,
                CellSize = 1,
            });

            state.Enabled = false;
        }
    }
}
