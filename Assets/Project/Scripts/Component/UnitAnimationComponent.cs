using Unity.Entities;
using UnityEngine;

namespace Darkos {

    public class UnitGameObjectPrefab : IComponentData {
        public GameObject Value;
    }

    public class UnitAnimatorReference : ICleanupComponentData {
        public Animator Value;
    }

}