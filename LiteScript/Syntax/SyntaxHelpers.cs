using System.Text;
using LiteScript.Core;

namespace LiteScript.Syntax;

internal static class SyntaxHelpers
{
    public static int GetOperatorPriority(SyntaxOperator op)
    {
        switch (op)
        {
            case SyntaxOperator.PostIncr:
            case SyntaxOperator.PostDecr:
            case SyntaxOperator.Call:
            case SyntaxOperator.Get:
            case SyntaxOperator.Member:
                return 1;
            case SyntaxOperator.PreIncr:
            case SyntaxOperator.PreDecr:
            case SyntaxOperator.UnaryPlus:
            case SyntaxOperator.UnaryMinus:
            case SyntaxOperator.Not:
            case SyntaxOperator.BitNot:
            case SyntaxOperator.New:
                return 2;
            case SyntaxOperator.Mul:
            case SyntaxOperator.Div:
            case SyntaxOperator.Mod:
                return 3;
            case SyntaxOperator.Add:
            case SyntaxOperator.Sub:
                return 4;
            case SyntaxOperator.LShift:
            case SyntaxOperator.RShift:
                return 5;
            case SyntaxOperator.Less:
            case SyntaxOperator.LessEqu:
            case SyntaxOperator.Great:
            case SyntaxOperator.GreatEqu:
                return 6;
            case SyntaxOperator.Equ:
            case SyntaxOperator.Dif:
                return 7;
            case SyntaxOperator.BitAnd:
                return 8;
            case SyntaxOperator.BitXor:
                return 9;
            case SyntaxOperator.BitOr:
                return 10;
            case SyntaxOperator.And:
                return 11;
            case SyntaxOperator.Or:
                return 12;
            case SyntaxOperator.Assign:
            case SyntaxOperator.AddAssign:
            case SyntaxOperator.SubAssign:
            case SyntaxOperator.MulAssign:
            case SyntaxOperator.DivAssign:
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
        if (ReadHexadecimal(text, offset, out var hex) is var hexRead and > 0)
        {
            result = new Number(hex);
            return hexRead;
        }

        if (ReadBinary(text, offset, out var bin) is var binRead and > 0)
        {
            result = new Number(bin);
            return binRead;
        }

        if (ReadOctal(text, offset, out var oct) is var octRead and > 0)
        {
            result = new Number(oct);
            return octRead;
        }

        if (ReadFloat(text, offset, out var floatN) is var floatRead and > 0)
        {
            result = new Number(floatN);
            return floatRead;
        }

        if (ReadInteger(text, offset, out var intN) is var intNRead and > 0)
        {
            result = new Number(intN);
            return intNRead;
        }

        result = default;
        return 0;
    }

    public static int ReadString(string text, int offset, out string? str)
    {
        if (text[offset] is not ('"' or '\''))
        {
            str = default;
            return 0;
        }

        var bld = new StringBuilder();
        var delimiter = text[offset];
        var size = 1;
        var escape = false;
        for (var i = offset + size; i < text.Length; i++)
        {
            var c = text[i];
            if (escape)
            {
                escape = false;
                bld.Append(c);
            }
            else if (c == '\\')
            {
                escape = true;
            }
            else if (c == delimiter)
            {
                str = bld.ToString();
                return size + 1;
            }
            else
            {
                bld.Append(c);
            }

            ++size;
        }

        str = default;
        return 0;
    }

    public static int ReadLiteral(string text, int offset, out string literal)
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

    public static int ReadComment(string text, int offset, out string? comment)
    {
        if (offset + 1 >= text.Length || text[offset] != '/' || text[offset + 1] is not ('/' or '*'))
        {
            comment = default;
            return 0;
        }

        var size = 2;
        var isInline = text[offset + 1] == '/';
        var bld = new StringBuilder();
        if (isInline)
        {
            for (var i = offset + size; i < text.Length; i++)
            {
                var c = text[i];
                if (c == '\n')
                {
                    break;
                }
                else if (c >= ' ' /* upper than [space] character = 32 */)
                {
                    // Lower characters are control characters
                    bld.Append(c);
                }

                ++size;
            }
        }
        else
        {
            var lastStar = false;
            for (var i = offset + size; i < text.Length; i++)
            {
                ++size;
                var c = text[i];
                if (lastStar)
                {
                    if (c == '/')
                        break;

                    bld.Append('*');
                }

                lastStar = c == '*';
                if (lastStar)
                    continue;

                if (c is '\n' or >= ' ' /* upper than [space] character = 32 */)
                {
                    // Lower characters are control characters
                    bld.Append(c);
                }
            }
        }

        comment = bld.ToString();
        return size;
    }

    public static int ReadWhitespace(string text, int offset)
    {
        var i = offset;
        while (i < text.Length)
        {
            if (text[i] is ' ' or '\t' or '\r' or '\n')
            {
                ++i;
            }
            else
            {
                break;
            }
        }

        return i - offset;
    }

    public static int ReadPrefixOperator(string text, int offset, out SyntaxOperator result)
    {
        if (text[offset] == '+')
        {
            if (offset + 1 < text.Length && text[offset + 1] == '+')
            {
                result = SyntaxOperator.PreIncr;
                return 2;
            }

            result = SyntaxOperator.UnaryPlus;
            return 1;
        }

        if (text[offset] == '-')
        {
            if (offset + 1 < text.Length && text[offset + 1] == '-')
            {
                result = SyntaxOperator.PreDecr;
                return 2;
            }

            result = SyntaxOperator.UnaryMinus;
            return 1;
        }

        if (text[offset] == '!')
        {
            result = SyntaxOperator.Not;
            return 1;
        }

        if (text[offset] == '~')
        {
            result = SyntaxOperator.BitNot;
            return 1;
        }

        result = default;
        return 0;
    }

    public static int ReadSuffixOperator(string text, int offset, out SyntaxOperator result)
    {
        if (offset + 1 < text.Length)
        {
            if (text[offset] == '+' && text[offset + 1] == '+')
            {
                result = SyntaxOperator.PostIncr;
                return 2;
            }

            if (text[offset] == '-' && text[offset + 1] == '-')
            {
                result = SyntaxOperator.PostDecr;
                return 2;
            }
        }

        result = default;
        return 0;
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

    private static int ReadBinary(string text, int offset, out long number)
    {
        if (offset + 3 >= text.Length || text[offset] != '0' || text[offset + 1] != 'b' || text[offset + 2] is not ('0' or '1'))
        {
            number = default;
            return 0;
        }

        number = 0;
        var size = 2;
        for (var i = offset + size; i < text.Length; i++)
        {
            var c = text[i];
            if (c is '0' or '1')
            {
                number = (number << 1) + (c - '0');
            }
            else if (c != '_')
            {
                break;
            }

            ++size;
        }

        return size;
    }

    private static int ReadOctal(string text, int offset, out long number)
    {
        if (offset + 3 >= text.Length || text[offset] != '0' || text[offset + 1] != 'o' || text[offset + 2] is < '0' or > '7')
        {
            number = default;
            return 0;
        }

        number = 0;
        var size = 2;
        for (var i = offset + size; i < text.Length; i++)
        {
            var c = text[i];
            if (c is >= '0' and <= '7')
            {
                number = number * 8 + (c - '0');
            }
            else if (c != '_')
            {
                break;
            }

            ++size;
        }

        return size;
    }

    private static int ReadFloat(string text, int offset, out double number)
    {
        var size = 0;
        var negative = false;
        if (text[offset] is var firstChar and ('-' or '+'))
        {
            ++size;
            negative = firstChar == '-';
        }

        if (offset + size >= text.Length || text[offset + size] is (< '0' or > '9') and not '.')
        {
            number = default;
            return 0;
        }

        var exponent1 = 0;
        var foundDot = false;
        var integer = 0L;
        for (var i = offset + size; i < text.Length; i++)
        {
            var c = text[i];
            if (c is >= '0' and <= '9')
            {
                integer = integer * 10 + (c - '0');
                if (foundDot)
                    --exponent1;
            }
            else if (c is '.')
            {
                if (foundDot)
                {
                    number = default;
                    return 0;
                }

                foundDot = true;
            }
            else if (c != '_')
            {
                break;
            }

            ++size;
        }

        var foundExponent = offset + size < text.Length && text[offset + size] is 'e' or 'E';
        if (foundExponent)
        {
            ++size;
            var negativeExponent = false;
            if (offset + size < text.Length && text[offset + size] is var firstExponentChar and ('-' or '+'))
            {
                ++size;
                negativeExponent = firstExponentChar == '-';
            }

            var exponent2 = 0;
            for (var i = offset + size; i < text.Length; i++)
            {
                var c = text[i];
                if (c is >= '0' and <= '9')
                {
                    exponent2 = exponent2 * 10 + (c - '0');
                }
                else if (c != '_')
                {
                    break;
                }

                ++size;
            }

            if (negativeExponent)
                exponent2 = -exponent2;

            exponent1 += exponent2;
        }

        if (!foundDot && !foundExponent)
        {
            number = default;
            return 0;
        }

        number = integer * Math.Pow(10, exponent1);
        if (negative)
            number = -number;

        return size;
    }

    private static int ReadInteger(string text, int offset, out long number)
    {
        var size = 0;
        var negative = false;
        if (text[offset] is var firstChar and ('-' or '+'))
        {
            ++size;
            negative = firstChar == '-';
        }

        if (text[offset + size] is < '0' or > '9')
        {
            number = default;
            return 0;
        }

        number = 0;
        for (var i = offset + size; i < text.Length; i++)
        {
            var c = text[i];
            if (c is >= '0' and <= '9')
            {
                number = number * 10 + (c - '0');
            }
            else if (c != '_')
            {
                break;
            }

            ++size;
        }

        if (negative)
            number = -number;

        return size;
    }
}
