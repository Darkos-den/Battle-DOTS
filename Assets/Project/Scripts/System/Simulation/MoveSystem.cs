using Unity.Burst;
using Unity.Collections;
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

        //[BurstCompile]
        void OnUpdate(ref SystemState state) {
            var grid = SystemAPI.GetSingleton<GridComponent>();

            var buffer = new EntityCommandBuffer(Allocator.Temp);

            foreach ((var position, var transform, var destination, var entity) in 
                SystemAPI.Query<RefRW<PositionComponent>, RefRW<LocalTransform>, MovingComponent>()
                .WithEntityAccess()
            ) {
                var dest = new float3(destination.DestinationX, 0, destination.DestinationY) * grid.CellSize;
                dest += new float3(grid.CellSize, 0, grid.CellSize) * 0.5f;
                dest.y = transform.ValueRO.Position.y;

                var direction = dest - transform.ValueRO.Position;
                direction = math.normalize(direction);

                var destRotation = quaternion.LookRotation(direction, new float3(0, 1, 0));

                //var angle = Quaternion.Angle(transform.ValueRW.Rotation, destRotation);
                //Debug.Log(">> angle: " + angle);

                transform.ValueRW.Rotation = destRotation;
                if (math.distance(dest, transform.ValueRO.Position) < 0.05) {
                    transform.ValueRW.Position = dest;
                    buffer.RemoveComponent<MovingComponent>(entity);

                    position.ValueRW.X = destination.DestinationX;
                    position.ValueRW.Y = destination.DestinationY;
                } else {
                    transform.ValueRW.Position += direction * Time.deltaTime * speed;
                }
            }

            buffer.Playback(state.EntityManager);
        }
    }
}
