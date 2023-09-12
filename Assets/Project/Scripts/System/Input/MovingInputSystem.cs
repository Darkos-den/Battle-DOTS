using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Darkos {

    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct MovingInputSystem : ISystem {

        void OnCreate(ref SystemState state) { }

        void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var active = SystemAPI.GetSingleton<ActiveUnit>();
            var physicWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            var buffer = new EntityCommandBuffer(Allocator.Temp);

            foreach (var input in SystemAPI.Query<DynamicBuffer<MoveInputComponent>>()) {
                if (active.Entity == Entity.Null) {
                    input.Clear();
                    continue;
                }
                foreach (var item in input) {
                    if (physicWorld.CastRay(item.Value, out var hit)) {
                        if (hit.Entity != Entity.Null && !SystemAPI.HasComponent<PositionComponent>(hit.Entity)) {
                            var grid = SystemAPI.GetSingleton<GridComponent>();

                            var x = math.clamp(Mathf.FloorToInt(hit.Position.x / grid.CellSize), 0, grid.Width);
                            var y = math.clamp(Mathf.FloorToInt(hit.Position.z / grid.CellSize), 0, grid.Height);

                            if (!SystemAPI.HasComponent<MovingComponent>(active.Entity)) {
                                buffer.AddComponent(active.Entity, new MovingComponent {
                                    DestinationX = x,
                                    DestinationY = y
                                });
                            }
                        }
                    }
                }
                input.Clear();
            }

            buffer.Playback(state.EntityManager);
        }
    }
}
