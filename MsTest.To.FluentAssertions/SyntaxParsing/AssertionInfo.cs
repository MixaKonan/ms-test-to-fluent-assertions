namespace MsTest.To.FluentAssertions.SyntaxParsing;

public static class AssertionInfo
{
    public enum MsTestAssertionKind
    {
        AreEqual,
        AreNotEqual,
        IsNull,
        IsNotNull,
        IsTrue,
        IsFalse
    }
    public enum FluentAssertionKind
    {
        Be,
        NotBe,
        BeNull,
        NotBeNull,
        BeTrue,
        BeFalse
    }

    public const string Assert = "Assert";

    public const string Should = "Should";
    public const string Be = "Be";
    public const string NotBe = "NotBe";
    public const string BeNull = "BeNull";
    public const string NotBeNull = "NotBeNull";
    public const string BeTrue = "BeTrue";
    public const string BeFalse = "BeFalse";

    /// <summary>
    /// MsAssertionKind to FluentAssertionKind and bool value whether the method is supposed to have a second argument
    /// </summary>
    public static readonly Dictionary<MsTestAssertionKind, (FluentAssertionKind, bool)> FrameworksMapping = new Dictionary<MsTestAssertionKind, (FluentAssertionKind, bool)>
    {
        [MsTestAssertionKind.AreEqual] = (FluentAssertionKind.Be, true),
        [MsTestAssertionKind.AreNotEqual] = (FluentAssertionKind.NotBe, true),
        [MsTestAssertionKind.IsNull] = (FluentAssertionKind.BeNull, false),
        [MsTestAssertionKind.IsNotNull] = (FluentAssertionKind.NotBeNull, false),
        [MsTestAssertionKind.IsTrue] = (FluentAssertionKind.BeTrue, false),
        [MsTestAssertionKind.IsFalse] = (FluentAssertionKind.BeFalse, false),
    };
}