using Darkos;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEditor.Search;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(SpawnUnitSystem))]
public partial struct GameLoopSystem : ISystem {

    [BurstCompile]
    void OnCreate(ref SystemState state) {
        state.RequireForUpdate<PlayerComponent>();
        state.EntityManager.CreateSingleton<GameStateComponent>();
        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.PlayerActivation });
    }

    [BurstCompile]
    void OnDestroy(ref SystemState state) { }

    
    void OnUpdate(ref SystemState state) { 
        var gameStateComponent = SystemAPI.GetSingleton<GameStateComponent>();

        switch(gameStateComponent.Value) {
            case GameState.Idle: {
                    ResetAllUnitsState(ref state);
                    ResetAllPlayersState(ref state);

                    SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.PlayerActivation });
                    break;
                }
            case GameState.PlayerActivation: {
                    if (TryActivatePlayer(ref state)) {
                        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.UnitActivation });
                    } else {
                        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.Idle });
                    }

                    break;
                }
            case GameState.UnitActivation: { 
                    if (TryActivateUnit(ref state)) {
                        state.EntityManager.CreateSingleton<AwaitActionTag>();
                        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.UnitAction });
                    } else {
                        MarkCurrentPlayerAsTired(ref state);
                        SystemAPI.SetSingleton(new ActivePlayer { Value = Entity.Null });
                        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.PlayerActivation });
                    }

                    break; 
                }
            case GameState.UnitAction: {
                    if (SystemAPI.GetSingleton<ActiveUnit>().Value == Entity.Null) {
                        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.UnitActivation });
                    }
                    break;
                }
        }
    }

    private void MarkCurrentPlayerAsTired(ref SystemState state) {
        var player = SystemAPI.GetSingleton<ActivePlayer>().Value;

        state.EntityManager.SetComponentData(player, new PlayerStateComponent { Value = PlayerState.Tired });
    }

    private bool TryActivatePlayer(ref SystemState state) {
        Entity entity = Entity.Null;

        foreach ((var pState, var e) in 
            SystemAPI.Query<RefRW<PlayerStateComponent>>()
            .WithEntityAccess()
        ) {
            if (pState.ValueRO.Value == PlayerState.Ready) {
                pState.ValueRW.Value = PlayerState.Active;
                entity = e;
                break;
            }
        }

        if (entity != Entity.Null) {
            SystemAPI.SetSingleton(new ActivePlayer { Value = entity });
            return true;
        } else {
            return false;
        }
    }

    private bool TryActivateUnit(ref SystemState state) {
        var playerId = SystemAPI.GetComponent<PlayerComponent>(
            SystemAPI.GetSingleton<ActivePlayer>().Value
        ).Id;

        Entity entity = Entity.Null;
        foreach ((var pId, var unitState, var e) in
            SystemAPI.Query<PlayerIdComponent, RefRW<UnitStateComponent>>()
            .WithEntityAccess()
        ) {
            if (pId.Id == playerId && unitState.ValueRO.Value == UnitState.Ready) {
                unitState.ValueRW.Value = UnitState.Active;
                entity = e;
                break;
            }
        }

        if (entity != Entity.Null) {
            SystemAPI.SetSingleton(new ActiveUnit { Value = entity });
            return true;
        } else {
            return false;
        }
    }

    private void ResetAllUnitsState(ref SystemState state) {
        foreach (var uState in SystemAPI.Query<RefRW<UnitStateComponent>>()) {
            uState.ValueRW.Value = UnitState.Ready;
        }
    }

    private void ResetAllPlayersState(ref SystemState state) {
        foreach (var uState in SystemAPI.Query<RefRW<PlayerStateComponent>>()) {
            uState.ValueRW.Value = PlayerState.Ready;
        }
    }
}
