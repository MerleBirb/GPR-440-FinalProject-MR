using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private Pathfinding m_pathfinding;

    private void Start()
    {
        m_pathfinding = new Pathfinding(10, 10, transform.position);
    }

    private void Update()
    {
        CheckInput();
    }

    /// <summary>
    /// Checks input from the player.
    /// </summary>
    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0)) // pathfind to cell clicked
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            m_pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = m_pathfinding.FindPath(0, 0, x, y);

            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) + transform.position + Vector3.one * 0.5f, 
                        new Vector3(path[i + 1].x, path[i + 1].y) + transform.position + Vector3.one * 0.5f, 
                        Color.green, 100f);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            m_pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            m_pathfinding.GetNode(x, y).SetIsWalkable(!m_pathfinding.GetNode(x, y).IsWalkable);
        }
    }
}
