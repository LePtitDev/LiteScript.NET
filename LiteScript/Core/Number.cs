namespace LiteScript.Core;

public struct Number
{
    public Number(long value)
    {
        IsInteger = true;
        AsInteger = value;
        AsDouble = default;
    }

    public Number(double value)
    {
        IsInteger = false;
        AsInteger = default;
        AsDouble = value;
    }

    public bool IsInteger { get; }

    public long AsInteger { get; }

    public double AsDouble { get; }
}
