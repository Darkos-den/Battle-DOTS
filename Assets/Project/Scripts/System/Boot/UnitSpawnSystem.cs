using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Darkos {

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(GridInitializationSystem))]
    public partial struct UnitSpawnSystem : ISystem {

        void OnCreate(ref SystemState state) {
            state.RequireForUpdate<LevelComponent>();
        }

        void OnDestroy(ref SystemState state) { }

        void OnUpdate(ref SystemState state) {
            var grid = SystemAPI.GetSingleton<GridComponent>();
            var level = SystemAPI.ManagedAPI.GetSingleton<LevelComponent>();
            var prefabProvider = UnitPrefabProvider.Instance;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var unit in level.unitsP1) {
                var unitEntity = ecb.CreateEntity();
                ecb.AddComponent(unitEntity, new PositionComponent { 
                    X = unit.startPositionX, 
                    Y = unit.startPositionY 
                });

                var newCompanionGameObject = Object.Instantiate(prefabProvider.GetPrefab(unit.Type));
                var newAnimatorReference = new UnitAnimatorReference {
                    Value = newCompanionGameObject.GetComponent<Animator>()
                };

                var dest = new float3(unit.startPositionX, 0, unit.startPositionY) * grid.CellSize;
                dest += new float3(grid.CellSize, 0, grid.CellSize) * 0.5f;
                dest.y = 0;

                ecb.AddComponent(unitEntity, LocalTransform.FromPosition(dest).RotateY(90f));
                ecb.AddComponent(unitEntity, newAnimatorReference);
            }

            foreach (var unit in level.unitsP2) {
                var unitEntity = ecb.CreateEntity();
                ecb.AddComponent(unitEntity, new PositionComponent {
                    X = unit.startPositionX,
                    Y = unit.startPositionY
                });

                var newCompanionGameObject = Object.Instantiate(prefabProvider.GetPrefab(unit.Type));
                var newAnimatorReference = new UnitAnimatorReference {
                    Value = newCompanionGameObject.GetComponent<Animator>()
                };

                var dest = new float3(unit.startPositionX, 0, unit.startPositionY) * grid.CellSize;
                dest += new float3(grid.CellSize, 0, grid.CellSize) * 0.5f;
                dest.y = 0;

                ecb.AddComponent(unitEntity, LocalTransform.FromPosition(dest).RotateY(-90f));
                ecb.AddComponent(unitEntity, newAnimatorReference);
            }

            ecb.Playback(state.EntityManager);

            state.Enabled = false;
        }
    }

}