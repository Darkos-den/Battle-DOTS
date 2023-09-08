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
                        //�������� ���� ������� � ������ ��� ������� � ���������
                        ResetActionTags();
                        //�������� ��������� ������
                        _gameState = GameState.PlayerActivation;
                        break;
                    }
                case GameState.UnitSelection: {
                        
                        //�������� �������� ������
                        var playerEntity = SystemAPI.QueryBuilder()
                            .WithAll<PlayerComponent>()
                            .WithAll<ActiveTag>()
                            .Build()
                            .ToEntityArray(Allocator.Temp)
                            .First();
                        var player = SystemAPI.GetComponent<PlayerComponent>(playerEntity);

                        UnitComponent? activeUnit = null;

                        //������� ������������ ����
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
                            //���� ���� �������� ����
                            //�������� ��������

                            _gameState = GameState.ActionSelection;
                        } else {
                            //���� ����� ��������� ��� ��������� �����������
                            //������ ������

                            _gameState = GameState.PlayerActivation;
                        }

                        break;
                    }
                case GameState.ActionSelection: {
                        //��������� ����� ��������
                        _actionInputSystem.Update(World.Unmanaged);

                        Entity entity;
                        if (SystemAPI.TryGetSingletonEntity<TargetComponent>(out entity)) {
                            //���� �������� �������
                            //�������� ����
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

                        //��������� ������� �������
                        if (query.CalculateEntityCount() == 0) {
                            //���� ����� ��� - ��������� �����
                            _gameState = GameState.Idle;
                        } else {
                            //����� ����� ������� ����������� ������
                            var array = query.ToEntityArray(Allocator.Temp);
                            array.Sort();
                            var entity = array.Last();

                            //�������� ��� ��� ���������
                            SystemAPI.SetComponentEnabled<ActiveTag>(entity, true);

                            //�������� ����
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
