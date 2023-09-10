using StateMachine;
using Unity.Entities;
using UnityEngine;

namespace Darkos {

    public class GameStateDebugAuthoring : MonoBehaviour {
        public StateTree gameStateTree;

        class Baker : Baker<GameStateDebugAuthoring> {
            public override void Bake(GameStateDebugAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponentObject(entity, new GameStateDebugComponent(authoring.gameStateTree));
            }
        }
    }
}
