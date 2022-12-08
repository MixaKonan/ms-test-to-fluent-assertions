using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MsTest.To.FluentAssertions.AST;

public class AstParser
{
    public static void Parse()
    {
        var fileText = File.ReadAllText(@"C:\Users\fater\ms-test-to-fluent\MsTest.To.FluentAssertions\TestFile.cs");
        var tree = CSharpSyntaxTree.ParseText(fileText);
        var root = tree.GetRoot();
        var assertionNodes = root.DescendantNodes().Where(node => node is InvocationExpressionSyntax).ToList();

        foreach (var assertionNode in assertionNodes)
        {
            var assertionDescendants = assertionNode.DescendantNodes().ToList();
            var assertionArguments = (ArgumentListSyntax) assertionDescendants.First(node => node is ArgumentListSyntax);

            var actual = assertionArguments.Arguments.First();
            var expected = assertionArguments.Arguments.Last();

            if (expected.Expression is LiteralExpressionSyntax)
            {
                (expected, actual) = (actual, expected);
            }

            var expectedInGoodPlace = assertionNode.ReplaceNode(actual, expected);
            var expectedInGoodPlaceExpectedNode = expectedInGoodPlace.DescendantNodes().Last(node => node is ArgumentSyntax);
            var swapped = expectedInGoodPlace.ReplaceNode(expectedInGoodPlaceExpectedNode, actual);
        }
    }
}