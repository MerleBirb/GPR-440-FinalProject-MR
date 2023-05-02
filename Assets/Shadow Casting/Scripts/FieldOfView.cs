// Author: Merle Roji

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

/// <summary>
/// Field of view projects a light that cuts through darkness in the field of view. 
/// </summary>
/// Notes:
/// - Using Utilies from https://unitycodemonkey.com/utils.php
/// - Based on Tutorial: https://youtu.be/CSeUMTaNFYk
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask blockMask;

    private Mesh m_mesh;
    private Vector3 m_origin;
    private float m_fov;
    private float m_startingAngle;

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
            RaycastHit2D hit = Physics2D.Raycast(m_origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, blockMask);

            if (hit.collider == null) // no hit
            {
                vertex = m_origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            }
            else // hit object
            {
                vertex = hit.point;
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
        m_startingAngle = UtilsClass.GetAngleFromVectorFloat(dir) - m_fov / 2f;
    }
}
