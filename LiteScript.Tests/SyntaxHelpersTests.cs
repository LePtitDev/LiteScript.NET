using LiteScript.Syntax;
using NUnit.Framework;

namespace LiteScript.Tests;

public class SyntaxHelpersTests
{
    private const double MaxFloatDelta = 0.0001;

    [Test]
    public void Test_can_read_hexadecimal_number()
    {
        const string text  = "   0x23_fC4  ";
        const long expected = 0x23fC4;

        var read = SyntaxHelpers.ReadNumber(text, 3, out var result);

        Assert.AreEqual(8, read);
        Assert.IsTrue(result.IsInteger);
        Assert.AreEqual(expected, result.AsInteger);
    }

    [Test]
    public void Test_can_read_binary_number()
    {
        const string text  = "   0b01_10_1  ";
        const long expected = 0b01101;

        var read = SyntaxHelpers.ReadNumber(text, 3, out var result);

        Assert.AreEqual(9, read);
        Assert.IsTrue(result.IsInteger);
        Assert.AreEqual(expected, result.AsInteger);
    }

    [Test]
    public void Test_can_read_octal_number()
    {
        const string text  = "   0o01_72  ";
        const long expected = 122;

        var read = SyntaxHelpers.ReadNumber(text, 3, out var result);

        Assert.AreEqual(7, read);
        Assert.IsTrue(result.IsInteger);
        Assert.AreEqual(expected, result.AsInteger);
    }

    [Test]
    public void Test_can_read_integer()
    {
        const string text  = "    154  ";
        const long expected = 154;

        var read = SyntaxHelpers.ReadNumber(text, 4, out var result);

        Assert.AreEqual(3, read);
        Assert.IsTrue(result.IsInteger);
        Assert.AreEqual(expected, result.AsInteger);
    }

    [Test]
    public void Test_can_read_negative_integer()
    {
        const string text  = "    -89  ";
        const long expected = -89;

        var read = SyntaxHelpers.ReadNumber(text, 4, out var result);

        Assert.AreEqual(3, read);
        Assert.IsTrue(result.IsInteger);
        Assert.AreEqual(expected, result.AsInteger);
    }

    [Test]
    public void Test_can_read_float()
    {
        const string text  = "    1.54  ";
        const double expected = 1.54;

        var read = SyntaxHelpers.ReadNumber(text, 4, out var result);

        Assert.AreEqual(4, read);
        Assert.IsFalse(result.IsInteger);
        Assert.AreEqual(expected, result.AsDouble, MaxFloatDelta);
    }

    [Test]
    public void Test_can_read_negative_float()
    {
        const string text  = "    -8.9  ";
        const double expected = -8.9;

        var read = SyntaxHelpers.ReadNumber(text, 4, out var result);

        Assert.AreEqual(4, read);
        Assert.IsFalse(result.IsInteger);
        Assert.AreEqual(expected, result.AsDouble, MaxFloatDelta);
    }

    [Test]
    public void Test_can_read_float_with_exponent()
    {
        const string text  = "    1.54e4  ";
        const double expected = 1.54e4;

        var read = SyntaxHelpers.ReadNumber(text, 4, out var result);

        Assert.AreEqual(6, read);
        Assert.IsFalse(result.IsInteger);
        Assert.AreEqual(expected, result.AsDouble, MaxFloatDelta);
    }

    [Test]
    public void Test_can_read_float_with_negative_exponent()
    {
        const string text  = "    8.9e-5  ";
        const double expected = 8.9e-5;

        var read = SyntaxHelpers.ReadNumber(text, 4, out var result);

        Assert.AreEqual(6, read);
        Assert.IsFalse(result.IsInteger);
        Assert.AreEqual(expected, result.AsDouble, MaxFloatDelta);
    }

    [Test]
    public void Test_can_read_negative_float_with_negative_exponent()
    {
        const string text  = "    -8.9e-5  ";
        const double expected = -8.9e-5;

        var read = SyntaxHelpers.ReadNumber(text, 4, out var result);

        Assert.AreEqual(7, read);
        Assert.IsFalse(result.IsInteger);
        Assert.AreEqual(expected, result.AsDouble, MaxFloatDelta);
    }

    [Test]
    public void Test_can_read_string_1()
    {
        const string text = "  \"my text\"  ";
        const string expected = "my text";

        var read = SyntaxHelpers.ReadString(text, 2, out var result);

        Assert.AreEqual(9, read);
        Assert.AreEqual('"', result.Delimiter);
        Assert.AreEqual(expected, result.Content);
    }

    [Test]
    public void Test_can_read_string_2()
    {
        const string text = "  'my other text'  ";
        const string expected = "my other text";

        var read = SyntaxHelpers.ReadString(text, 2, out var result);

        Assert.AreEqual(15, read);
        Assert.AreEqual('\'', result.Delimiter);
        Assert.AreEqual(expected, result.Content);
    }

    [Test]
    public void Test_can_read_string_with_escape()
    {
        const string text = "  \"my text with \\\"escape\\\"\"  ";
        const string expected = "my text with \"escape\"";

        var read = SyntaxHelpers.ReadString(text, 2, out var result);

        Assert.AreEqual(25, read);
        Assert.AreEqual('"', result.Delimiter);
        Assert.AreEqual(expected, result.Content);
    }
}
