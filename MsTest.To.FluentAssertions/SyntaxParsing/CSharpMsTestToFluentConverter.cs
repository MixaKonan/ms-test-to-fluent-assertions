using static MsTest.To.FluentAssertions.SyntaxParsing.AssertionInfo;

namespace MsTest.To.FluentAssertions.SyntaxParsing;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class CSharpMsTestToFluentConverter
{
    public static bool TryConvert(string fileText, out string newFileText)
    {
        newFileText = string.Empty;
        var tree = CSharpSyntaxTree.ParseText(fileText);
        var originalRoot = (CompilationUnitSyntax)tree.GetRoot();

        var msTestAssertions = originalRoot.DescendantNodes().OfType<InvocationExpressionSyntax>()
            .Where(invocation => invocation.Expression is MemberAccessExpressionSyntax { Expression: IdentifierNameSyntax identifierName }
                && identifierName.ToString() == Assert)
            .ToList();

        if (!msTestAssertions.Any())
        {
            return false;
        }

        var nodesToReplaceWith = new Dictionary<SyntaxNode, SyntaxNode>();
        foreach (var assertion in msTestAssertions)
        {
            var arguments = assertion.ArgumentList.Arguments;

            var msTestAssertionKind = GetAssertionKind(assertion);
            var (fluentAssertionKind, hasSecondArgument) = FrameworksMapping[msTestAssertionKind];

            InvocationExpressionSyntax fluentAssertion;
            if (hasSecondArgument)
            {
                var (actual, expected) = GetArguments(arguments);
                fluentAssertion = fluentAssertionKind switch
                {
                    FluentAssertionKind.Be => actual.Should().Be(expected),
                    FluentAssertionKind.NotBe => actual.Should().NotBe(expected),
                    _ => throw new NotSupportedException()
                };
            }
            else
            {
                var actual = arguments.First();
                fluentAssertion = fluentAssertionKind switch
                {
                    FluentAssertionKind.BeNull => actual.Should().BeNull(),
                    FluentAssertionKind.NotBeNull => actual.Should().NotBeNull(),
                    FluentAssertionKind.BeTrue => actual.Should().BeTrue(),
                    FluentAssertionKind.BeFalse => actual.Should().BeFalse(),
                    _ => throw new NotSupportedException()
                };
            }

            nodesToReplaceWith.Add(assertion, fluentAssertion);
        }

        var newRoot = originalRoot.ReplaceNodes(msTestAssertions, (node, _) => nodesToReplaceWith[node]);
        newFileText = newRoot.ToFullString();
        return true;
    }

    private static (ArgumentSyntax, ArgumentSyntax) GetArguments(SeparatedSyntaxList<ArgumentSyntax> arguments)
    {
        var expected = arguments.First();
        var actual = arguments.Last();

        if (actual.Expression is LiteralExpressionSyntax)
        {
            (expected, actual) = (actual, expected);
        }

        return (actual, expected);
    }

    private static MsTestAssertionKind GetAssertionKind(InvocationExpressionSyntax assertion)
    {
        var assertionKindText = ((MemberAccessExpressionSyntax)assertion.Expression).ChildNodes().Last().ToString();

        return Enum.TryParse<MsTestAssertionKind>(assertionKindText, out var assertionKind)
            ? assertionKind
            : throw new NotSupportedException();
    }
}