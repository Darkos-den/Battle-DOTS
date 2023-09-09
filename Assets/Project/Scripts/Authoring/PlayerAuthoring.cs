using Unity.Entities;
using UnityEngine;

namespace Darkos {
    public class PlayerAuthoring : MonoBehaviour {

        public int Id;
        public int LayerIndex;

        class Baker : Baker<PlayerAuthoring> {
            public override void Bake(PlayerAuthoring authoring) {

                //var newEntityArchetype = EntityManager.CreateArchetype(typeof(Foo), typeof(Bar), typeof(Baz));
                //var entity = EntityManager.CreateEntity(newEntityArchetype);

                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerComponent { 
                    Id = authoring.Id, 
                    PlayerLayerIndex = authoring.LayerIndex 
                });
                AddComponent<ReadyToActionTag>(entity);
                AddComponent<ActiveTag>(entity);
                SetComponentEnabled<ActiveTag>(entity, false);
                AddSharedComponent(entity, new PlayerStateComponent { Value = PlayerState.Ready });
            }
        }
    }
}
