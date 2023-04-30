// Merle Roji

using UnityEngine;

/// <summary>
/// Scriptable Object version of a float.
/// </summary>
/// Notes:

[CreateAssetMenu(menuName = "New Variable/Float", fileName = "Float")]
public class FloatVariable : ScriptableObject
{
    public float Value;
}
