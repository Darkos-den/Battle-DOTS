using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerGameObjectPrefab : IComponentData {
    public GameObject Value;
}

public class PlayerAnimatorReference : ICleanupComponentData {
    public Animator Value;
}
