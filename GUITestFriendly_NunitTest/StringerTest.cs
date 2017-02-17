using GUITestFriendly.Models;
using NUnit.Framework;

namespace GUITestFriendly_NunitTest
{

    [TestFixture]
    public class StringerTest
    {
        private Stringer Stringer;

        [SetUp]
        public void Init()
        {
            this.Stringer = new Stringer();
        }

        [TestCase("a", "b", "\"a\" + \"b\" = \"ab\";")]
        [TestCase("1", "b", "\"1\" + \"b\" = \"1b\";")]
        [TestCase("a", "2", "\"a\" + \"2\" = \"a2\";")]
        [TestCase("10", "10", "\"10\" + \"10\" = \"1010\";")]
        public void TestStringer(string str1, string str2, string a)
        {
            Assert.AreEqual(a, this.Stringer.Combine(str1, str2));
        }
    }
}
