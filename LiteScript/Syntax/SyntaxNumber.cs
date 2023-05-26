namespace LiteScript.Syntax;

internal struct SyntaxNumber
{
    public SyntaxNumber(long value)
    {
        IsInteger = true;
        AsInteger = value;
        AsDouble = default;
    }

    public SyntaxNumber(double value)
    {
        IsInteger = false;
        AsInteger = default;
        AsDouble = value;
    }

    public bool IsInteger { get; }

    public long AsInteger { get; }

    public double AsDouble { get; }
}
