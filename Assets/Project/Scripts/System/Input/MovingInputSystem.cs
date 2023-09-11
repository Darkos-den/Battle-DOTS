using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Darkos {

    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct MovingInputSystem : ISystem {

        void OnCreate(ref SystemState state) { }

        void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var physicWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            var buffer = new EntityCommandBuffer(Allocator.Temp);

            foreach (var input in SystemAPI.Query<DynamicBuffer<MoveInputComponent>>()) {
                foreach (var item in input) {
                    if (physicWorld.CastRay(item.Value, out var hit)) {
                        if (hit.Entity != Entity.Null && !SystemAPI.HasComponent<PositionComponent>(hit.Entity)) {
                            var grid = SystemAPI.GetSingleton<GridComponent>();

                            foreach ((var _, var transform, var entity) in SystemAPI.Query<PositionComponent, LocalTransform>().WithEntityAccess()) {
                                var x = math.clamp(Mathf.FloorToInt(hit.Position.x / grid.CellSize), 0, grid.Width);
                                var y = math.clamp(Mathf.FloorToInt(hit.Position.z / grid.CellSize), 0, grid.Height);

                                if (!SystemAPI.HasComponent<MovingComponent>(entity)) {
                                    buffer.AddComponent(entity, new MovingComponent {
                                        DestinationX = x,
                                        DestinationY = y
                                    });
                                }
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
