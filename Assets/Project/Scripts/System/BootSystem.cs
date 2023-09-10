using Darkos;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct BootSystem : ISystem {

    void OnCreate(ref SystemState state) { }

    void OnDestroy(ref SystemState state) { }

    void OnUpdate(ref SystemState state) {
        state.EntityManager.CreateSingleton<ActivePlayer>();
        state.EntityManager.CreateSingleton<ActiveUnit>();

        SystemAPI.SetSingleton(new ActivePlayer { Value = Entity.Null });
        SystemAPI.SetSingleton(new ActiveUnit { Value = Entity.Null });

        state.Enabled = false;
    }
}
