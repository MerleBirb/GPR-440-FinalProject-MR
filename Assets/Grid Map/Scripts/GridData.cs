// Author: Merle Roji

using System;
using UnityEngine;
using CodeMonkey.Utils;

/// <summary>
/// Contains the data for a grid object.
/// </summary>
/// Notes:
/// - Using Utils from https://unitycodemonkey.com/utils.php
/// - Based on Tutorial: https://youtu.be/waEsGu--9P8
public class GridData<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int m_width;
    private int m_height;
    private float m_cellSize;
    private TGridObject[,] m_gridArray;
    private Vector3 m_originPosition;
    private TextMesh[,] m_debugTextArray; 

    public GridData(int width, int height, float cellSize, Vector3 originPosition, Func<GridData<TGridObject>, int, int, TGridObject> createGridObject)
    {
        m_width = width;
        m_height = height;
        m_cellSize = cellSize;
        m_originPosition = originPosition;

        m_gridArray = new TGridObject[width, height];

        for (int x = 0; x < m_gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_gridArray.GetLength(1); y++)
            {
                m_gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            m_debugTextArray = new TextMesh[width, height];

            // cycle through multidimensional array
            for (int x = 0; x < m_gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < m_gridArray.GetLength(1); y++)
                {
                    m_debugTextArray[x, y] = UtilsClass.CreateWorldText(m_gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f,
                        16, Color.white, TextAnchor.MiddleCenter);

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                m_debugTextArray[eventArgs.x, eventArgs.y].text = m_gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    #region GETTERS - SETTERS

    /// <summary>
    /// Returns the width of the grid.
    /// </summary>
    /// <returns></returns>
    public int GetWidth()
    {
        return m_width;
    }

    /// <summary>
    /// Returns the height of the grid.
    /// </summary>
    /// <returns></returns>
    public int GetHeight()
    {
        return m_height;
    }

    /// <summary>
    /// Returns the cell size of the grid.
    /// </summary>
    /// <returns></returns>
    public float GetCellSize()
    {
        return m_cellSize;
    }

    /// <summary>
    /// Returns the world position by returning the x and y multiplied by the cell size.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(int x, int y)
    {
        return (new Vector3(x, y) * m_cellSize) + m_originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        var worldPosMinusOrigin = worldPosition - m_originPosition;

        x = Mathf.FloorToInt(worldPosMinusOrigin.x / m_cellSize);
        y = Mathf.FloorToInt(worldPosMinusOrigin.y / m_cellSize);
    }

    /// <summary>
    /// Sets the value of a given grid cell.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < m_width && y < m_height) // make sure its a valid grid position
        {
            m_gridArray[x, y] = value;
            TriggerGridObjectChanged(x, y);
        }
        else
        {
            Debug.LogError("Setting value to an invalid grid position.");
        }
    }

    /// <summary>
    /// Sets the value of a given grid cell.
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="value"></param>
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x;
        int y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    /// <summary>
    /// Returns a value from a given x and y position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < m_width && y < m_height)
        {
            return m_gridArray[x, y];
        }
        else
        {
            Debug.LogError("Returning an invalid value.");
            return default(TGridObject);
        }
    }

    /// <summary>
    /// Returns a value from a given Vector3 position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x;
        int y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    #endregion

    /// <summary>
    /// Triggers the OnGridObjectChanged event when a grid object is changed.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }
}
