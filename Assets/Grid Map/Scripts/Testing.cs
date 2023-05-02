using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private Pathfinding m_pathfinding;
    [SerializeField] private Transform m_sprite;
    [SerializeField] private LayerMask m_unwalkableMask;
    [SerializeField] private Tilemap m_tilemap;
    [SerializeField] private FieldOfView m_fov;

    List<PathNode> m_path;
    int m_targetIndex = 0;

    private void Start()
    {
        m_pathfinding = new Pathfinding(20, 20, transform.position - (Vector3.one * 10));

        // set all unwalkable locations
        foreach (var pos in m_tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y);
            Vector3 place = m_tilemap.CellToWorld(localPlace);

            if (m_tilemap.HasTile(localPlace))
            {
                m_pathfinding.GetGrid().GetXY(place, out int x, out int y);
                m_pathfinding.GetNode(x, y).SetIsWalkable(!m_pathfinding.GetNode(x, y).IsWalkable);
            }
        }
    }

    private void Update()
    {
        CheckInput();

        m_fov.SetOrigin(m_sprite.position);
    }

    /// <summary>
    /// Checks input from the player.
    /// </summary>
    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0)) // pathfind to cell clicked
        {
            m_path = null;

            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            m_pathfinding.GetGrid().GetXY(m_sprite.position, out int startX, out int startY);
            m_pathfinding.GetGrid().GetXY(mouseWorldPosition, out int endX, out int endY);

            m_path = m_pathfinding.FindPath(startX, startY, endX, endY);

            if (m_path != null)
            {
                for (int i = 0; i < m_path.Count - 1; i++)
                {
                    Debug.DrawLine(m_pathfinding.GetGrid().GetWorldPosition(m_path[i].x, m_path[i].y) + Vector3.one * 0.5f,
                        m_pathfinding.GetGrid().GetWorldPosition(m_path[i + 1].x, m_path[i + 1].y) + Vector3.one * 0.5f,
                        Color.green, 100f);
                }
            }

            m_targetIndex = 0;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            m_pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            Debug.Log((m_pathfinding.GetGrid().GetWorldPosition(x, y).x) + ", " + (m_pathfinding.GetGrid().GetWorldPosition(x, y).y));
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = m_pathfinding.GetGrid().GetWorldPosition(m_path[0].x, m_path[0].y) + Vector3.one * 0.5f;
        currentWaypoint.z = 0;

        while (true)
        {
            if (m_sprite.position == currentWaypoint)
            {
                m_targetIndex++;
                if (m_targetIndex >= m_path.Count)
                {
                    yield break;
                }

                currentWaypoint = m_pathfinding.GetGrid().GetWorldPosition(m_path[m_targetIndex].x, m_path[m_targetIndex].y) + Vector3.one * 0.5f;
                currentWaypoint.z = 0;
            }

            m_sprite.position = Vector3.MoveTowards(m_sprite.position, currentWaypoint, 3 * Time.deltaTime);
            yield return null;
        }
    }
}
