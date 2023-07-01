namespace LiteScript.Syntax.Expressions;

public class SyntaxErrorExpression : SyntaxExpression
{
    public SyntaxErrorExpression(SyntaxDocument document, int index, int length, string error)
        : base(document, index, length)
    {
        Error = error;
    }

    public string Error { get; }
}
