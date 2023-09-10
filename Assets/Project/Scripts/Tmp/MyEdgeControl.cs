using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class MyEdgeControl : EdgeControl {

    public MyEdgeControl() {
        base.generateVisualContent = new Action<MeshGenerationContext>(DrawEdge);
    }


    private void DrawEdge(MeshGenerationContext mgc) {
        var paint2D = mgc.painter2D;

        paint2D.lineWidth = 1.0f;
        paint2D.strokeColor = Color.white;
        paint2D.lineJoin = LineJoin.Round;
        paint2D.lineCap = LineCap.Round;

        paint2D.BeginPath();
        paint2D.MoveTo(from);
        paint2D.LineTo(to);
        paint2D.Stroke();
    }

    public override void UpdateLayout() {
        
    }
}