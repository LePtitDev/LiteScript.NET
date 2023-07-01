namespace LiteScript.Syntax.Expressions;

public class SyntaxCommentExpression : SyntaxExpression
{
    private SyntaxCommentExpression(SyntaxDocument document, int index, int length, string comment)
        : base(document, index, length)
    {
        Comment = comment;
    }

    public string Comment { get; }

    internal static SyntaxCommentExpression? Read(SyntaxDocument document, int offset)
    {
        var read = SyntaxHelpers.ReadComment(document.Content, offset, out var comment);
        return read == 0 ? null : new SyntaxCommentExpression(document, offset, read, comment!);
    }
}
