namespace MsTest.To.FluentAssertions;

using System.Collections.ObjectModel;

public enum MsTestAssertionFunctionNames
{
    AreEqual,
    AreNotEqual,
    IsNull,
    IsNotNull,
    IsTrue,
    IsFalse
}

public enum FluentAssertionsFunctionNames
{
    Be,
    NotBe,
    BeNull,
    NotBeNull,
    BeTrue,
    BeFalse
}

public static class AssertionInfo
{
    public const string MsTestAssertStatement = "Assert.";

    public static readonly ReadOnlyDictionary<string, string> MsTestToFluentAssertions = new ReadOnlyDictionary<string, string>(
        new Dictionary<string, string>()
        {
            [MsTestAssertStatement + MsTestAssertionFunctionNames.AreEqual] = FluentAssertionsFunctionNames.Be.ToString(),
            [MsTestAssertStatement + MsTestAssertionFunctionNames.AreNotEqual] = FluentAssertionsFunctionNames.NotBe.ToString(),
            [MsTestAssertStatement + MsTestAssertionFunctionNames.IsNull] = FluentAssertionsFunctionNames.BeNull.ToString(),
            [MsTestAssertStatement + MsTestAssertionFunctionNames.IsNotNull] = FluentAssertionsFunctionNames.NotBeNull.ToString(),
            [MsTestAssertStatement + MsTestAssertionFunctionNames.IsTrue] = FluentAssertionsFunctionNames.BeTrue.ToString(),
            [MsTestAssertStatement + MsTestAssertionFunctionNames.IsFalse] = FluentAssertionsFunctionNames.BeFalse.ToString(),
        });
}