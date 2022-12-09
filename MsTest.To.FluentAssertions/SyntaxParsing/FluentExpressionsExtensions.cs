namespace MsTest.To.FluentAssertions.SyntaxParsing;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class FluentExpressionsExtensions
{
    public static InvocationExpressionSyntax Should(this ArgumentSyntax invokeOn)
    {
        var shouldInvocation = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    invokeOn.Expression,
                    SyntaxFactory.IdentifierName(AssertionInfo.Should)));

        return shouldInvocation;
    }

    public static InvocationExpressionSyntax Be(this InvocationExpressionSyntax should, ArgumentSyntax expected)
    {
        var fluentInvocation = should.Fluent(AssertionInfo.Be).WithArgument(expected);

        return fluentInvocation;
    }

    public static InvocationExpressionSyntax NotBe(this InvocationExpressionSyntax should, ArgumentSyntax expected)
    {
        var fluentInvocation = should.Fluent(AssertionInfo.NotBe).WithArgument(expected);

        return fluentInvocation;
    }

    public static InvocationExpressionSyntax BeTrue(this InvocationExpressionSyntax should)
    {
        var fluentInvocation = should.Fluent(AssertionInfo.BeTrue);

        return fluentInvocation;
    }

    public static InvocationExpressionSyntax BeFalse(this InvocationExpressionSyntax should)
    {
        var fluentInvocation = should.Fluent(AssertionInfo.BeFalse);

        return fluentInvocation;
    }

    public static InvocationExpressionSyntax BeNull(this InvocationExpressionSyntax should)
    {
        var fluentInvocation = should.Fluent(AssertionInfo.BeNull);

        return fluentInvocation;
    }

    public static InvocationExpressionSyntax NotBeNull(this InvocationExpressionSyntax should)
    {
        var fluentInvocation = should.Fluent(AssertionInfo.NotBeNull);

        return fluentInvocation;
    }

    private static InvocationExpressionSyntax Fluent(this InvocationExpressionSyntax should, string methodName)
    {
        var fluentInvocation = SyntaxFactory.InvocationExpression(
            SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                should,
                SyntaxFactory.IdentifierName(methodName)));

        return fluentInvocation;
    }

    private static InvocationExpressionSyntax WithArgument(this InvocationExpressionSyntax argumentExpectingAssertion, ArgumentSyntax argument)
    {
        return argumentExpectingAssertion.WithArgumentList(
            SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Argument(argument.Expression))));
    }
}