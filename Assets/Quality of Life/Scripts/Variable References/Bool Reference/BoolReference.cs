// Merle Roji

using System;

/// <summary>
/// Reference to the Scriptable Object version of a bool.
/// </summary>
/// Notes:

[Serializable]
public class BoolReference
{
    public bool UseConstant = true;
    public bool ConstantValue;
    public BoolVariable Variable;

    public BoolReference()
    {
        UseConstant = true;
    }

    public BoolReference(bool value) : this()
    {
        ConstantValue = value;
    }

    public bool Bool
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
