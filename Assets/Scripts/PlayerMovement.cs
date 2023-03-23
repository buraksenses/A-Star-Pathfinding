using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private List<PathNode> _path;
    private PathFinding _pathFinding;
    private bool _startMove;
    private int index = 0;
    private float _speed = 100f;
    private void Start()
    { 
        _pathFinding = Testing.Instance.PathFinding;

        transform.position = new Vector3(_pathFinding.GetGrid()._cellSize / 2f, _pathFinding.GetGrid()._cellSize / 2f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           CreatePath();
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        if (!_startMove) return;
        if (_path == null) return;

        var grid = _pathFinding.GetGrid();
        var playerPos = transform.position;
        
        playerPos = Vector3.MoveTowards(playerPos,
            new Vector3(_path[index].x * grid._cellSize + grid._cellSize / 2f, _path[index].y * grid._cellSize + grid._cellSize / 2f), _speed * Time.deltaTime);
        transform.position = playerPos;
        
        SetPlayerAttitude();
        
        for (int k=0; k<_path.Count - 1; k++) {
            Debug.DrawLine(new Vector3(_path[k].x, _path[k].y) * 10f + Vector3.one * 5f, new Vector3(_path[k+1].x, _path[k+1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
        }

        if (Mathf.Approximately(playerPos.x,_path[index].x * grid._cellSize + grid._cellSize / 2f) && Mathf.Approximately(playerPos.y,_path[index].y * grid._cellSize + grid._cellSize / 2f))
            index++;

        if (index >= _path.Count)
        {
            _startMove = false;
            index = 0;
            GameManager.PlayerAttitude = PlayerAttitude.Idle;
        }
    }

    private void CreatePath()
    {
        var playerPos = transform.position;
        var grid = _pathFinding.GetGrid();
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        grid.GetXY(mouseWorldPosition,out var x,out var y); 
        _path = _pathFinding.FindPath((int)(playerPos.x /grid._cellSize), (int)(playerPos.y /grid._cellSize), x, y);
        _startMove = true;
    }

    private void SetPlayerAttitude()
    {
        if (index >= _path.Count - 1) return;
        
        var currentX = _path[index].x;
        var currentY = _path[index].y;
            
        var nextX = _path[index + 1].x;
        var nextY = _path[index + 1].y;

        if (currentX < nextX)
            GameManager.PlayerAttitude = PlayerAttitude.Rightward;
        else if (currentX > nextX)
            GameManager.PlayerAttitude = PlayerAttitude.Leftward;
        else
        {
            if (currentY > nextY)
                GameManager.PlayerAttitude = PlayerAttitude.Downward;
            else if (currentY < nextY)
                GameManager.PlayerAttitude = PlayerAttitude.Upward;
        }
    }
}
