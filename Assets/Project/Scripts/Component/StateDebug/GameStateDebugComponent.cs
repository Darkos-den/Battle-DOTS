using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using StateMachine;

public class GameStateDebugComponent : IComponentData {

    public StateTree StateTree;

    public GameStateDebugComponent() { }
    public GameStateDebugComponent(StateTree stateTree) {
        StateTree = stateTree;
    }

}
