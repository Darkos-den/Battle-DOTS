using Darkos;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using static UnityEditor.Progress;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class UpdateSliderUiSystem : SystemBase {

    protected override void OnUpdate() {
        foreach ((var ui, var transform) in SystemAPI.Query<HealthUiComponent, LocalToWorld>()) {
            ui.Slider.transform.position = transform.Position + ui.Offset;
        }
    }
}
