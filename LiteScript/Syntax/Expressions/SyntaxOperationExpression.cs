namespace LiteScript.Syntax.Expressions;

public class SyntaxOperationExpression : SyntaxExpression
{
    private SyntaxOperationExpression(SyntaxDocument document, int index, int length, SyntaxOperator @operator, SyntaxExpression left, SyntaxExpression? right)
        : base(document, index, length)
    {
        Operator = @operator;
        Left = left;
        Right = right;
    }

    public SyntaxOperator Operator { get; }

    public SyntaxExpression Left { get; }

    public SyntaxExpression? Right { get; }
}
