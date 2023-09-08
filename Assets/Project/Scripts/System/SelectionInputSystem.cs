using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Darkos {

    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [DisableAutoCreation]
    public partial class SelectionInputSystem : SystemBase {

        private PlayerInput _input;

        protected override void OnCreate() {
            Debug.Log(">> OnCreate");
            _input = new();
        }

        protected override void OnStartRunning() {
            Debug.Log(">> OnStartRunning");
            _input.General.Selection.performed += OnClick;
            _input.Enable();
        }

        protected override void OnStopRunning() {
            Debug.Log(">> OnStopRunning");
            _input.General.Selection.performed -= OnClick;
            _input.Dispose();
        }

        protected override void OnUpdate() {
            //throw new System.NotImplementedException();
        }

        private void OnClick(InputAction.CallbackContext context) {
            Debug.Log(">> click");
        }
    }
}
