using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Darkos {

    enum GameState {
        Idle, PlayerActivation, UnitSelection, ActionSelection, TargetSelection, ActionExecution
    }


    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class TurnBaseSystem : SystemBase {

        private SystemHandle _actionInputSystem;
        private GameState _gameState;

        protected override void OnCreate() {
            _actionInputSystem = World.GetOrCreateSystem<UnitSelectionSystem>();
            _gameState = GameState.Idle;
        }

        protected override void OnUpdate() {

            Debug.Log(">> GameState: " + _gameState);
            switch(_gameState) {
                case GameState.Idle: {
                        //пометить всех игроков и юнитов как готовых к действиям
                        ResetActionTags();
                        //выбираем активного игрока
                        _gameState = GameState.PlayerActivation;
                        break;
                    }
                case GameState.UnitSelection: {
                        
                        //получаем текущего игрока
                        var playerEntity = SystemAPI.QueryBuilder()
                            .WithAll<PlayerComponent>()
                            .WithAll<ActiveTag>()
                            .Build()
                            .ToEntityArray(Allocator.Temp)
                            .First();
                        var player = SystemAPI.GetComponent<PlayerComponent>(playerEntity);

                        UnitComponent? activeUnit = null;

                        //пробуем активирвоать юнит
                        foreach((var component, var ready, var active) in
                            SystemAPI.Query<UnitComponent, EnabledRefRO<ReadyToActionTag>, EnabledRefRW<ActiveTag>>()
                            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                            .WithSharedComponentFilter(new PlayerIdComponent { Id = player.Id })
                        ) {
                            if (ready.ValueRO) {
                                active.ValueRW = true;
                                activeUnit = component;
                                break;
                            }
                        }

                        if (activeUnit != null) {
                            //если есть активный юнит
                            //выбираем действие

                            _gameState = GameState.ActionSelection;
                        } else {
                            //если юниты доступные для активации закончились
                            //меняем игрока

                            _gameState = GameState.PlayerActivation;
                        }

                        break;
                    }
                case GameState.ActionSelection: {
                        //проверяем выбор действия
                        _actionInputSystem.Update(World.Unmanaged);

                        Entity entity;
                        if (SystemAPI.TryGetSingletonEntity<TargetComponent>(out entity)) {
                            //если действие выбрано
                            //выбираем цель
                            _gameState = GameState.TargetSelection;
                        }
                        
                        break;
                    }
                case GameState.TargetSelection: {
                        //todo: TBD
                        break;
                    }
                case GameState.ActionExecution: {
                        //todo: TBD
                        break;
                    }
                case GameState.PlayerActivation: {
                        
                        var query = SystemAPI.QueryBuilder()
                            .WithAll<PlayerComponent>()
                            .WithAll<ReadyToActionTag>()
                            .Build();

                        //проверить готовых игроков
                        if (query.CalculateEntityCount() == 0) {
                            //если таких нет - следующий раунд
                            _gameState = GameState.Idle;
                        } else {
                            //иначе берем первого подходящего игрока
                            var array = query.ToEntityArray(Allocator.Temp);
                            array.Sort();
                            var entity = array.Last();

                            //помечаем его как активного
                            SystemAPI.SetComponentEnabled<ActiveTag>(entity, true);

                            //выбираем юнит
                            _gameState = GameState.UnitSelection;
                        }

                        break;
                    }
            }
        }

        private void ResetActionTags() {
            foreach ((var _, var tag) in 
                SystemAPI.Query<PlayerComponent, EnabledRefRW<ReadyToActionTag>>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
            ) {
                tag.ValueRW = true;
            }
            foreach ((var _, var tag) in 
                SystemAPI.Query<UnitComponent, EnabledRefRW<ReadyToActionTag>>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
            ) {
                tag.ValueRW = true;
            }
        }
    }
}
