using GUITestFriendly.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUITestFriendly_MsTest
{
    [TestClass]
    public class UnitTestStringer
    {
        private Stringer Stringer;
        [TestInitialize]
        public void Init()
        {
            this.Stringer = new Stringer();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("\"a\" + \"b\" = \"ab\";", this.Stringer.Combine("a", "b"));
        }
    }
}
