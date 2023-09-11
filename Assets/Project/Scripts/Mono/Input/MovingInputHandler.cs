using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using Darkos;
using Unity.Physics;

public class MovingInputHandler : MonoBehaviour {

    [SerializeField]
    private InputAction Input;
    [SerializeField]
    private Camera Camera;

    private Entity entity;
    private World world;

    private void OnEnable() {
        Input.started += MouseClicked;
        Input.Enable();

        world = World.DefaultGameObjectInjectionWorld;
        if (world.IsCreated && !world.EntityManager.Exists(entity)) {
            entity = world.EntityManager.CreateEntity();
            world.EntityManager.AddBuffer<MoveInputComponent>(entity);
        }
    }

    private void OnDisable() {
        Input.started -= MouseClicked;
        Input.Disable();

        if (world.IsCreated && world.EntityManager.Exists(entity)) {
            world.EntityManager.DestroyEntity(entity);
        }
    }

    private void MouseClicked(InputAction.CallbackContext context) {
        Vector2 pos = context.ReadValue<Vector2>();
        var ray = Camera.ScreenPointToRay(pos);

        var layer = 1 << 8;
        layer |= 1 << 7;
        layer |= 1 << 6;

        var input = new RaycastInput {
            Start = ray.origin,
            Filter = new CollisionFilter {
                BelongsTo = 1 << 0,
                CollidesWith = (uint) layer,
            },
            End = ray.GetPoint(Camera.farClipPlane)
        };

        world.EntityManager.GetBuffer<MoveInputComponent>(entity)
            .Add(new MoveInputComponent { Value = input });
    }
}
