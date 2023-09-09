using Darkos;
using Unity.Burst;
using Unity.Entities;
using UnityEditor.Search;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct GameLoopSystem : ISystem {

    [BurstCompile]
    void OnCreate(ref SystemState state) {
        state.EntityManager.CreateSingleton<GameStateComponent>();
        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.Idle });
    }

    [BurstCompile]
    void OnDestroy(ref SystemState state) { }

    void OnUpdate(ref SystemState state) { 
        var gameStateComponent = SystemAPI.GetSingleton<GameStateComponent>();

        switch(gameStateComponent.Value) {
            case GameState.PlayerActivation: {
                    //mark current player as tired
                    MarkCurrentPlayerAsTired(ref state);

                    //try to activate next ready player
                    if (TryActivatePlayer(ref state)) {
                        //go to unit activation state
                        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.UnitActivation });
                    } else {
                        //all players tired, next round
                        SystemAPI.SetSingleton(new GameStateComponent { Value = GameState.Idle });
                    }

                    break;
                }
            case GameState.UnitActivation: { 
                    break; 
                }
        }
    }

    private void MarkCurrentPlayerAsTired(ref SystemState state) {
        var query = SystemAPI.QueryBuilder()
            .WithAll<PlayerComponent>()
            .WithAll<PlayerStateComponent>()
            .Build();
        query.SetSharedComponentFilter(new PlayerStateComponent { Value = PlayerState.Active });

        state.EntityManager.SetSharedComponent(query, new PlayerStateComponent { Value = PlayerState.Tired });
    }

    private bool TryActivatePlayer(ref SystemState state) {
        Entity? entity = null;

        foreach ((var _, var e) in 
            SystemAPI.Query<PlayerComponent>()
            .WithSharedComponentFilter(new PlayerStateComponent { Value = PlayerState.Ready })
            .WithEntityAccess()
        ) {
            entity = e;
            break;
        }

        if (entity != null) {
            state.EntityManager.SetSharedComponent((Entity)entity, new PlayerStateComponent { Value = PlayerState.Active });
            return true;
        } else {
            return false;
        }
    }
}