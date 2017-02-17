using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RM.Friendly.WPFStandardControls;
using System.Windows;
using Codeer.Friendly.Windows.Grasp;
using System.Windows.Controls;
using System.Diagnostics;
using Codeer.Friendly.Windows;
using System.Linq;
using Codeer.Friendly.Dynamic;

namespace GUITestFriendly_MsTest
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

    [TestClass]
    public class UnitTestGUI
    {
        private Process process;
        private WindowsAppFriend app;
        private MainWindowDriver driver;
        [TestInitialize]
        public void Initialize()
        {
            this.process = Process.Start(@"GUITestFriendly.exe");
            this.app = new WindowsAppFriend(this.process);
            this.driver = new MainWindowDriver(this.app.Type<Application>().Current.MainWindow);
        }


        [TestCleanup]
        public void Cleanup()
        {
            this.app.Dispose();
            this.process.Kill();
        }


        [TestMethod]
        public void TestAllButtonClick()
        {
            var buttons = this.driver.LogicalTree.ByType<Button>();
            for (int i = 0; i < buttons.Count; i++)
            {
                new WPFButtonBase(buttons[i]).EmulateClick();
            }
        }

        [TestMethod]
        public void TestInputTextBox()
        {
            this.driver.Lhs.EmulateChangeText("10");
            this.driver.Rhs.EmulateChangeText("3");

            Assert.AreEqual("10", this.driver.Lhs.Text);
            Assert.AreEqual("3", this.driver.Rhs.Text);
        }

        [TestMethod]
        public void TestButtonClick0_1()
        {
            this.driver.Lhs.EmulateChangeText("10");
            this.driver.Rhs.EmulateChangeText("3");
            this.driver.Add.EmulateClick();
            Assert.AreEqual("\"10\" + \"3\" = \"103\";", this.driver.Answer.Text);
        }

        [TestMethod]
        public void TestButtonClick0_2()
        {
            this.driver.Lhs.EmulateChangeText("SS");
            this.driver.Rhs.EmulateChangeText("A");
            this.driver.Buttons[0].EmulateClick();
            Assert.AreEqual("\"SS\" + \"A\" = \"SSA\";", this.driver.Answer.Text);
        }

        [TestMethod]
        public void TestButtonClick1()
        {
            this.driver.Buttons[1].EmulateClick();
            Assert.AreEqual("!!!", this.driver.Answer.Text);
        }
        [TestMethod]
        public void TestButtonClick2()
        {
            this.driver.Buttons[2].EmulateClick();
            Assert.AreEqual("???", this.driver.Answer.Text);
        }

        [TestMethod]
        public void TestButtonClick3_1()
        {
            this.driver.Buttons[3].EmulateClick();
            Assert.AreEqual("666", this.driver.Answer.Text);
        }

        [TestMethod]
        public void TestButtonClick3_2()
        {
            new WPFButtonBase(this.driver.LogicalTree.ByBinding("ACommand").Single()).EmulateClick();
            Assert.AreEqual("666", this.driver.Answer.Text);
        }


        [TestMethod]
        public void TestButtonClick4_1()
        {
            this.driver.Buttons[4].EmulateClick();
            Assert.AreEqual("111", this.driver.Answer.Text);
        }

        [TestMethod]
        public void TestButtonClick4_2()
        {
            Assert.AreEqual(2, this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").Count);
            //同じコマンドにバインドしていてパラメータで分かれている場合はさらにByCommandParameterで分ける
            new WPFButtonBase(this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").ByCommandParameter("1").Single()).EmulateClick();
            Assert.AreEqual("111", this.driver.Answer.Text);
        }
        public void TestButtonClick5_1()
        {
            this.driver.Buttons[5].EmulateClick();
            Assert.AreEqual("QQQ", this.driver.Answer.Text);
        }

        [TestMethod]
        public void TestButtonClick5_2()
        {
            Assert.AreEqual(2, this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").Count);
            //同じコマンドにバインドしていてパラメータで分かれている場合はさらにByCommandParameterで分ける
            new WPFButtonBase(this.driver.LogicalTree.ByType<Button>().ByBinding("BCommand").ByCommandParameter("Q").Single()).EmulateClick();
            Assert.AreEqual("QQQ", this.driver.Answer.Text);
        }
    }
}
