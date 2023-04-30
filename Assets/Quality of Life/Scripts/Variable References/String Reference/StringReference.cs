// Merle Roji

using System;

/// <summary>
/// Reference to the Scriptable Object version of a string.
/// </summary>
/// Notes:

[Serializable]
public class StringReference
{
    public bool UseConstant = true;
    public string ConstantValue;
    public StringVariable Variable;

    public StringReference()
    {
        UseConstant = true;
    }

    public StringReference(string value) : this()
    {
        ConstantValue = value;
    }

    public string String
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
