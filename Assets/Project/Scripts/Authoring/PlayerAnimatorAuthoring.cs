using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerAnimatorAuthoring : MonoBehaviour {
    public GameObject PlayerGameObjectPrefab;

    public class PlayerGameObjectPrefabBaker : Baker<PlayerAnimatorAuthoring> {
        public override void Bake(PlayerAnimatorAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new PlayerGameObjectPrefab { Value = authoring.PlayerGameObjectPrefab });
        }
    }
}