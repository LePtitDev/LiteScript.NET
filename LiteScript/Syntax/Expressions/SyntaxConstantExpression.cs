using LiteScript.Core;

namespace LiteScript.Syntax.Expressions;

public class SyntaxConstantExpression : SyntaxExpression
{
    private SyntaxConstantExpression(SyntaxDocument document, int index, int length, object value)
        : base(document, index, length)
    {
        Value = value;
    }

    public object Value { get; }

    internal static SyntaxConstantExpression? Read(SyntaxDocument document, int offset)
    {
        var read = SyntaxHelpers.ReadUndefined(document.Content, offset);
        if (read > 0)
            return new SyntaxConstantExpression(document, offset, read, EmptyValue.Undefined);

        read = SyntaxHelpers.ReadNull(document.Content, offset);
        if (read > 0)
            return new SyntaxConstantExpression(document, offset, read, EmptyValue.Null);

        read = SyntaxHelpers.ReadBoolean(document.Content, offset, out var b);
        if (read > 0)
            return new SyntaxConstantExpression(document, offset, read, b);

        read = SyntaxHelpers.ReadNumber(document.Content, offset, out var n);
        if (read > 0)
            return new SyntaxConstantExpression(document, offset, read, n);

        read = SyntaxHelpers.ReadString(document.Content, offset, out var s);
        if (read > 0)
            return new SyntaxConstantExpression(document, offset, read, s!);

        return null;
    }
}
