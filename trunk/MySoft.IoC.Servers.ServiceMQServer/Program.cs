using System;
using System.Collections.Generic;
using System.Text;
using MySoft.IoC;
using MySoft.Remoting;
using MySoft.IoC.Dll;
using System.Threading;
using System.Diagnostics;

namespace MySoft.IoC.Servers.ServiceMQServer
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

            CastleFactory.Create().OnLog += new LogEventHandler(mq_OnLog);
            IUserService service = CastleFactory.Create().GetService<IUserService>();

            int count = 1;

            for (int i = 0; i < count; i++)
            {
                Thread thread = new Thread(DoWork);
                thread.Name = string.Format("Thread-->{0}", i);
                thread.IsBackground = true;
                thread.Start(service);
            }

            Console.ReadKey();
        }

        static void mq_OnLog(string log)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + log;
            Console.WriteLine(message);
        }

        static void mq_OnError(Exception exception)
        {
            string message = "[" + DateTime.Now.ToString() + "] " + exception.Message;
            Console.WriteLine(message);
        }

        static void DoWork(object value)
        {
            IUserService service = value as IUserService;
            while (true)
            {
                Stopwatch watch = Stopwatch.StartNew();
                try
                {
                    UserInfo info = service.GetUserInfo("maoyong_" + new Random().Next(10000000));

                    if (info == null)
                    {
                        string msg = string.Format("�̣߳�{0} ��ʱ��{1} ms ����Ϊnull", Thread.CurrentThread.Name, watch.ElapsedMilliseconds);
                        //WriteMessage(msg);
                        Console.WriteLine(msg);
                    }
                    else
                    {
                        string msg = string.Format("�̣߳�{0} ��ʱ��{1} ms ���ݣ�{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, info.Description);
                        //WriteMessage(msg);
                        Console.WriteLine(msg);
                    }
                }
                catch (Exception ex)
                {
                    string msg = string.Format("�̣߳�{0} ��ʱ��{1} ms �쳣��{2}", Thread.CurrentThread.Name, watch.ElapsedMilliseconds, ex.Message);
                    //WriteMessage(msg);
                    Console.WriteLine(msg);
                }

                Thread.Sleep(10);
            }
        }
    }
}
