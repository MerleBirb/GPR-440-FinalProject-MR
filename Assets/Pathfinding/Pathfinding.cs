// Author: Merle Roji

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains logic for finding the best path. Uses A* pathfinding.
/// </summary>
/// Notes:
/// - Using Utilies from https://unitycodemonkey.com/utils.php
/// - Based on Tutorial: https://youtu.be/waEsGu--9P8
public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private GridData<PathNode> m_grid;
    private List<PathNode> m_openList;
    private HashSet<PathNode> m_closedList;

    public Pathfinding(int width, int height, Vector3 startPos)
    {
        m_grid = new GridData<PathNode>(width, height, 1, startPos, (GridData<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    /// <summary>
    /// A* Pathfinding algorithm.
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="endX"></param>
    /// <param name="endY"></param>
    /// <returns></returns>
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = m_grid.GetGridObject(startX, startY);
        PathNode endNode = m_grid.GetGridObject(endX, endY);

        m_openList = new List<PathNode>() { startNode };
        m_closedList = new HashSet<PathNode>();

        for (int x = 0; x < m_grid.GetWidth(); x++)
        {
            for (int y = 0; y < m_grid.GetHeight(); y++)
            {
                PathNode pathNode = m_grid.GetGridObject(x, y);
                pathNode.GCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.CameFromNode = null;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(m_openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(m_openList);
            if (currentNode == endNode)
            {
                // reached final node
                return CalculatePath(endNode);
            }

            m_openList.Remove(currentNode);
            m_closedList.Add(currentNode);

            foreach(PathNode neighborNode in GetNeighborList(currentNode))
            {
                if (m_closedList.Contains(neighborNode)) continue;
                if (!neighborNode.IsWalkable) // skip unwalkable nodes
                {
                    m_closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.GCost)
                {
                    neighborNode.CameFromNode = currentNode;
                    neighborNode.GCost = tentativeGCost;
                    neighborNode.HCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!m_openList.Contains(neighborNode))
                    {
                        m_openList.Add(neighborNode);
                    }
                }
            }
        }

        // out of nodes of the open list
        return null;
    }

    /// <summary>
    /// Constantly cycles through parent nodes until reaching one without a parent, which will be the start node.
    /// </summary>
    /// <param name="endNode"></param>
    /// <returns></returns>
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while(currentNode.CameFromNode != null)
        {
            path.Add(currentNode.CameFromNode);
            currentNode = currentNode.CameFromNode;
        }

        path.Reverse();
        return path;
    }

    /// <summary>
    /// Calculates the heuristic cost, or distance cost, between two nodes.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * remaining;
    }

    #region GETTERS - SETTERS

    /// <summary>
    /// Returns the grid this path uses.
    /// </summary>
    /// <returns></returns>
    public GridData<PathNode> GetGrid()
    {
        return m_grid;
    }

    /// <summary>
    /// Returns the list of neighbor nodes.
    /// </summary>
    /// <param name="currentNode"></param>
    /// <returns></returns>
    private List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighborList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) // check to left of node
        {
            neighborList.Add(GetNode(currentNode.x - 1, currentNode.y)); // Left
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1)); // Left Down
            if (currentNode.y + 1 < m_grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1)); // Left Up
        }
        if (currentNode.x + 1 < m_grid.GetWidth()) // check to right of node
        {
            neighborList.Add(GetNode(currentNode.x + 1, currentNode.y)); // Right
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1)); // Right Down
            if (currentNode.y + 1 < m_grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1)); // Right Up
        }

        if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1)); // Down
        if (currentNode.y + 1 < m_grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1)); // Up

        return neighborList;
    }

    /// <summary>
    /// Returns a node from an x and y position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public PathNode GetNode(int x, int y)
    {
        return m_grid.GetGridObject(x, y);
    }

    /// <summary>
    /// Returns the node with the lowest F Cost.
    /// </summary>
    /// <param name="pathNodeList"></param>
    /// <returns></returns>
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].FCost < lowestFCostNode.FCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }

    #endregion
}
