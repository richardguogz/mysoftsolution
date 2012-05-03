using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using MySoft.Logger;
using System.Collections;
using MySoft.Threading;

namespace MySoft.Web
{
    /// <summary>
    /// ������
    /// </summary>
    public interface IUpdateItem
    {
        /// <summary>
        /// ���Լ��(��λ:����)
        /// </summary>
        int RetryInterval { get; set; }

        /// <summary>
        /// ��������ҳ��
        /// </summary>
        void Update();

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        void Update(DateTime updateTime);

        /// <summary>
        /// �첽���£�TimeSpan��ʾ�ӳٸ��µ�ʱ�䣬��������ʧЧ
        /// </summary>
        void Update(TimeSpan timeSpan);

        /// <summary>
        /// ��ǰ�Ƿ���Ը���
        /// </summary>
        bool NeedUpdate(DateTime updateTime);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="domainUri"></param>
        void SetDomain(string domainUri);
    }

    /// <summary>
    /// ��̬ҳ����ӿ�
    /// </summary>
    public interface IStaticPageItem : IUpdateItem
    {
        /// <summary>
        /// ��ʼ����
        /// </summary>
        event ExecuteEventHandler OnStart;

        /// <summary>
        /// ��������
        /// </summary>
        event ExecuteEventHandler OnComplete;

        /// <summary>
        /// ����ʱ�Ļص�
        /// </summary>
        event CallbackEventHandler Callback;

