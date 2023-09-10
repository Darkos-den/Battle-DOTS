using Unity.Entities;
using UnityEngine;

namespace Darkos {
    public class UnitAuthoring : MonoBehaviour {

        [SerializeField] private float _healthMax;

        public int PlayerId;

        class Baker : Baker<UnitAuthoring> {
            public override void Bake(UnitAuthoring authoring) {

                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HealthComponent {
                    MaxValue = authoring._healthMax,
                    Value = authoring._healthMax
                });
                AddComponent<RawUnitTag>(entity);
                AddComponent<UnitComponent>(entity);
                AddComponent<ReadyToActionTag>(entity);
                AddComponent<ActiveTag>(entity);
                SetComponentEnabled<ActiveTag>(entity, false);
                AddComponent(entity, new PlayerIdComponent { Id = authoring.PlayerId });
                AddComponent<TargetTag>(entity);
                SetComponentEnabled<TargetTag>(entity, false);
                AddComponent(entity, new UnitStateComponent { Value = UnitState.Ready });
            }
        }
    }
}
