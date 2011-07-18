using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

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
    }

    /// <summary>
    /// ��̬ҳ����ӿ�
    /// </summary>
    public interface IStaticPageItem : IUpdateItem
    {
        /// <summary>
        /// ��ʼ����
        /// </summary>
        event ExcutingEventHandler OnStart;

        /// <summary>
        /// ��������
        /// </summary>
        event ExcutingEventHandler OnEnd;

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
    public delegate void ExcutingEventHandler(DateTime createTime, string dynamicurl, string staticurl);

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
    public delegate object[] GetResultEventHandler(object[] arguments);

    /// <summary>
    /// ��ȡ��ʼ����ֵ��ί��
    /// </summary>
    /// <returns></returns>
    public delegate int BeginEndValueEventHandler();

    /// <summary>
    /// ͨ�þ�̬ҳ����
    /// </summary>
    public sealed class SingleStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// �ص�
        /// </summary>
        public event ExcutingEventHandler OnStart;

        /// <summary>
        /// ��������
        /// </summary>
        public event ExcutingEventHandler OnEnd;

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

        private int retryInterval = 10;
        /// <summary>
        /// ���Լ��(��λ:����)
        /// </summary>
        public int RetryInterval
        {
            get { return retryInterval; }
            set { retryInterval = value; }
        }

        /// <summary>
        /// ��ǰ�Ƿ���Ը���
        /// </summary>
        bool IUpdateItem.NeedUpdate(DateTime updateTime)
        {
            //���û������ɣ��򷵻�
            if (!updateComplete) return false;

            //�жϲ����Ƿ��Ѿ��ﵽ����ֵ
            return staticPageDependency.HasUpdate(updateTime);
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

            try
            {
                string content = null;

                if (isRemote)
                    content = StaticPageManager.GetRemotePageString(dynamicurl, inEncoding, validateString);
                else
                {
                    content = StaticPageManager.GetLocalPageString(dynamicurl, query, inEncoding, validateString);

                    if (!string.IsNullOrEmpty(query))
                        dynamicurl = string.Format("{0}?{1}", dynamicurl, query);
                }

                DateTime createTime = DateTime.Now;

                //��ʼ����
                if (OnStart != null)
                {
                    try { OnStart(createTime, dynamicurl, RemoveRootPath(staticurl)); }
                    catch { };
                }

                //����ʱ�ص�
                if (Callback != null)
                {
                    try { content = Callback(content); }
                    catch { };
                }

                string extension = Path.GetExtension(staticurl);
                if (extension != null && extension.ToLower() == ".js")
                {
                    //���뾲̬ҳ����Ԫ��
                    content = string.Format("{3}\r\n\r\n//<!-- ���ɷ�ʽ���������� -->\r\n//<!-- ����ʱ�䣺{0} -->\r\n//<!-- ��̬URL��{1} -->\r\n//<!-- ��̬URL��{2} -->",
                                        createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                }
                else
                {
                    //���뾲̬ҳ����Ԫ��
                    content = string.Format("{3}\r\n\r\n<!-- ���ɷ�ʽ���������� -->\r\n<!-- ����ʱ�䣺{0} -->\r\n<!-- ��̬URL��{1} -->\r\n<!-- ��̬URL��{2} -->",
                                        createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                }

                StaticPageManager.SaveFile(content, staticurl, outEncoding);

                //��������
                if (OnEnd != null)
                {
                    try { OnEnd(createTime, dynamicurl, RemoveRootPath(staticurl)); }
                    catch { };
                }

                //ȫ�����ɳɹ�������������ʱ��
                if (updateTime == DateTime.MaxValue)
                    staticPageDependency.LastUpdateTime = DateTime.Now;
                else
                    staticPageDependency.LastUpdateTime = updateTime;

                staticPageDependency.UpdateSuccess = true;
            }
            catch (Exception ex)
            {
                StaticPageManager.SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", RemoveRootPath(staticurl)));
                //����������������ִ��

                //ȫ�����ɳɹ�������������ʱ��,����������10������������
                if (updateTime == DateTime.MaxValue)
                    staticPageDependency.LastUpdateTime = DateTime.Now.AddMinutes(retryInterval);
                else
                    staticPageDependency.LastUpdateTime = updateTime.AddMinutes(retryInterval);

                staticPageDependency.UpdateSuccess = false;
            }
            finally
            {
                //����������ʱ��
                //staticPageDependency.LastUpdateTime = updateTime;
            }

            updateComplete = true;
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        public void Update(TimeSpan timeSpan)
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                TimeSpan span = (TimeSpan)obj;
                Thread.Sleep(span);

                (this as IUpdateItem).Update(DateTime.MaxValue);
            }, timeSpan);
        }

        /// <summary>
        /// ȥ����Ŀ¼
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string RemoveRootPath(string path)
        {
            try
            {
                return path.Replace(AppDomain.CurrentDomain.BaseDirectory, "/").Replace("\\", "/").Replace("//", "/");
            }
            catch
            {
                return path;
            }
        }
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    public sealed class StaticPageParamInfo
    {
        private object[] arguments;
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
        public GetResultEventHandler GetResult
        {
            get { return getResult; }
        }

        /// <summary>
        /// ί�в���
        /// </summary>
        public object[] Arguments
        {
            get { return arguments; }
        }

        public StaticPageParamInfo(string paramName, int startPage, int endPage)
        {
            this.paramName = paramName;
            List<object> list = new List<object>();
            for (int index = startPage; index <= endPage; index++)
            {
                list.Add(index);
            }
            this.getResult = delegate(object[] args) { return list.ToArray(); };
        }

        public StaticPageParamInfo(string paramName, BeginEndValueEventHandler beginValue, BeginEndValueEventHandler endValue)
            : this(paramName, beginValue(), endValue())
        { }

        public StaticPageParamInfo(string paramName, object[] values)
        {
            this.paramName = paramName;
            this.getResult = delegate(object[] args) { return values; };
        }

        public StaticPageParamInfo(string paramName, GetResultEventHandler getResult, params object[] arguments)
        {
            this.paramName = paramName;
            this.getResult = getResult;
            this.arguments = arguments;
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
        public event ExcutingEventHandler OnStart;

        /// <summary>
        /// ��������
        /// </summary>
        public event ExcutingEventHandler OnEnd;

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
        private IList<string> updateErrorList;

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

        /// <summary>
        /// ��ǰ�Ƿ���Ը���
        /// </summary>
        bool IUpdateItem.NeedUpdate(DateTime updateTime)
        {
            //���û������ɣ��򷵻�
            if (!updateComplete) return false;

            //�жϲ����Ƿ��Ѿ��ﵽ����ֵ
            return staticPageDependency.HasUpdate(updateTime);
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
            this.updateErrorList = new List<string>();
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

        //�������ڸ��µ��ֵ���Ϣ
        private Dictionary<string, IList<object>> dict;
        private Dictionary<string, int> dictPosition;

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
            if (updateTime == DateTime.MaxValue)
            {
                updateErrorList.Clear();
            }

            try
            {
                dict = new Dictionary<string, IList<object>>();
                dictPosition = new Dictionary<string, int>();

                foreach (StaticPageParamInfo paramInfo in paramInfos)
                {
                    if (!dict.ContainsKey(paramInfo.ParamName))
                    {
                        List<object> objlist = new List<object>();
                        try
                        {
                            objlist = new List<object>(paramInfo.GetResult(paramInfo.Arguments));
                        }
                        catch
                        {
                            try
                            {
                                objlist = new List<object>(paramInfo.GetResult(paramInfo.Arguments));
                            }
                            catch { }
                        }
                        dict.Add(paramInfo.ParamName, objlist);
                        dictPosition.Add(paramInfo.ParamName, 0);
                    }
                }

                int count = GetPageCount(dict);
                bool allUpdateSuccess = true;

                for (int index = 0; index < count; index++)
                {
                    string dynamicurl = templatePath;
                    string staticurl = GetRealPath(savePath);
                    string queryURL = dynamicurl;
                    string queryurl = GetRealPath(query);
                    if (!string.IsNullOrEmpty(queryurl))
                        queryURL = string.Format("{0}?{1}", dynamicurl, queryurl);

                    if (updateTime != DateTime.MaxValue && updateErrorList.Count > 0)
                    {
                        //�жϸ���ʧ�ܵ�url
                        if (!staticPageDependency.UpdateSuccess && !updateErrorList.Contains(queryURL))
                        {
                            SetPosition(dict.Keys.Count - 1);
                            continue;
                        }
                    }

                    try
                    {
                        string content = null;
                        if (isRemote)
                            content = StaticPageManager.GetRemotePageString(dynamicurl, inEncoding, validateString);
                        else
                            content = StaticPageManager.GetLocalPageString(dynamicurl, queryurl, inEncoding, validateString);

                        DateTime createTime = DateTime.Now;

                        //��ʼ����
                        if (OnStart != null)
                        {
                            try
                            {
                                OnStart(createTime, dynamicurl, RemoveRootPath(staticurl));
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

                        string extension = Path.GetExtension(staticurl);
                        if (extension != null && extension.ToLower() == ".js")
                        {
                            //���뾲̬ҳ����Ԫ��
                            content = string.Format("{3}\r\n\r\n//<!-- ���ɷ�ʽ���������� -->\r\n//<!-- ����ʱ�䣺{0} -->\r\n//<!-- ��̬URL��{1} -->\r\n//<!-- ��̬URL��{2} -->",
                                                createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                        }
                        else
                        {
                            //���뾲̬ҳ����Ԫ��
                            content = string.Format("{3}\r\n\r\n<!-- ���ɷ�ʽ���������� -->\r\n<!-- ����ʱ�䣺{0} -->\r\n<!-- ��̬URL��{1} -->\r\n<!-- ��̬URL��{2} -->",
                                                createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                        }

                        StaticPageManager.SaveFile(content, staticurl, outEncoding);

                        //��������
                        if (OnEnd != null)
                        {
                            try
                            {
                                OnEnd(createTime, dynamicurl, RemoveRootPath(staticurl));
                            }
                            catch { };
                        }

                        //�����ɳɹ���url�Ƴ��б�
                        if (updateErrorList.Contains(queryURL))
                        {
                            updateErrorList.Remove(queryURL);
                        }
                    }
                    catch (Exception ex)
                    {
                        StaticPageManager.SaveError(ex, string.Format("���ɾ�̬�ļ�{0}ʧ�ܣ�", RemoveRootPath(staticurl)));
                        //����������������ִ��

                        //�����ɳ����url�����б�
                        if (!updateErrorList.Contains(queryURL))
                        {
                            updateErrorList.Add(queryURL);
                        }

                        allUpdateSuccess = false;
                    }
                    finally
                    {
                        SetPosition(dict.Keys.Count - 1);
                    }
                }

                //δȫ�����³ɹ�
                if (!allUpdateSuccess)
                {
                    string html = string.Join("\r\n", updateErrorList.ToArray());
                    throw new Exception("��̬ҳδ��ȫ�����ɳɹ�����Ҫ�ӳ��������ɣ�" + "\r\n" + html);
                }

                //ȫ�����ɳɹ�������������ʱ��
                if (updateTime == DateTime.MaxValue)
                    staticPageDependency.LastUpdateTime = DateTime.Now;
                else
                    staticPageDependency.LastUpdateTime = updateTime;

                staticPageDependency.UpdateSuccess = true;
            }
            catch (Exception ex)
            {
                StaticPageManager.SaveError(ex, "���þ�̬ҳ���ɷ���Updateʱ�����쳣��" + ex.Message);
                //����������������ִ��

                //ȫ�����ɳɹ�������������ʱ��,����������10������������
                if (updateTime == DateTime.MaxValue)
                    staticPageDependency.LastUpdateTime = DateTime.Now.AddMinutes(retryInterval);
                else
                    staticPageDependency.LastUpdateTime = updateTime.AddMinutes(retryInterval);

                staticPageDependency.UpdateSuccess = false;
            }
            finally
            {
                //����������ʱ��
                //staticPageDependency.LastUpdateTime = updateTime;
            }

            updateComplete = true;
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        public void Update(TimeSpan timeSpan)
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                TimeSpan span = (TimeSpan)obj;
                Thread.Sleep(span);

                (this as IUpdateItem).Update(DateTime.MaxValue);
            }, timeSpan);
        }

        /// <summary>
        /// ��ȡ��ѭ������
        /// </summary>
        /// <returns></returns>
        private int GetPageCount(Dictionary<string, IList<object>> dict)
        {
            int count = 1;
            foreach (string paramName in dict.Keys)
            {
                count *= dict[paramName].Count;
            }
            return count;
        }

        /// <summary>
        /// ��ȡ��ѯ�ַ���
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetRealPath(string query)
        {
            foreach (string key in dict.Keys)
            {
                query = query.Replace(key, dict[key][dictPosition[key]].ToString());
            }

            return query;
        }

        /// <summary>
        /// ���ö�Ӧ������ֵ
        /// </summary>
        /// <param name="index"></param>
        private void SetPosition(int index)
        {
            if (index < 0) return;
            string key = new List<string>(dict.Keys)[index];
            if (dictPosition[key] < dict[key].Count - 1)
            {
                dictPosition[key]++;
            }
            else
            {
                dictPosition[key] = 0;
                SetPosition(--index);
            }
        }

        /// <summary>
        /// ȥ����Ŀ¼
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string RemoveRootPath(string path)
        {
            try
            {
                return path.Replace(AppDomain.CurrentDomain.BaseDirectory, "/").Replace("\\", "/").Replace("//", "/");
            }
            catch
            {
                return path;
            }
        }
    }
}