        /// <summary>
        /// ��̬ҳ��������
        /// </summary>
        IUpdateDependency StaticPageDependency { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        Encoding OutEncoding { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        Encoding InEncoding { get; set; }

        /// <summary>
        /// �Ƿ�ΪԶ��ҳ��
        /// </summary>
        bool IsRemote { get; set; }
    }

    /// <summary>
    /// ִ������ʱ��ί��
    /// </summary>
    /// <param name="createTime"></param>
    /// <param name="dynamicurl"></param>
    /// <param name="staticurl"></param>
    public delegate void ExecuteEventHandler(DateTime createTime, string dynamicurl, string staticurl);

    /// <summary>
    /// ����ҳ��ʱί��
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public delegate string CallbackEventHandler(string content);

    /// <summary>
    /// ����ֵ�����ί��
    /// </summary>
    /// <returns></returns>
    public delegate object[] GetResultEventHandler(object state);

    /// <summary>
    /// ͨ�þ�̬ҳ����
    /// </summary>
    public sealed class SingleStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// �ص�
        /// </summary>
        public event ExecuteEventHandler OnStart;

        /// <summary>
        /// ��������
        /// </summary>
        public event ExecuteEventHandler OnComplete;

        /// <summary>
        /// ����ʱ�Ļص�
        /// </summary>
        public event CallbackEventHandler Callback;

        #region ����

        private string query;
        private string templatePath;
        private string savePath;
        private string validateString;
        private bool updateComplete;

        private IUpdateDependency staticPageDependency;
        /// <summary>
        /// ��̬ҳ��������
        /// </summary>
        public IUpdateDependency StaticPageDependency
        {
            get { return staticPageDependency; }
            set { staticPageDependency = value; }
        }

        private Encoding outEncoding;
        /// <summary>
        /// �������
        /// </summary>
        public Encoding OutEncoding
        {
            get { return outEncoding; }
            set { outEncoding = value; }
        }

        private Encoding inEncoding;
        /// <summary>
        /// �������
        /// </summary>
        public Encoding InEncoding
        {
            get { return inEncoding; }
            set { inEncoding = value; }
        }

        private bool isRemote;
        /// <summary>
        /// �Ƿ�ΪԶ��ҳ��
        /// </summary>
        public bool IsRemote
        {
            get { return isRemote; }
            set { isRemote = value; }
        }

        private int retryInterval = 5;
        /// <summary>
        /// ���Լ��(��λ:����)
        /// </summary>
        public int RetryInterval
        {
            get { return retryInterval; }
            set { retryInterval = value; }
        }

        private int inMinutes = 5;
        /// <summary>
        /// ���ٷ���֮�ڲ�����
        /// </summary>
        public int InMinutes
        {
            get { return inMinutes; }
            set { inMinutes = value; }
        }

        /// <summary>
        /// ��ǰ�Ƿ���Ը���
        /// </summary>
        bool IUpdateItem.NeedUpdate(DateTime updateTime)
        {
            //���û������ɣ��򷵻�
            if (!updateComplete) return false;

            //�жϲ����Ƿ��Ѿ��ﵽ����ֵ
            return staticPageDependency.HasUpdate(updateTime, inMinutes);
        }

        #endregion

        #region ���캯��

        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        private SingleStaticPageItem()
        {
            this.outEncoding = Encoding.UTF8;
            this.inEncoding = Encoding.UTF8;
            this.staticPageDependency = new SlidingUpdateTime(new TimeSpan(1, 0, 0));
            this.isRemote = false;
            this.updateComplete = true;
        }

        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        /// <param name="templatePath">ģ��·��</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="validateString">��֤�ַ���</param>
        public SingleStaticPageItem(string templatePath, string savePath, string validateString)
            : this()
        {
            this.templatePath = templatePath;
            this.savePath = savePath;
            this.validateString = validateString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath">ģ��·��</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="createSpan">���ɼ��ʱ��</param>
        public SingleStaticPageItem(string templatePath, string savePath, string validateString, IUpdateDependency staticPageDependency)
            : this(templatePath, savePath, validateString)
        {
            this.staticPageDependency = staticPageDependency;
        }

        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        /// <param name="templatePath">ģ��ҳ·��</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="createSpan">���ɼ��ʱ��</param>
        /// <param name="validateString">��֤�ַ���</param>
        /// <param name="query">��ѯ�����ַ���</param>
        public SingleStaticPageItem(string templatePath, string query, string savePath, string validateString)
            : this(templatePath, savePath, validateString)
        {
            this.query = query;
        }


        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        /// <param name="templatePath">ģ��ҳ·��</param>
        /// <param name="query">��ѯ�����ַ���</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="createSpan">���ɼ��ʱ��</param>
        /// <param name="validateString">��֤�ַ�</param>
        public SingleStaticPageItem(string templatePath, string query, string savePath, string validateString, IUpdateDependency staticPageDependency)
            : this(templatePath, query, savePath, validateString)
        {
            this.staticPageDependency = staticPageDependency;
        }

        #endregion

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="domainUri"></param>
        public void SetDomain(string domainUri)
        {
            //�����Զ������������
            if (isRemote) return;

            isRemote = true;
            templatePath = string.Concat(domainUri.TrimEnd('/'), "/", templatePath.TrimStart(new char[] { '~', '/' }));
        }

        /// <summary>
        /// ��������ҳ��
        /// </summary>
        public void Update()
        {
            (this as IUpdateItem).Update(DateTime.MaxValue);
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        void IUpdateItem.Update(DateTime updateTime)
        {
            updateComplete = false;

            string dynamicurl = templatePath;
            string staticurl = savePath;
            if (!string.IsNullOrEmpty(query))
                dynamicurl = string.Format("{0}?{1}", dynamicurl, query);

            var item = new UpdateItem { DynamicUrl = dynamicurl, StaticPath = staticurl };

            try
            {
                string content = null;

                if (isRemote)
                    content = StaticPageManager.GetRemotePageString(item.DynamicUrl, inEncoding, validateString);
                else
                    content = StaticPageManager.GetLocalPageString(item.Path, item.Query, inEncoding, validateString);

                DateTime createTime = DateTime.Now;

                //��ʼ����
                if (OnStart != null)
                {
                    try
                    {
                        OnStart(createTime, item.DynamicUrl, item.StaticUrl);
                    }
                    catch (Exception ex)
                    {
                    };
                }

                //����ʱ�ص�
                if (Callback != null)
                {
                    try
                    {
                        content = Callback(content);
                    }
                    catch (Exception ex)
                    {
                    };
                }

                string extension = Path.GetExtension(item.StaticPath);
                if (extension != null && extension.ToLower() == ".js")
                {
                    //���뾲̬ҳ����Ԫ��
                    content = string.Format("{3}\r\n\r\n//<!-- ���ɷ�ʽ���������� -->\r\n//<!-- ����ʱ�䣺{0} -->\r\n//<!-- ��̬URL��{1} -->\r\n//<!-- ��̬URL��{2} -->",
                                        createTime.ToString("yyyy-MM-dd HH:mm:ss"), item.DynamicUrl, item.StaticUrl, content.Trim());
                }
                else
                {
                    //���뾲̬ҳ����Ԫ��
                    content = string.Format("{3}\r\n\r\n<!-- ���ɷ�ʽ���������� -->\r\n<!-- ����ʱ�䣺{0} -->\r\n<!-- ��̬URL��{1} -->\r\n<!-- ��̬URL��{2} -->",
                                        createTime.ToString("yyyy-MM-dd HH:mm:ss"), item.DynamicUrl, item.StaticUrl, content.Trim());
                }

                StaticPageManager.SaveFile(content, item.StaticPath, outEncoding);

                //��������
                if (OnComplete != null)
                {
                    try
                    {
                        OnComplete(createTime, item.DynamicUrl, item.StaticUrl);
                    }
                    catch (Exception ex)
                    {
                    };
                }

                staticPageDependency.UpdateSuccess = true;
            }
            catch (Exception ex)
            {
                StaticPageManager.SaveError(new StaticPageException(string.Format("�������ɾ�̬�ļ�ʧ�ܣ���{2}�����Ӻ��������ɣ�\r\n{0} => {1}",
                    item.DynamicUrl, item.StaticUrl, retryInterval), ex));

                //����������������ִ��
                staticPageDependency.UpdateSuccess = false;
            }
            finally
            {
                //����������ʱ��
                //staticPageDependency.LastUpdateTime = updateTime;
            }

            //ȫ�����ɳɹ�������������ʱ��
            if (updateTime == DateTime.MaxValue)
                staticPageDependency.LastUpdateTime = DateTime.Now;
            else
                staticPageDependency.LastUpdateTime = updateTime;

            if (!staticPageDependency.UpdateSuccess)
            {
                //ȫ�����ɳɹ�������������ʱ��,����������5������������
                staticPageDependency.LastUpdateTime = staticPageDependency.LastUpdateTime.AddMinutes(retryInterval);
            }

            updateComplete = true;
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        public void Update(TimeSpan timeSpan)
        {
            ManagedThreadPool.QueueUserWorkItem(state =>
            {
                ArrayList arr = state as ArrayList;
                IStaticPageItem item = (IStaticPageItem)arr[0];
                TimeSpan span = (TimeSpan)arr[1];
                Thread.Sleep(span);

                item.Update();
            }, new ArrayList { this, timeSpan });
        }
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    public sealed class StaticPageParamInfo
    {
        private object state;
        private string paramName;
        /// <summary>
        /// ������
        /// </summary>
        internal string ParamName
        {
            get { return paramName; }
        }

        private GetResultEventHandler getResult;
        /// <summary>
        /// ��ȡֵί��
        /// </summary>
        internal object[] GetResult()
        {
            if (getResult != null)
                return getResult(state);
            else
                return new object[0];
        }

        public StaticPageParamInfo(string paramName, object[] values)
        {
            this.paramName = paramName;
            this.getResult = delegate(object state) { return values; };
        }

        public StaticPageParamInfo(string paramName, GetResultEventHandler getResult)
        {
            this.paramName = paramName;
            this.getResult = getResult;
        }

        public StaticPageParamInfo(string paramName, GetResultEventHandler getResult, object state)
            : this(paramName, getResult)
        {
            this.state = state;
        }
    }

    /// <summary>
    /// ��ҳ��̬ҳ����
    /// </summary>
    public sealed class ParamStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// �ص�
        /// </summary>
        public event ExecuteEventHandler OnStart;

        /// <summary>
        /// ��������
        /// </summary>
        public event ExecuteEventHandler OnComplete;

        /// <summary>
        /// ����ʱ�Ļص�
        /// </summary>
        public event CallbackEventHandler Callback;

        #region ����

        private string templatePath;
        private string savePath;
        private string query;
        private string validateString;
        private bool updateComplete;
        private List<UpdateItem> updateErrorList;
        private IUpdateDependency staticPageDependency;
        /// <summary>
        /// ��̬ҳ��������
        /// </summary>
        public IUpdateDependency StaticPageDependency
        {
            get { return staticPageDependency; }
            set { staticPageDependency = value; }
        }

        private StaticPageParamInfo[] paramInfos;
        /// <summary>
        /// ��̬ҳ����
        /// </summary>
        public StaticPageParamInfo[] ParamInfos
        {
            get { return paramInfos; }
            set { paramInfos = value; }
        }

        private Encoding outEncoding;
        /// <summary>
        /// �������
        /// </summary>
        public Encoding OutEncoding
        {
            get { return outEncoding; }
            set { outEncoding = value; }
        }

        private Encoding inEncoding;
        /// <summary>
        /// �������
        /// </summary>
        public Encoding InEncoding
        {
            get { return inEncoding; }
            set { inEncoding = value; }
        }

        private bool isRemote;
        /// <summary>
        /// �Ƿ�ΪԶ��ҳ��
        /// </summary>
        public bool IsRemote
        {
            get { return isRemote; }
            set { isRemote = value; }
        }

        private int retryInterval = 5;
        /// <summary>
        /// ���Լ��(��λ:����)
        /// </summary>
        public int RetryInterval
        {
            get { return retryInterval; }
            set { retryInterval = value; }
        }

        private int inMinutes = 5;
        /// <summary>
        /// ���ٷ���֮�ڲ�����
        /// </summary>
        public int InMinutes
        {
            get { return inMinutes; }
            set { inMinutes = value; }
        }

        /// <summary>
        /// Ĭ���߳���
        /// </summary>
        private const int DEFAULT_THREAD = 1;

        /// <summary>
        /// ��С�߳���
        /// </summary>
        private const int MIN_THREAD = 1;

        /// <summary>
        /// ����߳���
        /// </summary>
        private const int MAX_THREAD = 10;

        private int threadCount = DEFAULT_THREAD;
        /// <summary>
        /// ����ҳ����߳�����Ĭ��Ϊ1
        /// </summary>
        public int ThreadCount
        {
            get { return threadCount; }
            set
            {
                if (threadCount > MAX_THREAD)
                    throw new WebException(string.Format("�����߳������ܴ���{0}��", MAX_THREAD));

                if (threadCount < MIN_THREAD)
                    throw new WebException(string.Format("�����߳�������С��{0}��", MIN_THREAD));

                threadCount = value;
            }
        }

        /// <summary>
        /// ��ǰ�Ƿ���Ը���
        /// </summary>
        bool IUpdateItem.NeedUpdate(DateTime updateTime)
        {
            //���û������ɣ��򷵻�
            if (!updateComplete) return false;

            //�жϲ����Ƿ��Ѿ��ﵽ����ֵ
            return staticPageDependency.HasUpdate(updateTime, inMinutes);
        }

        #endregion

        #region ���캯��

        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        private ParamStaticPageItem()
        {
            this.outEncoding = Encoding.UTF8;
            this.inEncoding = Encoding.UTF8;
            this.staticPageDependency = new SlidingUpdateTime(new TimeSpan(1, 0, 0));
            this.isRemote = false;
            this.updateComplete = true;
            this.updateErrorList = new List<UpdateItem>();
        }

        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        /// <param name="templatePath">ģ��ҳ·��</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="createSpan">���ɼ��ʱ��</param>
        /// <param name="validateString">��֤�ַ���</param>
        /// <param name="query">��ѯ�����ַ���</param>
        public ParamStaticPageItem(string templatePath, string query, string savePath, string validateString, params StaticPageParamInfo[] paramInfos)
            : this()
        {
            this.templatePath = templatePath;
            this.query = query;
            this.savePath = savePath;
            this.validateString = validateString;
            this.paramInfos = paramInfos;
        }


        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        /// <param name="templatePath">ģ��ҳ·��</param>
        /// <param name="query">��ѯ�����ַ���</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="createSpan">���ɼ��ʱ��</param>
        /// <param name="validateString">��֤�ַ�</param>
        public ParamStaticPageItem(string templatePath, string query, string savePath, string validateString, IUpdateDependency staticPageDependency, params StaticPageParamInfo[] paramInfos)
            : this(templatePath, query, savePath, validateString, paramInfos)
        {
            this.staticPageDependency = staticPageDependency;
        }

        #endregion

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="domainUri"></param>
        public void SetDomain(string domainUri)
        {
            //�����Զ������������
            if (isRemote) return;

            isRemote = true;
            templatePath = string.Concat(domainUri.TrimEnd('/'), "/", templatePath.TrimStart(new char[] { '~', '/' }));
        }

        /// <summary>
        /// ��������ҳ��
        /// </summary>
        public void Update()
        {
            (this as IUpdateItem).Update(DateTime.MaxValue);
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        void IUpdateItem.Update(DateTime updateTime)
        {
            updateComplete = false;

            //���δ��ȫ�����ɳɹ���������ʧ���б�
            if (updateTime != DateTime.MaxValue && !staticPageDependency.UpdateSuccess)
            {
                var errors = Update(updateTime, updateErrorList);

                //��ӵ��쳣�б�
                if (errors.Count > 0)
                {
                    updateErrorList.AddRange(errors);
                }
            }
            else
            {
                var dictPosition = new Dictionary<string, int>();
                var dictValues = new Dictionary<string, IList<object>>();
                foreach (var paramInfo in paramInfos)
                {
                    try
                    {
                        if (!dictValues.ContainsKey(paramInfo.ParamName))
                        {
                            var objlist = new List<object>(paramInfo.GetResult());
                            dictValues[paramInfo.ParamName] = objlist;
                            dictPosition[paramInfo.ParamName] = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        StaticPageManager.SaveError(new StaticPageException(string.Format("��ȡ����{0}��ֵ����URL��{1}��", paramInfo.ParamName, templatePath), ex));
                        return;
                    }
                }

                try
                {
                    int count = GetPageCount(dictValues);
                    var items = new List<UpdateItem>();

                    string dynamicurl = templatePath;
                    string staticurl = savePath;
                    if (!string.IsNullOrEmpty(query))
                        dynamicurl = string.Format("{0}?{1}", dynamicurl, query);

                    for (int index = 0; index < count; index++)
                    {
                        var tmpDynamicUrl = dynamicurl;
                        var tmpStaticUrl = staticurl;

                        //���ɶ�Ӧ��url
                        foreach (string key in dictValues.Keys)
                        {
                            var value = dictValues[key][dictPosition[key]].ToString();

                            //��̬��ַ
                            tmpDynamicUrl = tmpDynamicUrl.Replace(key, value);

                            //��̬��ַ
                            tmpStaticUrl = tmpStaticUrl.Replace(key, value);
                        }

                        //��ӵ�������
                        items.Add(new UpdateItem { DynamicUrl = tmpDynamicUrl, StaticPath = tmpStaticUrl });

                        //��λ����
                        SetPosition(dictPosition, dictValues, dictPosition.Keys.Count - 1);
                    }

                    if (items.Count > 0)
                    {
                        //����ҳ��, ֻ��һ���߳�ʱ
                        if (threadCount == 1)
                        {
                            var errors = Update(updateTime, items);

                            //��ӵ��쳣�б�
                            if (errors.Count > 0)
                            {
                                updateErrorList.AddRange(errors);
                            }
                        }
                        else
                        {
                            int pageSize = (int)Math.Ceiling(items.Count / (threadCount * 1.0));

                            //��ҳ���ɾ�̬ҳ
                            var events = new AutoResetEvent[threadCount];

                            for (int index = 0; index < threadCount; index++)
                            {
                                events[index] = new AutoResetEvent(false);

                                var updateItems = new List<UpdateItem>();
                                if (items.Count >= (index + 1) * pageSize)
                                    updateItems = items.GetRange(index * pageSize, pageSize);
                                else
                                    updateItems = items.GetRange(index * pageSize, items.Count - (index * pageSize));

                                var thread = new Thread(state =>
                                {
                                    if (state == null) return;

                                    var arr = state as ArrayList;
                                    var list = arr[0] as List<UpdateItem>;
                                    var reset = arr[1] as AutoResetEvent;

                                    var errors = Update(updateTime, list);

                                    //��ӵ��쳣�б�
                                    if (errors.Count > 0)
                                    {
                                        lock (updateErrorList)
                                        {
                                            updateErrorList.AddRange(errors);
                                        }
                                    }

                                    reset.Set();
                                });

                                //�����߳�
                                thread.Start(new ArrayList { updateItems, events[index] });
                            }

                            //�ȴ�������Ӧ
                            WaitHandle.WaitAll(events);
                        }
                    }
                }
                catch (Exception ex)
                {
                    StaticPageManager.SaveError(new StaticPageException("����URL�б���� Error => " + ex.Message, ex));
                }
            }

            //ȫ�����ɳɹ�������������ʱ��
            if (updateTime == DateTime.MaxValue)
                staticPageDependency.LastUpdateTime = DateTime.Now;
            else
                staticPageDependency.LastUpdateTime = updateTime;

            if (!staticPageDependency.UpdateSuccess)
            {
                //ȫ�����ɳɹ�������������ʱ��,����������5������������
                staticPageDependency.LastUpdateTime = staticPageDependency.LastUpdateTime.AddMinutes(retryInterval);
            }

            //�������
            updateComplete = true;
        }

        /// <summary>
        /// ��ȡ��ѭ������
        /// </summary>
        /// <returns></returns>
        private int GetPageCount(IDictionary<string, IList<object>> dictValues)
        {
            int count = 1;
            foreach (string paramName in dictValues.Keys)
            {
                count *= dictValues[paramName].Count;
            }
            return count;
        }

        /// <summary>
        /// ���ö�Ӧ������ֵ
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="values"></param>
        /// <param name="index"></param>
        private void SetPosition(IDictionary<string, int> positions, IDictionary<string, IList<object>> values, int index)
        {
            if (index < 0) return;
            string key = new List<string>(positions.Keys)[index];
            if (positions[key] < values[key].Count - 1)
            {
                positions[key]++;
            }
            else
            {
                positions[key] = 0;
                SetPosition(positions, values, --index);
            }
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        private IList<UpdateItem> Update(DateTime updateTime, IList<UpdateItem> items)
        {
            var errorList = new List<UpdateItem>();

            try
            {
                foreach (var item in items)
                {
                    try
                    {
                        string content = null;
                        if (isRemote)
                            content = StaticPageManager.GetRemotePageString(item.DynamicUrl, inEncoding, validateString);
                        else
                            content = StaticPageManager.GetLocalPageString(item.Path, item.Query, inEncoding, validateString);

                        DateTime createTime = DateTime.Now;

                        //��ʼ����
                        if (OnStart != null)
                        {
                            try
                            {
                                OnStart(createTime, item.DynamicUrl, item.StaticUrl);
                            }
                            catch { };
                        }

                        //����ʱ�ص�
                        if (Callback != null)
                        {
                            try
                            {
                                content = Callback(content);
                            }
                            catch { };
                        }

                        string extension = Path.GetExtension(item.StaticPath);
                        if (extension != null && extension.ToLower() == ".js")
                        {
                            //���뾲̬ҳ����Ԫ��
                            content = string.Format("{3}\r\n\r\n//<!-- ���ɷ�ʽ���������� -->\r\n//<!-- ����ʱ�䣺{0} -->\r\n//<!-- ��̬URL��{1} -->\r\n//<!-- ��̬URL��{2} -->",
                                                createTime.ToString("yyyy-MM-dd HH:mm:ss"), item.DynamicUrl, item.StaticUrl, content.Trim());
                        }
                        else
                        {
                            //���뾲̬ҳ����Ԫ��
                            content = string.Format("{3}\r\n\r\n<!-- ���ɷ�ʽ���������� -->\r\n<!-- ����ʱ�䣺{0} -->\r\n<!-- ��̬URL��{1} -->\r\n<!-- ��̬URL��{2} -->",
                                                createTime.ToString("yyyy-MM-dd HH:mm:ss"), item.DynamicUrl, item.StaticUrl, content.Trim());
                        }

                        StaticPageManager.SaveFile(content, item.StaticPath, outEncoding);

                        //��������
                        if (OnComplete != null)
                        {
                            try
                            {
                                OnComplete(createTime, item.DynamicUrl, item.StaticUrl);
                            }
                            catch { };
                        }

                        //��״̬Ϊ���ɳɹ�
                        item.UpdateSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        //��״̬Ϊ����ʧ��
                        item.UpdateSuccess = false;

                        StaticPageManager.SaveError(new StaticPageException(string.Format("�������ɾ�̬�ļ�ʧ�ܣ��ȴ��������ɣ�\r\n{0} => {1}",
                                item.DynamicUrl, item.StaticUrl), ex));
                    }
                }

                //δȫ�����³ɹ�
                if (items.Any(p => !p.UpdateSuccess))
                {
                    errorList = items.Where(p => !p.UpdateSuccess).ToList();
                    string html = string.Join("\r\n", errorList.Select(p => string.Format("{0} => {1}", p.DynamicUrl, p.StaticUrl)).ToArray());
                    throw new StaticPageException(string.Format("�������ɡ�{0}������̬ҳʧ�ܣ���{1}�����Ӻ��������ɣ�\r\n{2}",
                        errorList.Count, retryInterval, html));
                }

                staticPageDependency.UpdateSuccess = true;
            }
            catch (Exception ex)
            {
                //����������������ִ��
                if (ex is StaticPageException)
                    StaticPageManager.SaveError(ex as StaticPageException);

                staticPageDependency.UpdateSuccess = false;
            }
            finally
            {
                //����������ʱ��
                //staticPageDependency.LastUpdateTime = updateTime;
            }

            return errorList;
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        public void Update(TimeSpan timeSpan)
        {
            ManagedThreadPool.QueueUserWorkItem(state =>
            {
                TimeSpan span = (TimeSpan)state;
                Thread.Sleep(span);

                (this as IUpdateItem).Update(DateTime.MaxValue);
            }, timeSpan);
        }
    }
}
