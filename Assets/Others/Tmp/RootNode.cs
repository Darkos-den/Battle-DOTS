using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node {

    public Node child;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override NodeState OnUpdate() {
        return child.Update();
    }
}
