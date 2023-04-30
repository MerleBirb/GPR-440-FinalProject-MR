// Merle Roji

using System;

/// <summary>
/// Reference to the Scriptable Object version of a float.
/// </summary>
/// Notes:

[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public FloatReference()
    {
        UseConstant = true;
    }

    public FloatReference(float value) : this()
    {
        ConstantValue = value;
    }

    public float Float
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
