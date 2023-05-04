using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Tests the lighting and controls.
/// </summary>
/// Notes:
/// - Based on Tutorial: https://youtu.be/waEsGu--9P8
public class Testing : MonoBehaviour
{
    private Pathfinding m_pathfinding;
    [SerializeField] private Transform character;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField, Min(0.0f)] private float moveSpeed;

    private List<PathNode> m_path;
    private int m_targetIndex = 0;

    private Vector3 m_currentWaypoint;
    private Camera m_mainCam;

    private void Start()
    {
        m_pathfinding = new Pathfinding(gridSize.x, gridSize.y, transform.position);
        m_currentWaypoint = character.position;
        m_mainCam = Camera.main;

        // set all unwalkable locations
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y);
            Vector3 place = tilemap.CellToWorld(localPlace);

            if (tilemap.HasTile(localPlace))
            {
                m_pathfinding.GetGrid().GetXY(place, out int x, out int y);
                m_pathfinding.GetNode(x, y).SetIsWalkable(!m_pathfinding.GetNode(x, y).IsWalkable);
            }
        }
    }

    private void Update()
    {
        CheckInput();

        fieldOfView.SetOrigin(character.position);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Checks input from the player.
    /// </summary>
    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0)) // pathfind to cell clicked
        {
            m_path = null;

            Vector3 mouseWorldPosition = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            m_pathfinding.GetGrid().GetXY(character.position, out int startX, out int startY);
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
            Vector3 mouseWorldPosition = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            m_pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            Debug.Log((m_pathfinding.GetGrid().GetWorldPosition(x, y).x) + ", " + (m_pathfinding.GetGrid().GetWorldPosition(x, y).y));
        }   
    }

    /// <summary>
    /// If the waypoint isnt equal to the current position, move character
    /// </summary>
    private void Movement()
    {
        if (m_currentWaypoint != character.position) character.position = Vector3.MoveTowards(character.position, m_currentWaypoint, moveSpeed * Time.deltaTime);
    }

    IEnumerator FollowPath()
    {
        m_currentWaypoint = m_pathfinding.GetGrid().GetWorldPosition(m_path[m_targetIndex].x, m_path[m_targetIndex].y) + Vector3.one * 0.5f;
        m_currentWaypoint.z = 0;

        while (true)
        {
            if (character.position == m_currentWaypoint)
            {
                m_targetIndex++;
                if (m_targetIndex >= m_path.Count)
                {
                    yield break;
                }

                m_currentWaypoint = m_pathfinding.GetGrid().GetWorldPosition(m_path[m_targetIndex].x, m_path[m_targetIndex].y) + Vector3.one * 0.5f;
                m_currentWaypoint.z = 0;
            }

            yield return null;
        }
    }
}
