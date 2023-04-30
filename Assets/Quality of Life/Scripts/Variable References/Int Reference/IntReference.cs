// Merle Roji

using System;

/// <summary>
/// Reference to the Scriptable Object version of an int.
/// </summary>
/// Notes:

[Serializable]
public class IntReference
{
    public bool UseConstant = true;
    public int ConstantValue;
    public IntVariable Variable;

    public IntReference()
    {
        UseConstant = true;
    }

    public IntReference(int value) : this()
    {
        ConstantValue = value;
    }

    public int Int
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
