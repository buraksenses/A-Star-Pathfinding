using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PathNode
{
   private Grid<PathNode> _grid;
   public int x;
   public int y;
   public SpriteRenderer renderer;

   public int gCost;
   public int hCost;
   public int fCost;

   public PathNode parentNode;
   public List<PathNode> neighbours;
   public bool isWalkable;

   public PathNode(Grid<PathNode> grid,int x,int y)
   {
      this._grid = grid;
      this.x = x;
      this.y = y;

      isWalkable = true;
   }

   public void CalculateFCost()
   {
      fCost = gCost + hCost;
   }

   public void SetNodeColor(SpriteRenderer spriteRenderer,Grid<PathNode> grid, Color color)
   {
      renderer = Object.Instantiate(spriteRenderer, new Vector3(x * grid._cellSize + grid._cellSize / 2f,y * grid._cellSize + grid._cellSize / 2f),quaternion.identity);
      renderer.color = color;
      renderer.transform.localScale *= grid._cellSize;
   }

   public override string ToString()
   {
      return x + "," + y;
   }
}
