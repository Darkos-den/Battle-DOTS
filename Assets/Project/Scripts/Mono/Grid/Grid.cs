using UnityEngine;

public class Grid {

    private int _width;
    private int _height;
    private float _cellSize;

    public int Width {
        get => _width;
    }
    public int Height {
        get => _height;
    }

    public Grid(int width, int height, float cellSize) {
        _width = width; 
        _height = height;
        _cellSize = cellSize;
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, 0, y) * _cellSize;
    }

    #region Public API

    public Vector3 MapToGridPosition(int x, int y) {
        return GetWorldPosition(x, y) + new Vector3(_cellSize, 0, _cellSize) * 0.5f;
    }
    
    public Vector3 MapToGridPositionBaseline(int x, int y) {
        return GetWorldPosition(x, y);
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt(worldPosition.x / _cellSize);
        y = Mathf.FloorToInt(worldPosition.z / _cellSize);
    }

    #endregion
}
