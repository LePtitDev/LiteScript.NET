namespace LiteScript.Core;

public readonly struct EmptyValue
{
    public static readonly EmptyValue Undefined = new EmptyValue(false);
     
    public static readonly EmptyValue Null = new EmptyValue(true);

    public EmptyValue()
    {
        IsNull = false;
    }

    public EmptyValue(bool isNull)
    {
        IsNull = isNull;
    }

    public bool IsUndefined => !IsNull;

    public bool IsNull { get; }
}