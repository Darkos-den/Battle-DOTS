using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Unity.Entities.UniversalDelegates;

public class NodeView : UnityEditor.Experimental.GraphView.Node {

    public Node node;
    public Port input;
    public Port output;

    public NodeView(Node node) {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        //inputContainer.parent.parent.StretchToParentSize();
        inputContainer.parent.StretchToParentSize();
        inputContainer.StretchToParentSize();
        var color = Color.white;
        color.a = 0f;
        inputContainer.style.backgroundColor = color;
        inputContainer.parent.style.backgroundColor = color;
        //inputContainer.parent.parent.style.backgroundColor = color;

        RegisterCallback<GeometryChangedEvent>(GeometryChangedCallback);

        //inputContainer.style.height = 0;
        //outputContainer.style.height = 0;

        CreateInputPorts();
        CreateOutputPorts();
    }

    private void GeometryChangedCallback(GeometryChangedEvent evt) {
        UnregisterCallback<GeometryChangedEvent>(GeometryChangedCallback);

        var width = resolvedStyle.width;
        var height = resolvedStyle.height;

        var offset = width - inputContainer.resolvedStyle.width + 5;

        var center = new Vector2(0, -height/2);

        foreach (var item in inputContainer.Children()){
            item.transform.position = center;
        }

        center.x *= -1;

        foreach (var item in outputContainer.Children()) {
            item.transform.position = center;
        }
    }

    public override void SetPosition(Rect newPos) {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    private void CreateInputPorts() {
        if (node is ActionNode) {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        } else if (node is CompositeNode) {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        } else if (node is DecoratorNode) {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        } else if (node is RootNode) {
            
        }

        if (input != null) {
            var color = Color.white;
            color.a = 0;

            input.name = "sampleInput";
            input.ClearClassList();

            input.style.borderRightWidth = 0;
            input.style.borderLeftWidth = 0;
            input.style.borderTopWidth = 0;
            input.style.borderBottomWidth = 0;

            input.style.backgroundColor = color;
            foreach (var cap in input.Children()) {
                cap.style.backgroundColor = color;
            }

            input.portName = "";
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts() {
        if (node is ActionNode) {
            
        } else if (node is CompositeNode) {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        } else if (node is DecoratorNode) {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        } else if (node is RootNode) {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null) {
            var color = Color.white;
            color.a = 0;

            output.name = "sampleOutput";
            output.ClearClassList();

            output.style.borderRightWidth = 0;
            output.style.borderLeftWidth = 0;
            output.style.borderTopWidth = 0;
            output.style.borderBottomWidth = 0;

            output.style.backgroundColor = color;
            foreach (var cap in output.Children()) {
                cap.style.backgroundColor = color;
            }

            output.portName = "";
            outputContainer.Add(output);
        }
    }
}
