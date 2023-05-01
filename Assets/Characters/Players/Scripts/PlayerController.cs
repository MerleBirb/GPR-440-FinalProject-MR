// Merle Roji

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the player object's logic using input from the player.
/// </summary>
/// Notes:
/// - Based on Tutorial: https://youtu.be/_Pm16a18zy8
public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField, Min(0f)] private float baseMoveSpeed = 1f;
    [SerializeField, Min(0f)] private float radius = 0.25f;

    private bool m_isMoving;
    private Vector2 m_input;

    #region UNITY METHODS

    private void Update()
    {
        CheckInput();
    }

    #endregion

    /// <summary>
    /// Checks input from the player.
    /// </summary>
    private void CheckInput()
    {
        // Grid Movement
        if (!m_isMoving) // movement can only happen if the player hasn't inputted anything yet
        {
            // movement inputs
            m_input.x = Input.GetAxisRaw("Horizontal");
            m_input.y = Input.GetAxisRaw("Vertical");

            if (m_input != Vector2.zero)  // check if movement inputs have been made
            {
                var targetPos = transform.position; // init target position with own position
                targetPos.x += m_input.x;
                targetPos.y += m_input.y;

                if (IsWalkable(targetPos)) StartCoroutine(Move(targetPos));
            }
        }
    }

    /// <summary>
    /// Moves the player towards a target position.
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    IEnumerator Move(Vector3 targetPos)
    {
        m_isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, baseMoveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        m_isMoving = false;
    }

    /// <summary>
    /// Checks if a tile position is walkable using an Overlap Sphere.
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, radius, collisionLayer) != null) // if there is a solid object in the target position
        {
            return false;
        }

        return true;
    }
}
