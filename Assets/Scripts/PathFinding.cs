using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    private Grid<PathNode> _grid;
    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public PathFinding(int width, int height)
    {
        _grid = new Grid<PathNode>(width, height, 10, Vector3.zero, (g, x, y) => new PathNode(g, x, y));
        
        for (var i = 0; i < _grid.GetWidth(); i++)
        {
            for (var j = 0; j < _grid.GetHeight(); j++)
            {
                _grid.GetGridObject(i, j).neighbours = GetNeighbourList(_grid.GetGridObject(i, j));
            }
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = _grid.GetGridObject(startX, startY);
        PathNode endNode = _grid.GetGridObject(endX, endY);
        
        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }

        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        for (int i = 0; i < _grid.GetWidth(); i++)
        {
            for (int j = 0; j < _grid.GetHeight(); j++)
            {
                PathNode pathNode = _grid.GetGridObject(i,j);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.parentNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (_openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(_openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);
            
            foreach (var neighbourNode in currentNode.neighbours)
            {
                if(_closedList.Contains(neighbourNode))continue;
                if (!neighbourNode.isWalkable)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                int tempGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tempGCost < neighbourNode.gCost)
                {
                    neighbourNode.parentNode = currentNode;
                    neighbourNode.gCost = tempGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                    
                    if(!_openList.Contains(neighbourNode))
                        _openList.Add(neighbourNode);
                }
            }
        }
        
        //Out of nodes on the _openList
        return null;
    }

    private static int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private static PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostNode = pathNodes[0];

        foreach (var t in pathNodes.Where(t => t.fCost < lowestFCostNode.fCost))
        {
            lowestFCostNode = t;
        }
        return lowestFCostNode;
    }

    private static List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode> { endNode };
        PathNode currentNode = endNode;
        while (currentNode.parentNode != null)
        {
            path.Add(currentNode.parentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();
        if (currentNode.x - 1 >= 0) {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < _grid.GetWidth()) {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }
    
    public PathNode GetNode(int x, int y) {
        return _grid.GetGridObject(x, y);
    }

    public Grid<PathNode> GetGrid()
    {
        return _grid;
    }
}
