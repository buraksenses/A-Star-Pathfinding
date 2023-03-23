using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
public class Testing : MonoBehaviour
{
    public static Testing Instance;
    public PathFinding PathFinding;
    public SpriteRenderer square;

    [SerializeField] private int width;
    [SerializeField] private int height;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       Init();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
                
        var grid = PathFinding.GetGrid();
        grid.GetXY(mouseWorldPosition,out var x,out var y);
        grid.GetGridObject(x, y).isWalkable = !grid.GetGridObject(x, y).isWalkable;
        grid.GetGridObject(x,y).SetNodeColor(square,grid,grid.GetGridObject(x,y).isWalkable ? Color.gray : Color.red);
    }

    private void Init()
    {
        PathFinding = new PathFinding(width, height);
        var grid = PathFinding.GetGrid();

        for (var i = 0; i < grid.GetWidth(); i++)
        {
            for (var j = 0; j < grid.GetHeight(); j++)
            {
                grid.GetGridObject(i,j).SetNodeColor(square,grid,Color.gray);
            }
        }
    }
}