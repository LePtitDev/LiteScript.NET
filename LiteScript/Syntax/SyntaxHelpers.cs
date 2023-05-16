namespace LiteScript.Syntax;

internal static class SyntaxHelpers
{
    public static int GetOperatorPriority(Operator op)
    {
        switch (op)
        {
            case Operator.PostIncr:
            case Operator.PostDecr:
            case Operator.Call:
            case Operator.Get:
            case Operator.Member:
                return 1;
            case Operator.PreIncr:
            case Operator.PreDecr:
            case Operator.UnaryPlus:
            case Operator.UnaryMinus:
            case Operator.Not:
            case Operator.BitNot:
            case Operator.New:
                return 2;
            case Operator.Mul:
            case Operator.Div:
            case Operator.Mod:
                return 3;
            case Operator.Add:
            case Operator.Sub:
                return 4;
            case Operator.LShift:
            case Operator.RShift:
                return 5;
            case Operator.Less:
            case Operator.LessEqu:
            case Operator.Great:
            case Operator.GreatEqu:
                return 6;
            case Operator.Equ:
            case Operator.Dif:
                return 7;
            case Operator.BitAnd:
                return 8;
            case Operator.BitXor:
                return 9;
            case Operator.BitOr:
                return 10;
            case Operator.And:
                return 11;
            case Operator.Or:
                return 12;
            case Operator.Assign:
            case Operator.AddAssign:
            case Operator.SubAssign:
            case Operator.MulAssign:
            case Operator.DivAssign:
                return 13;
            default:
                throw new ArgumentOutOfRangeException(nameof(op), op, $"Cannot resolve operator priority (op: {op})");
        }
    }

    public static int ReadUndefined(string text, int offset)
    {
        return ReadLiteral(text, offset, out var str) > 0 && string.Equals(str, "undefined", StringComparison.Ordinal) ? str.Length : 0;
    }

    public static int ReadNull(string text, int offset)
    {
        return ReadLiteral(text, offset, out var str) > 0 && string.Equals(str, "null", StringComparison.Ordinal) ? str.Length : 0;
    }

    public static int ReadThis(string text, int offset)
    {
        return ReadLiteral(text, offset, out var str) > 0 && string.Equals(str, "this", StringComparison.Ordinal) ? str.Length : 0;
    }

    public static int ReadBoolean(string text, int offset, out bool result)
    {
        if (ReadLiteral(text, offset, out var str) > 0)
        {
            if (str == "true")
            {
                result = true;
            }
            else if (str == "false")
            {
                result = false;
            }
            else
            {
                result = default;
                return 0;
            }

            return str.Length;
        }

        result = default;
        return 0;
    }

    public static int ReadNumber(string text, int offset, out Number result)
    {
        if (ReadHexadecimal(text, offset, out var integer) is var read and > 0)
        {
            result = new Number(integer);
            return read;
        }

        result = default;
        return 0;
    }

    private static int ReadLiteral(string text, int offset, out string literal)
    {
        var size = 0;
        for (var i = offset;
             i < text.Length &&
             ((text[i] >= 'a' && text[i] <= 'z') ||
              (text[i] >= 'A' && text[i] <= 'Z') ||
              (text[i] >= '0' && text[i] <= '9') ||
              text[i] == '$' ||
              text[i] == '_');
             i++)
        {
            ++size;
        }

        literal = size == 0 ? string.Empty : text.Substring(offset, size);
        return size;
    }

    private static int ReadHexadecimal(string text, int offset, out long number)
    {
        if (offset + 3 >= text.Length || text[offset] != '0' || text[offset + 1] != 'x' || text[offset + 2] is (< '0' or > '9') and (< 'A' or > 'F') and (< 'a' or > 'f'))
        {
            number = default;
            return 0;
        }

        number = 0;
        var size = 2;
        for (var i = offset + size; i < text.Length; i++)
        {
            var c = text[i];
            if (c is >= '0' and <= '9')
            {
                number = (number << 4) + (c - '0');
            }
            else if (c is >= 'A' and <= 'F')
            {
                number = (number << 4) + (c - 'A' + 10);
            }
            else if (c is >= 'a' and <= 'f')
            {
                number = (number << 4) + (c - 'a' + 10);
            }
            else if (c != '_')
            {
                break;
            }

            ++size;
        }

        return size;
    }
}