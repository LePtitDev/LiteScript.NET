namespace LiteScript.Syntax;

internal struct SyntaxComment
{
    public SyntaxComment(string value, bool isInline)
    {
        Value = value;
        IsInline = isInline;
    }

    public string Value { get; }

    public bool IsInline { get; }
}
