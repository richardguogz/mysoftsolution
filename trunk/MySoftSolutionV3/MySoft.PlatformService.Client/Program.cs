using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC;
using MySoft.Remoting;
using System.Threading;
using System.Diagnostics;
using MySoft.PlatformService.UserService;
using MySoft.Logger;
using MySoft.Cache;

namespace MySoft.PlatformService.Client
{
    class Program
    {
        ////д�߳̽�����д��myData
        //static int myData = 0;

        ////��д����
        //const int readWriteCount = 10;

        ////false:��ʼʱû���ź�
        //static AutoResetEvent autoResetEvent = new AutoResetEvent(true);

        //static void Main(string[] args)
        //{
        //    //����һ�����߳�(���߳�)
        //    Thread readerThread = new Thread(new ThreadStart(ReadThreadProc));
        //    readerThread.Name = "ReaderThread";
        //    readerThread.Start();

        //    for (int i = 1; i <= readWriteCount; i++)
        //    {
        //        Console.WriteLine("MainThread writing : {0}", i);

        //        //��(д)�߳̽�����д��
        //        myData = 0;

        //        //��(д)�̷߳��źţ�˵��ֵ��д����
        //        //��֪ͨ���ڵȴ����߳����¼�����
        //        autoResetEvent.Set();

        //        Thread.Sleep(1);
        //    }

        //    //��ֹ�߳�
        //    //readerThread.Abort();

        //    Console.ReadKey();
        //}

        //static void ReadThreadProc()
        //{
        //    while (true)
        //    {
        //        //�����ݱ�д��ǰ�����̵߳ȴ���ʵ�����ǵȴ�д�̷߳�������д����źţ�
        //        autoResetEvent.WaitOne();
        //        Console.WriteLine("{0} reading : {1}", Thread.CurrentThread.Name, myData);
        //    }
        //}

        private static readonly object syncobj = new object();
        static void Main(string[] args)
        {
            //CastleFactoryConfiguration config = CastleFactoryConfiguration.GetConfig();

            //LogEventHandler logger = Console.WriteLine;
            //MemoryServiceMQ mq = new MemoryServiceMQ();
            //mq.OnLog += new LogEventHandler(mq_OnLog);
            //mq.OnError += new ErrorLogEventHandler(mq_OnError);

            //CastleServiceHelper cs = new CastleServiceHelper(config);
            //cs.OnLog += logger;
            //cs.PublishWellKnownServiceInstance(mq);

            //Console.WriteLine("Service MQ Server started...");
            //Console.WriteLine("Logger Status: On");
            //Console.WriteLine("Press any key to exit and stop server...");
            //Console.ReadLine();

            //CastleFactory.Create().OnError += new ErrorLogEventHandler(mq_OnError);
            //CastleFactory.Create().OnLog += new LogEventHandler(mq_OnLog);
            //Console.ReadKey();

            //int count = 1;

            //var castle = CastleFactory.Create();
            ////castle.RegisterCacheDependent(DefaultCacheDependent.Create());
            //castle.OnLog += new LogEventHandler(castle_OnLog);
            //castle.OnError += new ErrorLogEventHandler(castle_OnError);
            //IUserService service = castle.GetService<IUserService>();

            //IList<ServiceInfo> list = castle.GetService<IStatusService>().GetServiceInfoList();
            //var str = service.GetUserID();

            //service.GetUsers();
            //service.GetDictUsers();

            //try
            //{
            //    int userid;
            //    var user = service.GetUserInfo("maoyong", out userid);
            //    user = service.GetUserInfo("maoyong", out userid);
            //    user = service.GetUserInfo("maoyong", out userid);
            //    user = service.GetUserInfo("maoyong", out userid);
            //    user = service.GetUserInfo("maoyong", out userid);

            //    service.SetUser(null, ref userid);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //for (int i = 0; i < count; i++)
            //{
            //    try
            //    {
            //        Thread thread = new Thread(DoWork);
            //        thread.Name = string.Format("Thread-->{0}", i);
            //        thread.IsBackground = true;
            //        thread.Start(service);
            //    }
            //    catch (Exception ex)
            //    {
            //        //WriteMessage(msg);
            //        castle_OnError(ex);
            //    }
            //}

            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(DoWork1);
                thread.Start();
            }

            //DoWork1();
            Console.ReadKey();
        }

        static void DoWork1()
        {
            while (true)
            {
                try
                {
                    //var users = CastleFactory.Create().DiscoverChannel<IUserService>().GetUsers();
                    //Console.WriteLine(users[0].Description);

                    int length = 1;
                    UserInfo user;
                    CastleFactory.Create().GetChannel<IUserService>().GetUserInfo("maoyong", ref length, out user);
                    Console.WriteLine(user.Description);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(1000);
            }
        }

        static void Program_OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        static void castle_OnError(Exception error)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + error.Message;
            if (error.InnerException != null)
            {
                message += "\r\n������Ϣ => " + error.InnerException.Message;
            }
            lock (syncobj)
            {
                if (error is WarningException)
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(message);
            }
        }

        static void castle_OnLog(string log, LogType type)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + log;
            lock (syncobj)
            {
                if (type == LogType.Error)
                    System.Console.ForegroundColor = ConsoleColor.Red;
                else if (type == LogType.Warning)
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine(message);
            }
        }

        static void DoWork(object value)
        {
            IUserService service = value as IUserService;
            while (true)
            {
                Stopwatch watch = Stopwatch.StartNew();
                try
                {
                    //int userid = service.GetUserID();
                    //UserInfo info = service.GetUserInfo("maoyong_" + new Random().Next(10000000), out userid);
                    //UserInfo info = service.GetUserInfo("maoyong", out userid);

                    var users = service.GetUsers();

                    if (users == null)
                    {
                        string msg = string.Format("�̣߳�{0} ��ʱ��{1} ms ����Ϊnull", Thread.CurrentThread.Name, watch.ElapsedMilliseconds);
                        //WriteMessage(msg);
                        castle_OnLog(msg, LogType.Error);
                    }
                    else
                    {
                        string msg = string.Format("�̣߳�{0} ��ʱ��{1} ms ���ݣ�{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, users.Count); //info.Description
                        //WriteMessage(msg);
                        castle_OnLog(msg, LogType.Information);
                    }
                }
                catch (Exception ex)
                {
                    string msg = string.Format("�̣߳�{0} ��ʱ��{1} ms �쳣��{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, ex.Message);
                    //WriteMessage(msg);
                    castle_OnLog(msg, LogType.Error);
                }

                Thread.Sleep(10);
            }
        }
    }
}
