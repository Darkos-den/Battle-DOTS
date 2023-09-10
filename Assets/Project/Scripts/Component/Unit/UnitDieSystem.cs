using Darkos;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(SelectionInputSystem))]
public partial class UnitDieSystem : SystemBase {
    protected override void OnUpdate() {
        var buffer = new EntityCommandBuffer(Allocator.Temp);
        foreach ((var health, var entity) in SystemAPI.Query<HealthComponent>().WithEntityAccess()) {
            if(health.Value <= 0) {
                buffer.DestroyEntity(entity);
                var slider = SystemAPI.ManagedAPI.GetComponent<HealthUiComponent>(entity);
                HealthBarPooling.Instance.ReturnSlider(slider.Slider);
            }
        }

        buffer.Playback(EntityManager);
    }
}
