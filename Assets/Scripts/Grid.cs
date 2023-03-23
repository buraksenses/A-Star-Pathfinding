using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class Grid<TGridObject>
{
    private int _width;
    private int _height;
    
    private TGridObject[,] _gridArray;
    private TextMesh[,] _debugTextArray;
    private Vector3 _originPosition;
    
    public float _cellSize { get; private set; }

    public Grid(int width, int height, float cellSize, Vector3 originPosition,
        Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this._height = height;
        this._width = width;
        this._cellSize = cellSize;
        this._originPosition = originPosition;

        _gridArray = new TGridObject[width, height];
        _debugTextArray = new TextMesh[width, height];

        for (var i = 0; i < _gridArray.GetLength(0); i++)
        {
            for (var j = 0; j < _gridArray.GetLength(1); j++)
            {
                _gridArray[i, j] = createGridObject(this, i, j);
            }
        }

        for (var i = 0; i < _gridArray.GetLength(0); i++)
        {
            for (var j = 0; j < _gridArray.GetLength(1); j++)
            {
                _debugTextArray[i, j] =
                    UtilsClass.CreateWorldText(_gridArray[i, j]?.ToString(), null,
                        GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white,
                        TextAnchor.MiddleCenter);

                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPos - _originPosition).y / _cellSize);
    }

    private void SetGridObjectValue(int x, int y, TGridObject value)
    {
        if (x >= _width || x < 0 || y >= _height || y < 0) return;
        _gridArray[x, y] = value;
        _debugTextArray[x, y].text = value.ToString();
    }

    public void SetGridObjectValue(Vector3 worldPos, TGridObject value)
    {
        GetXY(worldPos, out var x, out var y);
        SetGridObjectValue(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            return _gridArray[x, y];
        } else {
            return default(TGridObject);
        }
    }

    public int GetHeight() => _height;
    public int GetWidth() => _width;
}


