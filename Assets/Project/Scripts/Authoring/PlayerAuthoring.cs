using Unity.Entities;
using UnityEngine;

namespace Darkos {
    public class PlayerAuthoring : MonoBehaviour {

        public int Id;
        public int LayerIndex;

        class Baker : Baker<PlayerAuthoring> {
            public override void Bake(PlayerAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerComponent { 
                    Id = authoring.Id, 
                    PlayerLayerIndex = authoring.LayerIndex 
                });
                AddComponent<ReadyToActionTag>(entity);
                AddComponent<ActiveTag>(entity);
                SetComponentEnabled<ActiveTag>(entity, false);
            }
        }
    }
}
