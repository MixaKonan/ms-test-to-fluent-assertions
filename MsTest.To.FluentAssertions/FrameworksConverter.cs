namespace MsTest.To.FluentAssertions;

using System.Text;
public static class FrameworksConverter
{
    public static string ToFluentAssertion(string assertion)
    {
        var actualLine = assertion;

        if (assertion.StartsWith(AssertionInfo.MsTestAssertStatement))
        {
            foreach (var assertionFunction in Enum.GetValues<MsTestAssertionFunctionNames>())
            {
                if (TryParseLine(assertion, assertionFunction.ToString(), out actualLine))
                {
                    return actualLine;
                }
            }
        }

        return actualLine;
    }

    private static bool TryParseLine(string trimmedLine, string msTestFunction, out string actualLine)
    {
        actualLine = string.Empty;
        var assertFunctionCall = AssertionInfo.MsTestAssertStatement + msTestFunction;
        if (trimmedLine.StartsWith(assertFunctionCall))
        {
            actualLine = ParseToFluentAssertions(trimmedLine, assertFunctionCall);
            return true;
        }

        return false;
    }

    private static string ParseToFluentAssertions(string trimmedLine, string assertFunctionCall)
    {
        var stringBuilder = new StringBuilder(trimmedLine);
        stringBuilder.Remove(0, $"{assertFunctionCall}(".Length);
        stringBuilder.Remove(stringBuilder.Length - 1, 1);

        var fluentAssertFunction = AssertionInfo.MsTestToFluentAssertions[assertFunctionCall];

        switch (fluentAssertFunction)
        {
            case "Be":
            case "NotBe":
                var assertionValues = stringBuilder.ToString().Split(", ");
                var expected = assertionValues[0];
                var actual = assertionValues[1];

                return $"{actual}.Should().{fluentAssertFunction}({expected});";

            case "BeNull":
            case "NotBeNull":
            case "BeTrue":
            case "BeFalse":
                return $"{stringBuilder}.Should().{fluentAssertFunction}();";

            default:
                throw new NotSupportedException();
        }
    }
}