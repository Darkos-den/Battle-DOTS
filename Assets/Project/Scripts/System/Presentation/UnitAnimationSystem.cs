using Darkos;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

[UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
public partial struct UnitAnimateSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        var grid = SystemAPI.GetSingleton<GridComponent>();
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (playerGameObjectPrefab, entity) in
                 SystemAPI.Query<PlayerGameObjectPrefab>().WithNone<PlayerAnimatorReference>().WithEntityAccess()) {
            var newCompanionGameObject = Object.Instantiate(playerGameObjectPrefab.Value);
            var newAnimatorReference = new PlayerAnimatorReference {
                Value = newCompanionGameObject.GetComponent<Animator>()
            };

            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            var position = SystemAPI.GetComponent<PositionComponent>(entity);

            var dest = new float3(position.X, 0, position.Y) * grid.CellSize;
            dest += new float3(grid.CellSize, 0, grid.CellSize) * 0.5f;
            dest.y = transform.ValueRO.Position.y;

            transform.ValueRW.Position = dest;

            ecb.AddComponent(entity, newAnimatorReference);
        }

        foreach (var (transform, animatorReference) in
                 SystemAPI.Query<LocalTransform, PlayerAnimatorReference>()) {
            animatorReference.Value.transform.position = transform.Position;
            animatorReference.Value.transform.rotation = transform.Rotation;
        }

        foreach (var animatorReference in SystemAPI.Query<PlayerAnimatorReference>().WithAll<MovingComponent>()) {
            Debug.Log(">> moving");
            animatorReference.Value.SetBool("IsMoving", true);
        }
        foreach (var animatorReference in SystemAPI.Query<PlayerAnimatorReference>().WithNone<MovingComponent>()) {
            Debug.Log(">> stopped");
            animatorReference.Value.SetBool("IsMoving", false);
        }

        foreach (var (animatorReference, entity) in
                 SystemAPI.Query<PlayerAnimatorReference>().WithNone<PlayerGameObjectPrefab, LocalTransform>()
                     .WithEntityAccess()) {
            Object.Destroy(animatorReference.Value.gameObject);
            ecb.RemoveComponent<PlayerAnimatorReference>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}