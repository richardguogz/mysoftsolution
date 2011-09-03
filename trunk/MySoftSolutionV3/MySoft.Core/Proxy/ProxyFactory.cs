using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MySoft
{
    /// <summary>
    /// ������
    /// </summary>
    public class ProxyFactory
    {
        private static ProxyFactory instance;
        private static Object lockObj = new Object();

        /// <summary>
        /// ��ȡһ��ʵ��
        /// </summary>
        /// <returns></returns>
        public static ProxyFactory GetInstance()
        {
            if (instance == null)
            {
                CreateInstance();
            }

            return instance;
        }

        private static void CreateInstance()
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = new ProxyFactory();
                }
            }
        }

        /// <summary>
        /// ����һ���������
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="objType"></param>
        /// <param name="isObjInterface"></param>
        /// <returns></returns>
        public Object Create(IProxyInvocationHandler handler, Type objType, bool isObjInterface)
        {
            if (isObjInterface)
            {
                return DynamicProxy.NewInstance(AppDomain.CurrentDomain, new Type[] { objType }, handler);
            }
            else
            {
                return DynamicProxy.NewInstance(AppDomain.CurrentDomain, objType.GetInterfaces(), handler);
            }
        }

        /// <summary>
        /// ����һ���������
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public Object Create(IProxyInvocationHandler handler, Type objType)
        {
            return Create(handler, objType, false);
        }
    }
}