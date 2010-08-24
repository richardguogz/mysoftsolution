using MySoft.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MySoft.Core.Test
{


    /// <summary>
    ///这是 XmlHelperTest 的测试类，旨在
    ///包含所有 XmlHelperTest 单元测试
    ///</summary>
    [TestClass()]
    public class XmlHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试属性
        // 
        //编写测试时，还可使用以下属性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            string path = string.Empty; // TODO: 初始化为适当的值
            XmlHelper target = new XmlHelper(path); // TODO: 初始化为适当的值
            string node = string.Empty; // TODO: 初始化为适当的值
            string attribute = string.Empty; // TODO: 初始化为适当的值
            string value = string.Empty; // TODO: 初始化为适当的值
            target.Update(node, attribute, value);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///Read 的测试
        ///</summary>
        [TestMethod()]
        public void ReadTest()
        {
            string path = string.Empty; // TODO: 初始化为适当的值
            XmlHelper target = new XmlHelper(path); // TODO: 初始化为适当的值
            string node = string.Empty; // TODO: 初始化为适当的值
            string attribute = string.Empty; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = target.Read(node, attribute);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        [TestMethod()]
        public void InsertTest()
        {
            string path = string.Empty; // TODO: 初始化为适当的值
            XmlHelper target = new XmlHelper(path); // TODO: 初始化为适当的值
            string node = string.Empty; // TODO: 初始化为适当的值
            string element = string.Empty; // TODO: 初始化为适当的值
            string attribute = string.Empty; // TODO: 初始化为适当的值
            string value = string.Empty; // TODO: 初始化为适当的值
            target.Insert(node, element, attribute, value);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            string path = string.Empty; // TODO: 初始化为适当的值
            XmlHelper target = new XmlHelper(path); // TODO: 初始化为适当的值
            string node = string.Empty; // TODO: 初始化为适当的值
            string attribute = string.Empty; // TODO: 初始化为适当的值
            target.Delete(node, attribute);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///Create 的测试
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            string path = "c:\\test.xml"; // TODO: 初始化为适当的值
            using (XmlHelper target = new XmlHelper(path)) // TODO: 初始化为适当的值
            {
                string element = "config"; // TODO: 初始化为适当的值
                //target.Create(element);
                //target.Insert(element, new string[] { "key", "value" }, new string[] { "pokey", "povalue" });
                //target.Insert(element, "config", new string[] { "key", "value" }, new string[] { "pokey1", "povalue1" });

                target.Update(element, new string[] { "key", "value" }, new string[] { "pokey123", "povalue123" });

                //Assert.Inconclusive("无法验证不返回值的方法。");
            }
        }

        /// <summary>
        ///XmlHelper 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void XmlHelperConstructorTest()
        {
            string path = string.Empty; // TODO: 初始化为适当的值
            XmlHelper target = new XmlHelper(path);
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }
    }
}
