// Author: Merle Roji

using UnityEngine;

/// <summary>
/// Field of view projects a light that cuts through darkness in the field of view. 
/// </summary>
/// Notes:
/// - Based on Tutorial: https://youtu.be/CSeUMTaNFYk
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask targetMask;

    private Mesh m_mesh;
    private Vector3 m_origin;
    private float m_fov;
    private float m_startingAngle;
    private Transform m_target;
    public Transform Target
    {
        get => m_target;
    }

    private void Start()
    {
        InitFOVMesh();
    }

    private void LateUpdate()
    {
        UpdateMesh();
    }

    /// <summary>
    /// Initializes the light mesh.
    /// </summary>
    private void InitFOVMesh()
    {
        // create mesh
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = m_mesh;
        m_origin = transform.position;
        m_fov = 360f;
    }

    /// <summary>
    /// Sets various variables for the mesh.
    /// </summary>
    private void UpdateMesh()
    {
        int rayCount = 180;
        float angle = m_startingAngle;
        float angleIncrease = m_fov / rayCount;
        float viewDistance = 50f;

        // init components
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = m_origin;
        int vertIndex = 1;
        int triIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hitObstacle = Physics2D.Raycast(m_origin, GetVectorFromAngle(angle), viewDistance, obstacleMask);

            if (hitObstacle.collider == null) // no hit
            {
                vertex = m_origin + GetVectorFromAngle(angle) * viewDistance;
                m_target = null; // if there is no target to detect
            }
            else // hit object
            {
                vertex = hitObstacle.point;
                if (hitObstacle.transform.tag == "Player") // check if the obstacle is a player
                {
                    m_target = hitObstacle.transform; // set target to player
                }
            }

            vertices[vertIndex] = vertex;

            if (i > 0)
            {
                triangles[triIndex + 0] = 0;
                triangles[triIndex + 1] = vertIndex - 1;
                triangles[triIndex + 2] = vertIndex;

                triIndex += 3;
            }

            vertIndex++;
            angle -= angleIncrease; // clockwise
        }

        m_mesh.vertices = vertices;
        m_mesh.uv = uv;
        m_mesh.triangles = triangles;
    }

    /// <summary>
    /// Set origin of the mesh.
    /// </summary>
    /// <param name="origin"></param>
    public void SetOrigin(Vector3 origin)
    {
        m_origin = origin;
    }

    /// <summary>
    /// Sets the direction of the mesh.
    /// </summary>
    /// <param name="dir"></param>
    public void SetAimDirection(Vector3 dir)
    {
        m_startingAngle = GetAngleFromVectorFloat(dir) - m_fov / 2f;
    }

    /// <summary>
    /// Returns a vector from a given angle.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRadians = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
    }

    /// <summary>
    /// Returns an angle from a given angle.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
