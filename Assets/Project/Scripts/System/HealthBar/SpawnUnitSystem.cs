using Darkos;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial class SpawnUnitSystem : SystemBase {
    protected override void OnUpdate() {
        var buffer = new EntityCommandBuffer(Allocator.Temp);

        foreach ((var _, var entity) in SystemAPI.Query<RawUnitTag>().WithEntityAccess()) {
            buffer.RemoveComponent<RawUnitTag>(entity);

            var slider = HealthBarPooling.Instance.GetNextSlider();
            buffer.AddComponent(entity, new HealthUiComponent(slider, new Vector3 { y = 1.25f }));
        }

        buffer.Playback(EntityManager);
        buffer.Dispose();
    }
}
