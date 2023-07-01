using LiteScript.Syntax.Expressions;

namespace LiteScript.Syntax;

public abstract class SyntaxExpression
{
    protected SyntaxExpression(SyntaxDocument document, int index, int length)
    {
        Document = document;
        Index = index;
        Length = length;
    }

    public SyntaxDocument Document { get; }

    public int Index { get; }

    public int Length { get; }

    internal static SyntaxExpression? Read(SyntaxDocument document, int offset)
    {

    }

    internal static SyntaxExpression? Read(SyntaxDocument document, int offset, Stack<SyntaxOperator> ops, ref List<SyntaxCommentExpression>? comments)
    {
        var i = offset;
        var needValue = true;
        while (true)
        {
            ReadWhitespace(document, ref i, ref comments);

            if (needValue)
            {
                SyntaxOperator? prefixOperator = null;
                var read = SyntaxHelpers.ReadPrefixOperator(document.Content, i, out var op);
                if (read > 0)
                {
                    i += read;
                    prefixOperator = op;
                }
                else
                {
                    ReadWhitespace(document, ref i, ref comments);
                }

                var val = ReadValue(document, i);
                if (val == null)
                {
                    if (prefixOperator != null)
                    {
                        return new SyntaxErrorExpression(document, i, 0, "Value expected after prefix operator");
                    }

                    return null;
                }

                i += val.Length;

                ReadWhitespace(document, ref i, ref comments);

                SyntaxOperator? suffixOperator = null;
                read = SyntaxHelpers.ReadPrefixOperator(document.Content, i, out op);
                if (read > 0)
                {
                    i += read;
                    suffixOperator = op;
                }
                else
                {
                    ReadWhitespace(document, ref i, ref comments);
                }

                needValue = false;
            }
        }
    }

    private static SyntaxExpression? ReadValue(SyntaxDocument document, int offset)
    {

    }

    private static void ReadWhitespace(SyntaxDocument document, ref int offset, ref List<SyntaxCommentExpression>? comments)
    {
        var i = offset;
        while (true)
        {
            var read = SyntaxHelpers.ReadWhitespace(document.Content, i);
            if (read > 0)
            {
                i += read;
                continue;
            }

            var comment = SyntaxCommentExpression.Read(document, i);
            if (comment != null)
            {
                i += read;
                (comments ??= new List<SyntaxCommentExpression>()).Add(comment);
                continue;
            }

            break;
        }
    }
}
