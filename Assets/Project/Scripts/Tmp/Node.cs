using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    
    public enum NodeState {
        Idle, Running, Success
    }

    public NodeState State = NodeState.Running;
    public bool Started = false;
    public string guid;
    public Vector2 position;

    public NodeState Update() {
        if (!Started) {
            Started = true;
            OnStart();
        }

        State = OnUpdate();

        if (State == NodeState.Success) {
            OnStop();
            Started = false;
        }

        return State;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract NodeState OnUpdate();
}
