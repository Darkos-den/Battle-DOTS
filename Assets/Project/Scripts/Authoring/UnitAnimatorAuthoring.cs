using Unity.Entities;
using UnityEngine;
using Darkos;

public class UnitAnimatorAuthoring : MonoBehaviour {
    public GameObject PlayerGameObjectPrefab;

    class Baker : Baker<UnitAnimatorAuthoring> {
        public override void Bake(UnitAnimatorAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new UnitGameObjectPrefab { Value = authoring.PlayerGameObjectPrefab });
        }
    }
}