using LiteScript.Syntax;
using NUnit.Framework;

namespace LiteScript.Tests;

public class SyntaxHelpersTests
{
    [Test]
    public void Test_can_read_hex_number()
    {
        const string text  = "   0x23_fC4  ";
        const int expected = 0x23fC4;

        var read = SyntaxHelpers.ReadNumber(text, 3, out var result);

        Assert.AreEqual(8, read);
        Assert.IsTrue(result.IsInteger);
        Assert.AreEqual(expected, result.AsInteger);
    }
}