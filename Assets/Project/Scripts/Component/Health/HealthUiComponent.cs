using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Darkos {
    public class HealthUiComponent : IComponentData {
        public readonly Slider Slider;
        public readonly float3 Offset;

        public HealthUiComponent() { }

        public HealthUiComponent(Slider slider, Vector3 offset) {
            Slider = slider;
            Offset = new float3(offset.x, offset.y, offset.z);
        }
    }
}
