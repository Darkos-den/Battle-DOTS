using Darkos;
using System;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Darkos {

    public partial class SelectionInputSystem : SystemBase {

        private PlayerInput _input;
        private Camera _camera;

        private Vector2? _lastPos = null;

        protected override void OnCreate() {
            RequireForUpdate<TargetComponent>();

            _input = new();
            
            _input.General.Selection.performed += OnClick;
            _input.Enable();
        }

        protected override void OnDestroy() {
            _input.General.Selection.performed -= OnClick;
            _input.Dispose();
        }

        protected override void OnStartRunning() {
            _camera = Camera.main;
        }

        protected override void OnUpdate() {
            if (_lastPos == null) {
                return;
            }

            var collision = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            var screenPos = _lastPos;
            if (_camera == null) {
                _camera = Camera.main;
            }
            var ray = _camera.ScreenPointToRay((Vector2) screenPos);

            var target = SystemAPI.GetSingleton<TargetComponent>();

            uint layer = 0;

            var currentPlayerId = SystemAPI.GetComponent<PlayerComponent>(
                SystemAPI.GetSingleton<ActivePlayer>().Value
            ).Id;

            switch (target.Type) {
                case TargetType.Enemy: {
                        foreach (var item in SystemAPI.Query<PlayerComponent>()) {
                            if (item.Id != currentPlayerId) {
                                layer |= (uint)(1 << item.PlayerLayerIndex);
                            }
                        }
                        break;
                    }
                case TargetType.Friend: {
                        foreach (var item in SystemAPI.Query<PlayerComponent>()) {
                            if (item.Id == currentPlayerId) {
                                layer |= (uint)(1 << item.PlayerLayerIndex);
                            }
                        }
                        break;
                    }
            }

            var raycastInput = new RaycastInput {
                Start = ray.origin,
                Filter = new CollisionFilter {
                    BelongsTo = 1 << 0,
                    CollidesWith = layer,
                },
                End = ray.GetPoint(_camera.farClipPlane),
            };


            if (collision.CastRay(raycastInput, out var hit)) {
                var entity = collision.Bodies[hit.RigidBodyIndex].Entity;

                var health = SystemAPI.GetComponentRW<HealthComponent>(entity);
                health.ValueRW.Value = health.ValueRO.Value + target.HealtEffect;

                Entity unit = SystemAPI.GetSingleton<ActiveUnit>().Value;
                EntityManager.SetComponentData(unit, new UnitStateComponent { Value = UnitState.Tired });
                SystemAPI.SetSingleton(new ActiveUnit { Value = Entity.Null });

                EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<TargetComponent>());
            }
            _lastPos = null;
        }

        private void OnClick(InputAction.CallbackContext context) {
            var screenPos = context.ReadValue<Vector2>();
            _lastPos = screenPos;
        }

        public UnitComponent? GetActiveUnit(ref SystemState state) {
            if (SystemAPI.HasSingleton<UnitComponent>()) {
                return SystemAPI.GetSingleton<UnitComponent>();
            } else {
                return null;
            }
        }
    }

}
