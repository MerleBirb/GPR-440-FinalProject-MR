// Merle Roji

using UnityEngine;

/// <summary>
/// Scriptable Object version of an int.
/// </summary>
/// Notes:

[CreateAssetMenu(menuName = "New Variable/Integer", fileName = "Int")]
public class IntVariable : ScriptableObject
{
    public int Value;
}
