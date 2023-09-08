using Darkos;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial class UpdateSliderUiSystem : SystemBase {

    protected override void OnUpdate() {
        foreach ((var ui, var transform, var health) in SystemAPI.Query<HealthUiComponent, LocalToWorld, HealthComponent>()) {
            ui.Slider.transform.position = transform.Position + ui.Offset;
            ui.Slider.value = health.Value / 100f;
        }
    }
}
