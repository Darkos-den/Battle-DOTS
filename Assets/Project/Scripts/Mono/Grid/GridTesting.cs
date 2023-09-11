using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTesting : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    private Grid grid;

    void Start() {
        grid = new Grid(width, height, 1);

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                Debug.DrawLine(grid.MapToGridPositionBaseline(x, y), grid.MapToGridPositionBaseline(x, y + 1), Color.white, 100f);
                Debug.DrawLine(grid.MapToGridPositionBaseline(x, y), grid.MapToGridPositionBaseline(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(grid.MapToGridPositionBaseline(0, height), grid.MapToGridPositionBaseline(width, height), Color.white, 100f);
        Debug.DrawLine(grid.MapToGridPositionBaseline(width, 0), grid.MapToGridPositionBaseline(width, height), Color.white, 100f);
    }
}
