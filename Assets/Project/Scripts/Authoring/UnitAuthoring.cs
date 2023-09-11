using Unity.Entities;
using UnityEngine;

namespace Darkos {

    public class UnitAuthoring : MonoBehaviour {
        public int X;
        public int Y;

        class Baker : Baker<UnitAuthoring> {
            public override void Bake(UnitAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PositionComponent { 
                    X = authoring.X, 
                    Y = authoring.Y 
                });
            }
        }
    }

}