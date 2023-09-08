using System;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Darkos {

    [DisableAutoCreation]
    public partial class SelectionInputSystem : SystemBase {

        private PlayerInput _input;
        private Camera _camera;

        protected override void OnCreate() {
            _input = new();
        }

        protected override void OnStartRunning() {
            _camera = Camera.main;
            _input.General.Selection.performed += OnClick;
            _input.Enable();
        }

        protected override void OnStopRunning() {
            _input.General.Selection.performed -= OnClick;
            _input.Dispose();
        }

        protected override void OnUpdate() {
        }

        private void OnClick(InputAction.CallbackContext context) {
            var collision = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            var screenPos = context.ReadValue<Vector2>();
            var ray = _camera.ScreenPointToRay(screenPos);

            var target = SystemAPI.GetSingleton<TargetComponent>();

            uint layer = 0;

            switch (target.Type) {
                case TargetType.Enemy: {
                        foreach (var item in SystemAPI.Query<PlayerComponent>().WithDisabled<ActiveTag>()) {
                            layer |= (uint) (1 << item.PlayerLayerIndex);
                        }
                        break;
                    }
                case TargetType.Friend: {
                        foreach (var item in SystemAPI.Query<PlayerComponent>().WithAll<ActiveTag>()) {
                            layer |= (uint) (1 << item.PlayerLayerIndex);
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

                SystemAPI.SetComponentEnabled<TargetTag>(entity, true);
            }
        }
    }
}
