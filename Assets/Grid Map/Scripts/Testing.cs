using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private GridData<bool> m_grid;

    private void Start()
    {
        //m_grid = new GridData<bool>(10, 10, 1f, new Vector3(0, 0));
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
        if (Input.GetMouseButtonDown(0))
        {
            m_grid.SetValue(UtilsClass.GetMouseWorldPosition(), true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(m_grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}
