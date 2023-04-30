// Author: Merle Roji

using UnityEngine;

/// <summary>
/// Contains data for each node on a path.
/// </summary>
/// Notes:
/// - Using Utilies from https://unitycodemonkey.com/utils.php
/// - Based on Tutorial: https://youtu.be/waEsGu--9P8
public class PathNode
{
    private GridData<PathNode> m_grid;
    internal int x;
    internal int y;

    internal int GCost; // walking cost from the start node
    internal int HCost; // heuristic cost to reach end node
    internal int FCost; // g + h

    internal bool IsWalkable;

    internal PathNode CameFromNode;

    public PathNode(GridData<PathNode> grid, int x, int y)
    {
        m_grid = grid;
        this.x = x;
        this.y = y;

        IsWalkable = true;
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

    #region GETTERS - SETTERS

    public void SetIsWalkable(bool isWalkable)
    {
        IsWalkable = isWalkable;
        m_grid.TriggerGridObjectChanged(x, y);
    }

    #endregion

    #region OVERRIDES

    public override string ToString()
    {
        return x + ", " + y;
    }

    #endregion
}
