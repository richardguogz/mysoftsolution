using MySoft.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using MySoft;

namespace MySoft.Data.Test
{
    
    
    /// <summary>
    ///这是 DbSessionTest 的测试类，旨在
    ///包含所有 DbSessionTest 单元测试
    ///</summary>
    [TestClass()]
    public class DbSessionTest
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
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
        ///Avg 的测试
        ///</summary>
        public void AvgTestHelper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Avg<T, TResult>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void AvgTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 AvgTestHelper<T, TResult>()。");
        }

        /// <summary>
        ///Avg 的测试
        ///</summary>
        public void AvgTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Avg<T>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void AvgTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 AvgTest1Helper<T>()。");
        }

        /// <summary>
        ///Avg 的测试
        ///</summary>
        public void AvgTest2Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Avg<T, TResult>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void AvgTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 AvgTest2Helper<T, TResult>()。");
        }

        /// <summary>
        ///Avg 的测试
        ///</summary>
        public void AvgTest3Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Avg<T>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void AvgTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 AvgTest3Helper<T>()。");
        }

        /// <summary>
        ///BeginBatch 的测试
        ///</summary>
        [TestMethod()]
        public void BeginBatchTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbBatch expected = null; // TODO: 初始化为适当的值
            DbBatch actual;
            actual = target.BeginBatch();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BeginBatch 的测试
        ///</summary>
        [TestMethod()]
        public void BeginBatchTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            int batchSize = 0; // TODO: 初始化为适当的值
            DbBatch expected = null; // TODO: 初始化为适当的值
            DbBatch actual;
            actual = target.BeginBatch(batchSize);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BeginTrans 的测试
        ///</summary>
        [TestMethod()]
        public void BeginTransTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbTrans expected = null; // TODO: 初始化为适当的值
            DbTrans actual;
            actual = target.BeginTrans();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BeginTrans 的测试
        ///</summary>
        [TestMethod()]
        public void BeginTransTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            IsolationLevel isolationLevel = new IsolationLevel(); // TODO: 初始化为适当的值
            DbTrans expected = null; // TODO: 初始化为适当的值
            DbTrans actual;
            actual = target.BeginTrans(isolationLevel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BeginTransaction 的测试
        ///</summary>
        [TestMethod()]
        public void BeginTransactionTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            IsolationLevel isolationLevel = new IsolationLevel(); // TODO: 初始化为适当的值
            DbTransaction expected = null; // TODO: 初始化为适当的值
            DbTransaction actual;
            actual = target.BeginTransaction(isolationLevel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BeginTransaction 的测试
        ///</summary>
        [TestMethod()]
        public void BeginTransactionTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbTransaction expected = null; // TODO: 初始化为适当的值
            DbTransaction actual;
            actual = target.BeginTransaction();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///CacheOff 的测试
        ///</summary>
        [TestMethod()]
        public void CacheOffTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            target.CacheOff();
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///CacheOn 的测试
        ///</summary>
        [TestMethod()]
        public void CacheOnTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            target.CacheOn();
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///Count 的测试
        ///</summary>
        public void CountTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Count<T>(table, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void CountTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 CountTestHelper<T>()。");
        }

        /// <summary>
        ///Count 的测试
        ///</summary>
        public void CountTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Count<T>(where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void CountTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 CountTest1Helper<T>()。");
        }

        /// <summary>
        ///CreateConnection 的测试
        ///</summary>
        [TestMethod()]
        public void CreateConnectionTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbConnection expected = null; // TODO: 初始化为适当的值
            DbConnection actual;
            actual = target.CreateConnection();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///CreateParameter 的测试
        ///</summary>
        [TestMethod()]
        public void CreateParameterTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbParameter expected = null; // TODO: 初始化为适当的值
            DbParameter actual;
            actual = target.CreateParameter();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Decrypt 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MySoft.Data.dll")]
        public void DecryptTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        public void DeleteTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Delete<T>(table, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 DeleteTestHelper<T>()。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        public void DeleteTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            object[] pkValues = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Delete<T>(table, pkValues);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void DeleteTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 DeleteTest1Helper<T>()。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        public void DeleteTest2Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Delete<T>(table, entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void DeleteTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 DeleteTest2Helper<T>()。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        public void DeleteTest3Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Delete<T>(entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void DeleteTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 DeleteTest3Helper<T>()。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        public void DeleteTest4Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            object[] pkValues = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Delete<T>(pkValues);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void DeleteTest4()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 DeleteTest4Helper<T>()。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        public void DeleteTest5Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Delete<T>(where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void DeleteTest5()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 DeleteTest5Helper<T>()。");
        }

        /// <summary>
        ///Excute 的测试
        ///</summary>
        [TestMethod()]
        public void ExcuteTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            UpdateCreator creator = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Excute(creator);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Excute 的测试
        ///</summary>
        [TestMethod()]
        public void ExcuteTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            InsertCreator creator = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Excute(creator);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Excute 的测试
        ///</summary>
        public void ExcuteTest2Helper<TResult>()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            InsertCreator creator = null; // TODO: 初始化为适当的值
            TResult identityValue = default(TResult); // TODO: 初始化为适当的值
            TResult identityValueExpected = default(TResult); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Excute<TResult>(creator, out identityValue);
            Assert.AreEqual(identityValueExpected, identityValue);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void ExcuteTest2()
        {
            ExcuteTest2Helper<GenericParameterHelper>();
        }

        /// <summary>
        ///Excute 的测试
        ///</summary>
        [TestMethod()]
        public void ExcuteTest3()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DeleteCreator creator = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Excute(creator);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Exists 的测试
        ///</summary>
        public void ExistsTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.Exists<T>(table, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void ExistsTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 ExistsTestHelper<T>()。");
        }

        /// <summary>
        ///Exists 的测试
        ///</summary>
        public void ExistsTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            object[] pkValues = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.Exists<T>(pkValues);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void ExistsTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 ExistsTest1Helper<T>()。");
        }

        /// <summary>
        ///Exists 的测试
        ///</summary>
        public void ExistsTest2Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.Exists<T>(where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void ExistsTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 ExistsTest2Helper<T>()。");
        }

        /// <summary>
        ///Exists 的测试
        ///</summary>
        public void ExistsTest3Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.Exists<T>(table, entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void ExistsTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 ExistsTest3Helper<T>()。");
        }

        /// <summary>
        ///Exists 的测试
        ///</summary>
        public void ExistsTest4Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.Exists<T>(entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void ExistsTest4()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 ExistsTest4Helper<T>()。");
        }

        /// <summary>
        ///Exists 的测试
        ///</summary>
        public void ExistsTest5Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            object[] pkValues = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.Exists<T>(table, pkValues);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void ExistsTest5()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 ExistsTest5Helper<T>()。");
        }

        /// <summary>
        ///From 的测试
        ///</summary>
        public void FromTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            FromSection<T> expected = null; // TODO: 初始化为适当的值
            FromSection<T> actual;
            actual = target.From<T>(table);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void FromTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 FromTestHelper<T>()。");
        }

        /// <summary>
        ///From 的测试
        ///</summary>
        public void FromTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            string aliasName = string.Empty; // TODO: 初始化为适当的值
            FromSection<T> expected = null; // TODO: 初始化为适当的值
            FromSection<T> actual;
            actual = target.From<T>(aliasName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void FromTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 FromTest1Helper<T>()。");
        }

        /// <summary>
        ///From 的测试
        ///</summary>
        public void FromTest2Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            TableRelation<T> relation = null; // TODO: 初始化为适当的值
            FromSection<T> expected = null; // TODO: 初始化为适当的值
            FromSection<T> actual;
            actual = target.From<T>(relation);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void FromTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 FromTest2Helper<T>()。");
        }

        /// <summary>
        ///From 的测试
        ///</summary>
        public void FromTest3Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            FromSection<T> expected = null; // TODO: 初始化为适当的值
            FromSection<T> actual;
            actual = target.From<T>();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void FromTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 FromTest3Helper<T>()。");
        }

        /// <summary>
        ///From 的测试
        ///</summary>
        [TestMethod()]
        public void FromTest4()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            QueryCreator creator = null; // TODO: 初始化为适当的值
            QuerySection expected = null; // TODO: 初始化为适当的值
            QuerySection actual;
            actual = target.From(creator);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FromProc 的测试
        ///</summary>
        [TestMethod()]
        public void FromProcTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            string procName = string.Empty; // TODO: 初始化为适当的值
            SQLParameter[] parameters = null; // TODO: 初始化为适当的值
            ProcSection expected = null; // TODO: 初始化为适当的值
            ProcSection actual;
            actual = target.FromProc(procName, parameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FromProc 的测试
        ///</summary>
        [TestMethod()]
        public void FromProcTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            string procName = string.Empty; // TODO: 初始化为适当的值
            IDictionary<string, object> parameters = null; // TODO: 初始化为适当的值
            ProcSection expected = null; // TODO: 初始化为适当的值
            ProcSection actual;
            actual = target.FromProc(procName, parameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FromSql 的测试
        ///</summary>
        [TestMethod()]
        public void FromSqlTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            string sql = string.Empty; // TODO: 初始化为适当的值
            SQLParameter[] parameters = null; // TODO: 初始化为适当的值
            SqlSection expected = null; // TODO: 初始化为适当的值
            SqlSection actual;
            actual = target.FromSql(sql, parameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///FromSql 的测试
        ///</summary>
        [TestMethod()]
        public void FromSqlTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            string sql = string.Empty; // TODO: 初始化为适当的值
            IDictionary<string, object> parameters = null; // TODO: 初始化为适当的值
            SqlSection expected = null; // TODO: 初始化为适当的值
            SqlSection actual;
            actual = target.FromSql(sql, parameters);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///InitSession 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MySoft.Data.dll")]
        public void InitSessionTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T>(table, fvs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTestHelper<T>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field[] fields = null; // TODO: 初始化为适当的值
            object[] values = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T>(table, fields, values);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest1Helper<T>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest2Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            TResult retVal = default(TResult); // TODO: 初始化为适当的值
            TResult retValExpected = default(TResult); // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T, TResult>(entity, out retVal, fvs);
            Assert.AreEqual(retValExpected, retVal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest2Helper<T, TResult>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest3Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            TResult retVal = default(TResult); // TODO: 初始化为适当的值
            TResult retValExpected = default(TResult); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T, TResult>(table, fvs, out retVal);
            Assert.AreEqual(retValExpected, retVal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest3Helper<T, TResult>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest4Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            TResult retVal = default(TResult); // TODO: 初始化为适当的值
            TResult retValExpected = default(TResult); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T, TResult>(fvs, out retVal);
            Assert.AreEqual(retValExpected, retVal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest4()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest4Helper<T, TResult>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest5Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field[] fields = null; // TODO: 初始化为适当的值
            object[] values = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T>(fields, values);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest5()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest5Helper<T>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest6Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field[] fields = null; // TODO: 初始化为适当的值
            object[] values = null; // TODO: 初始化为适当的值
            TResult retVal = default(TResult); // TODO: 初始化为适当的值
            TResult retValExpected = default(TResult); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T, TResult>(table, fields, values, out retVal);
            Assert.AreEqual(retValExpected, retVal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest6()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest6Helper<T, TResult>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest7Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T>(fvs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest7()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest7Helper<T>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest8Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field[] fields = null; // TODO: 初始化为适当的值
            object[] values = null; // TODO: 初始化为适当的值
            TResult retVal = default(TResult); // TODO: 初始化为适当的值
            TResult retValExpected = default(TResult); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T, TResult>(fields, values, out retVal);
            Assert.AreEqual(retValExpected, retVal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest8()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest8Helper<T, TResult>()。");
        }

        /// <summary>
        ///Insert 的测试
        ///</summary>
        public void InsertTest9Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            TResult retVal = default(TResult); // TODO: 初始化为适当的值
            TResult retValExpected = default(TResult); // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Insert<T, TResult>(table, entity, out retVal, fvs);
            Assert.AreEqual(retValExpected, retVal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertTest9()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertTest9Helper<T, TResult>()。");
        }

        /// <summary>
        ///InsertOrUpdate 的测试
        ///</summary>
        public void InsertOrUpdateTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.InsertOrUpdate<T>(table, entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertOrUpdateTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertOrUpdateTestHelper<T>()。");
        }

        /// <summary>
        ///InsertOrUpdate 的测试
        ///</summary>
        public void InsertOrUpdateTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.InsertOrUpdate<T>(entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void InsertOrUpdateTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 InsertOrUpdateTest1Helper<T>()。");
        }

        /// <summary>
        ///Max 的测试
        ///</summary>
        public void MaxTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Max<T>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MaxTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MaxTestHelper<T>()。");
        }

        /// <summary>
        ///Max 的测试
        ///</summary>
        public void MaxTest1Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Max<T, TResult>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MaxTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MaxTest1Helper<T, TResult>()。");
        }

        /// <summary>
        ///Max 的测试
        ///</summary>
        public void MaxTest2Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Max<T, TResult>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MaxTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MaxTest2Helper<T, TResult>()。");
        }

        /// <summary>
        ///Max 的测试
        ///</summary>
        public void MaxTest3Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Max<T>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MaxTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MaxTest3Helper<T>()。");
        }

        /// <summary>
        ///Min 的测试
        ///</summary>
        public void MinTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Min<T>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MinTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MinTestHelper<T>()。");
        }

        /// <summary>
        ///Min 的测试
        ///</summary>
        public void MinTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Min<T>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MinTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MinTest1Helper<T>()。");
        }

        /// <summary>
        ///Min 的测试
        ///</summary>
        public void MinTest2Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Min<T, TResult>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MinTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MinTest2Helper<T, TResult>()。");
        }

        /// <summary>
        ///Min 的测试
        ///</summary>
        public void MinTest3Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Min<T, TResult>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void MinTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 MinTest3Helper<T, TResult>()。");
        }

        /// <summary>
        ///RegisterOnEndHandler 的测试
        ///</summary>
        [TestMethod()]
        public void RegisterOnEndHandlerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            ExcutingEventHandler handler = null; // TODO: 初始化为适当的值
            target.RegisterOnEndHandler(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///RegisterOnStartHandler 的测试
        ///</summary>
        [TestMethod()]
        public void RegisterOnStartHandlerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            ExcutingEventHandler handler = null; // TODO: 初始化为适当的值
            target.RegisterOnStartHandler(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///RegisterSqlExceptionLogger 的测试
        ///</summary>
        [TestMethod()]
        public void RegisterSqlExceptionLoggerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            ErrorLogEventHandler handler = null; // TODO: 初始化为适当的值
            target.RegisterSqlExceptionLogger(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///RegisterSqlLogger 的测试
        ///</summary>
        [TestMethod()]
        public void RegisterSqlLoggerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            LogEventHandler handler = null; // TODO: 初始化为适当的值
            target.RegisterSqlLogger(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///RemoveAllCache 的测试
        ///</summary>
        [TestMethod()]
        public void RemoveAllCacheTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            target.RemoveAllCache();
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///RemoveCache 的测试
        ///</summary>
        public void RemoveCacheTestHelper<T>()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            target.RemoveCache<T>();
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        [TestMethod()]
        public void RemoveCacheTest()
        {
            RemoveCacheTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///Save 的测试
        ///</summary>
        public void SaveTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Save<T>(entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SaveTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SaveTestHelper<T>()。");
        }

        /// <summary>
        ///Save 的测试
        ///</summary>
        public void SaveTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            T entity = default(T); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Save<T>(table, entity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SaveTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SaveTest1Helper<T>()。");
        }

        /// <summary>
        ///Serialization 的测试
        ///</summary>
        [TestMethod()]
        public void SerializationTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            OrderByClip order = null; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = target.Serialization(order);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Serialization 的测试
        ///</summary>
        [TestMethod()]
        public void SerializationTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = target.Serialization(where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SetConnection 的测试
        ///</summary>
        [TestMethod()]
        public void SetConnectionTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbConnection connection = null; // TODO: 初始化为适当的值
            DbTrans expected = null; // TODO: 初始化为适当的值
            DbTrans actual;
            actual = target.SetConnection(connection);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SetDefault 的测试
        ///</summary>
        [TestMethod()]
        public void SetDefaultTest()
        {
            string connectName = string.Empty; // TODO: 初始化为适当的值
            DbSession.SetDefault(connectName);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///SetDefault 的测试
        ///</summary>
        [TestMethod()]
        public void SetDefaultTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession.SetDefault(dbProvider);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///SetProvider 的测试
        ///</summary>
        [TestMethod()]
        public void SetProviderTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            string connectName = string.Empty; // TODO: 初始化为适当的值
            target.SetProvider(connectName);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///SetProvider 的测试
        ///</summary>
        [TestMethod()]
        public void SetProviderTest1()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbProvider dbProvider1 = null; // TODO: 初始化为适当的值
            target.SetProvider(dbProvider1);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///SetTransaction 的测试
        ///</summary>
        [TestMethod()]
        public void SetTransactionTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            DbTransaction trans = null; // TODO: 初始化为适当的值
            DbTrans expected = null; // TODO: 初始化为适当的值
            DbTrans actual;
            actual = target.SetTransaction(trans);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Single 的测试
        ///</summary>
        public void SingleTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            T expected = default(T); // TODO: 初始化为适当的值
            T actual;
            actual = target.Single<T>(table, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SingleTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SingleTestHelper<T>()。");
        }

        /// <summary>
        ///Single 的测试
        ///</summary>
        public void SingleTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            object[] pkValues = null; // TODO: 初始化为适当的值
            T expected = default(T); // TODO: 初始化为适当的值
            T actual;
            actual = target.Single<T>(table, pkValues);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SingleTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SingleTest1Helper<T>()。");
        }

        /// <summary>
        ///Single 的测试
        ///</summary>
        public void SingleTest2Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            object[] pkValues = null; // TODO: 初始化为适当的值
            T expected = default(T); // TODO: 初始化为适当的值
            T actual;
            actual = target.Single<T>(pkValues);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SingleTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SingleTest2Helper<T>()。");
        }

        /// <summary>
        ///Single 的测试
        ///</summary>
        public void SingleTest3Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            T expected = default(T); // TODO: 初始化为适当的值
            T actual;
            actual = target.Single<T>(where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SingleTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SingleTest3Helper<T>()。");
        }

        /// <summary>
        ///Sum 的测试
        ///</summary>
        public void SumTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Sum<T>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SumTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SumTestHelper<T>()。");
        }

        /// <summary>
        ///Sum 的测试
        ///</summary>
        public void SumTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            object expected = null; // TODO: 初始化为适当的值
            object actual;
            actual = target.Sum<T>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SumTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SumTest1Helper<T>()。");
        }

        /// <summary>
        ///Sum 的测试
        ///</summary>
        public void SumTest2Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Sum<T, TResult>(field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SumTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SumTest2Helper<T, TResult>()。");
        }

        /// <summary>
        ///Sum 的测试
        ///</summary>
        public void SumTest3Helper<T, TResult>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            TResult expected = default(TResult); // TODO: 初始化为适当的值
            TResult actual;
            actual = target.Sum<T, TResult>(table, field, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void SumTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 SumTest3Helper<T, TResult>()。");
        }

        /// <summary>
        ///UnregisterOnEndHandler 的测试
        ///</summary>
        [TestMethod()]
        public void UnregisterOnEndHandlerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            ExcutingEventHandler handler = null; // TODO: 初始化为适当的值
            target.UnregisterOnEndHandler(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///UnregisterOnStartHandler 的测试
        ///</summary>
        [TestMethod()]
        public void UnregisterOnStartHandlerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            ExcutingEventHandler handler = null; // TODO: 初始化为适当的值
            target.UnregisterOnStartHandler(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///UnregisterSqlExceptionLogger 的测试
        ///</summary>
        [TestMethod()]
        public void UnregisterSqlExceptionLoggerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            ErrorLogEventHandler handler = null; // TODO: 初始化为适当的值
            target.UnregisterSqlExceptionLogger(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///UnregisterSqlLogger 的测试
        ///</summary>
        [TestMethod()]
        public void UnregisterSqlLoggerTest()
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            LogEventHandler handler = null; // TODO: 初始化为适当的值
            target.UnregisterSqlLogger(handler);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTestHelper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(table, fvs, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTestHelper<T>()。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTest1Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            FieldValue fv = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(table, fv, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest1()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTest1Helper<T>()。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTest2Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            object value = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(table, field, value, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest2()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTest2Helper<T>()。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTest3Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Table table = null; // TODO: 初始化为适当的值
            Field[] fields = null; // TODO: 初始化为适当的值
            object[] values = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(table, fields, values, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest3()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTest3Helper<T>()。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTest4Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            FieldValue[] fvs = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(fvs, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest4()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTest4Helper<T>()。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTest5Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            FieldValue fv = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(fv, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest5()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTest5Helper<T>()。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTest6Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field[] fields = null; // TODO: 初始化为适当的值
            object[] values = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(fields, values, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest6()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTest6Helper<T>()。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        public void UpdateTest7Helper<T>()
            where T : Entity
        {
            DbProvider dbProvider = null; // TODO: 初始化为适当的值
            DbSession target = new DbSession(dbProvider); // TODO: 初始化为适当的值
            Field field = null; // TODO: 初始化为适当的值
            object value = null; // TODO: 初始化为适当的值
            WhereClip where = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update<T>(field, value, where);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void UpdateTest7()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 UpdateTest7Helper<T>()。");
        }
    }
}
