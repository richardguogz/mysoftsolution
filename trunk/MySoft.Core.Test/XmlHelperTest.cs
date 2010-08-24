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
        ///Create 的测试
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            string path = "c:\\test.xml"; // TODO: 初始化为适当的值

            using (XmlHelper target = new XmlHelper(path)) // TODO: 初始化为适当的值
            {
                string element = "config"; // TODO: 初始化为适当的值
                //target.Insert(element, new string[] { "key", "value" }, new string[] { "pokey1", "povalue1" })
                //    .Insert(element, new string[] { "key", "value" }, new string[] { "pokey2", "povalue2" })
                //    .Insert(element, new string[] { "key", "value" }, new string[] { "pokey3", "povalue3" });

                ////target.GetNode(element).Update(new string[] { "key", "value" }, new string[] { "pokey123", "povalue123" });
                ////Assert.Inconclusive("无法验证不返回值的方法。");

                //target.GetNode("config.config").Insert(element, new string[] { "key", "value" }, new string[] { "pokey3", "povalue3" });

                XmlNodeHelper[] h = target.GetNode(element).GetNode(element).GetNodes(element);
            }
        }
    }
}
