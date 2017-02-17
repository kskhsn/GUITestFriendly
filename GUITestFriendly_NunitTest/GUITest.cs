using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Windows.Grasp;
using NUnit.Framework;
using RM.Friendly.WPFStandardControls;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace GUITestFriendly_NunitTest
{
    public class MainWindowDriver
    {
        public WPFTextBox Lhs { get; }
        public WPFTextBox Rhs { get; }
        public WPFButtonBase Add { get; }
        public WPFTextBlock Answer { get; }

        public WPFButtonBase[] Buttons { get; }

        public IWPFDependencyObjectCollection<DependencyObject> LogicalTree { get; }

        public MainWindowDriver(dynamic window)
        {
            //必要な要素はここで取得する。
            var w = new WindowControl(window);

            this.LogicalTree = w.LogicalTree();

            this.Lhs = new WPFTextBox(this.LogicalTree.ByBinding("Lhs").Single());
            this.Rhs = new WPFTextBox(this.LogicalTree.ByBinding("Rhs").Single());
            this.Answer = new WPFTextBlock(this.LogicalTree.ByBinding("Answer").Single());

            var btns = this.LogicalTree.ByType<Button>();

            this.Add = new WPFButtonBase(btns[0]);

            this.Buttons = Enumerable.Range(0, btns.Count).Select(i => new WPFButtonBase(btns[i])).ToArray();

        }
    }

    [TestFixture]
    class GUITest
    {
        private Process process;
        private WindowsAppFriend app;
        private MainWindowDriver driver;
        [SetUp]
        public void Initialize()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var exename = Path.Combine(dir, "GUITestFriendly.exe");

            this.process = Process.Start(exename);
            this.app = new WindowsAppFriend(this.process);
            this.driver = new MainWindowDriver(this.app.Type<Application>().Current.MainWindow);
        }


        [TearDown]
        public void Cleanup()
        {
            this.app.Dispose();
            this.process.Kill();
        }


        [Test]
        public void TestAllButtonClick()
        {
            var buttons = this.driver.LogicalTree.ByType<Button>();
            for (int i = 0; i < buttons.Count; i++)
            {
                new WPFButtonBase(buttons[i]).EmulateClick();
            }
        }

        [TestCase("10", "3", "10", "3")]
        [TestCase("a", "3", "a", "3")]
        [TestCase("あ", "ん", "あ", "ん")]
        public void TestInputTextBox(string str1, string str2, string expected1, string expected2)
        {
            this.driver.Lhs.EmulateChangeText(str1);
            this.driver.Rhs.EmulateChangeText(str2);

            Assert.AreEqual(expected1, this.driver.Lhs.Text);
            Assert.AreEqual(expected2, this.driver.Rhs.Text);
        }

        [TestCase("10", "3", "\"10\" + \"3\" = \"103\";")]
        [TestCase("SS", "A", "\"SS\" + \"A\" = \"SSA\";")]
        [TestCase("SS", "55", "\"SS\" + \"55\" = \"SS55\";")]
        [TestCase("55", "HH", "\"55\" + \"HH\" = \"55HH\";")]
        public void TestButtonClick0_1(string str1, string str2, string expected)
        {
            this.driver.Lhs.EmulateChangeText(str1);
            this.driver.Rhs.EmulateChangeText(str2);
            this.driver.Add.EmulateClick();
            Assert.AreEqual(expected, this.driver.Answer.Text);
        }

        [TestCase("10", "3", "\"10\" + \"3\" = \"103\";")]
        [TestCase("SS", "A", "\"SS\" + \"A\" = \"SSA\";")]
        [TestCase("SS", "55", "\"SS\" + \"55\" = \"SS55\";")]
        [TestCase("55", "HH", "\"55\" + \"HH\" = \"55HH\";")]
        public void TestButtonClick0_2(string str1, string str2, string expected)
        {
            this.driver.Lhs.EmulateChangeText(str1);
            this.driver.Rhs.EmulateChangeText(str2);
            this.driver.Buttons[0].EmulateClick();
            Assert.AreEqual(expected, this.driver.Answer.Text);
        }

        [Test]
        public void TestButtonClick1()
        {
            this.driver.Buttons[1].EmulateClick();
            Assert.AreEqual("!!!", this.driver.Answer.Text);
        }
        [Test]
        public void TestButtonClick2()
        {
            this.driver.Buttons[2].EmulateClick();
            Assert.AreEqual("???", this.driver.Answer.Text);
        }

        [Test]
        public void TestButtonClick3_1()
        {
            this.driver.Buttons[3].EmulateClick();
            Assert.AreEqual("666", this.driver.Answer.Text);
        }
        [Test]
        public void TestButtonClick3_2()
        {
            new WPFButtonBase(this.driver.LogicalTree.ByBinding("ACommand").Single()).EmulateClick();
            Assert.AreEqual("666", this.driver.Answer.Text);
        }


        [Test]
        public void TestButtonClick4_1()
        {
            this.driver.Buttons[4].EmulateClick();
            Assert.AreEqual("111", this.driver.Answer.Text);
        }

        [Test]
        public void TestButtonClick4_2()
        {
            Assert.AreEqual(2, this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").Count);
            //同じコマンドにバインドしていてパラメータで分かれている場合はさらにByCommandParameterで分ける
            new WPFButtonBase(this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").ByCommandParameter("1").Single()).EmulateClick();
            Assert.AreEqual("111", this.driver.Answer.Text);
        }
        [Test]
        public void TestButtonClick5_1()
        {
            this.driver.Buttons[5].EmulateClick();
            Assert.AreEqual("QQQ", this.driver.Answer.Text);
        }
        [Test]
        public void TestButtonClick5_2()
        {
            Assert.AreEqual(2, this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").Count);
            //同じコマンドにバインドしていてパラメータで分かれている場合はさらにByCommandParameterで分ける
            new WPFButtonBase(this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").ByCommandParameter("Q").Single()).EmulateClick();
            Assert.AreEqual("QQQ", this.driver.Answer.Text);
        }
    }
}
