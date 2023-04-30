// Merle Roji

using UnityEngine;

/// <summary>
/// Scriptable Object version of a bool.
/// </summary>
/// Notes:

[CreateAssetMenu(menuName = "New Variable/Boolean", fileName = "Bool")]
public class BoolVariable : ScriptableObject
{
    public bool Value;
}
