namespace LiteScript.Syntax;

internal struct SyntaxString
{
    public SyntaxString(char delimiter, string content)
    {
        Delimiter = delimiter;
        Content = content;
    }

    public char Delimiter { get; }

    public string Content { get; }
}
