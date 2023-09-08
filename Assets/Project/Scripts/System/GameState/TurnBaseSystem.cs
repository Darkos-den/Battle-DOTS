using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Darkos {

    enum GameState {
        Idle, PlayerActivation, UnitSelection, ActionSelection, TargetSelection, ActionExecution
    }


    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class TurnBaseSystem : SystemBase {

        private SystemHandle _actionInputSystem;
        private SystemHandle _unitInputSystem;
        private GameState _gameState;

        protected override void OnCreate() {
            _actionInputSystem = World.GetOrCreateSystem<ActionSelectionSystem>();
            _unitInputSystem = World.GetOrCreateSystem<SelectionInputSystem>();
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

                        Entity? activeUnit = null;

                        //������� ������������ ����
                        foreach((var component, var ready, var active, var entity) in
                            SystemAPI.Query<UnitComponent, EnabledRefRO<ReadyToActionTag>, EnabledRefRW<ActiveTag>>()
                            .WithEntityAccess()
                            .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
                            .WithSharedComponentFilter(new PlayerIdComponent { Id = player.Id })
                        ) {
                            if (ready.ValueRO) {
                                active.ValueRW = true;
                                activeUnit = entity;
                                break;
                            }
                        }

                        if (activeUnit != null) {
                            //���� ���� �������� ����

                            var transform = SelectionIndicatorPooling.Instance.GetSelectionIndicator();

                            var unitPosition = SystemAPI.GetComponent<LocalTransform>((Entity)activeUnit).Position;
                            unitPosition.y = transform.position.y;

                            transform.position = unitPosition;
                            transform.gameObject.SetActive(true);

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

                        if (SystemAPI.TryGetSingletonEntity<TargetComponent>(out Entity entity)) {
                            //���� �������� �������
                            //�������� ����
                            _gameState = GameState.TargetSelection;
                        }

                        break;
                    }
                case GameState.TargetSelection: {
                        //��������� ����� ����
                        _unitInputSystem.Update(World.Unmanaged);

                        foreach (var item in SystemAPI.Query<TargetTag>()) {
                            _gameState = GameState.ActionExecution;
                            break;
                        }
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
