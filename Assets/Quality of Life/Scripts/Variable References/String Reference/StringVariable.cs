// Merle Roji

using UnityEngine;

/// <summary>
/// Scriptable Object version of a string.
/// </summary>
/// Notes:

[CreateAssetMenu(menuName = "New Variable/String", fileName = "String")]
public class StringVariable : ScriptableObject
{
    public string Value;
}
