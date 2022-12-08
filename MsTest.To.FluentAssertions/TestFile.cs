using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsTest.To.FluentAssertions;

[TestClass]
public class TestFile
{
    [TestMethod]
    public void AssertionTest()
    {
        var x = "jopa";
        var q = "huy";
        
        Assert.AreEqual(q, "jopa");
    }
}