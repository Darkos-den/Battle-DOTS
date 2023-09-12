using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Darkos {

    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct MoveSystem : ISystem {

        private const float speed = 2f;

        void OnCreate(ref SystemState state) {
            state.RequireForUpdate<MovingComponent>();
        }

        void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        void OnUpdate(ref SystemState state) {
            var active = SystemAPI.GetSingleton<ActiveUnit>();

            if (active.Entity == Entity.Null) {
                return;
            }

            var grid = SystemAPI.GetSingleton<GridComponent>();

            var position = SystemAPI.GetComponentRW<PositionComponent>(active.Entity);
            var transform = SystemAPI.GetComponentRW<LocalTransform>(active.Entity);
            var destination = SystemAPI.GetComponent<MovingComponent>(active.Entity);

            var dest = new float3(destination.DestinationX, 0, destination.DestinationY) * grid.CellSize;
            dest += new float3(grid.CellSize, 0, grid.CellSize) * 0.5f;
            dest.y = transform.ValueRO.Position.y;

            var direction = dest - transform.ValueRO.Position;
            direction = math.normalize(direction);

            var destRotation = quaternion.LookRotation(direction, new float3(0, 1, 0));

            transform.ValueRW.Rotation = destRotation;
            if (math.distance(dest, transform.ValueRO.Position) < 0.05) {
                transform.ValueRW.Position = dest;
                state.EntityManager.RemoveComponent<MovingComponent>(active.Entity);

                position.ValueRW.X = destination.DestinationX;
                position.ValueRW.Y = destination.DestinationY;

                SystemAPI.SetSingleton(new ActiveUnit { Entity = Entity.Null });
            } else {
                transform.ValueRW.Position += direction * Time.deltaTime * speed;
            }
        }
    }
}
