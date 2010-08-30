using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace KiShion.Web
{
    /// <summary>
    /// ��̬ҳ����ӿ�
    /// </summary>
    public interface IStaticPageItem
    {
        /// <summary>
        /// ����ҳ��ʱ�ص�
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
    /// ����ҳ��ʱ�ص�
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public delegate string CallbackEventHandler(string content);

    /// <summary>
    /// ����ֵ�����ί��
    /// </summary>
    /// <returns></returns>
    public delegate object[] GetValuesEventHandler();

    /// <summary>
    /// ���ؿ�ʼ����ί��
    /// </summary>
    /// <returns></returns>
    public delegate int StartValueEventHandler();

    /// <summary>
    /// ���ؽ�������ί��
    /// </summary>
    /// <returns></returns>
    public delegate int EndValueEventHandler();

    /// <summary>
    /// ͨ�þ�̬ҳ����
    /// </summary>
    public sealed class SingleStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// ����ҳ��ʱ�ص�
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

        /// <summary>
        /// ��ǰ�Ƿ���Ը���
        /// </summary>
        public bool NeedUpdate(DateTime updateTime)
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
            this.outEncoding = new UTF8Encoding();
            this.inEncoding = new UTF8Encoding();
            this.staticPageDependency = new SlidingUpdateTime(new TimeSpan(0, 20, 0));
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
            Update(DateTime.MaxValue);
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        public void Update(DateTime updateTime)
        {
            updateComplete = false;

            try
            {
                if (isRemote)
                {
                    string content = StaticPageUtils.GetRemotePageString(templatePath, inEncoding, validateString);
                    if (Callback != null) content = Callback(content);

                    StaticPageUtils.SaveFile(content, savePath, outEncoding);
                }
                else
                {
                    string content = StaticPageUtils.GetLocalPageString(templatePath, query, inEncoding, validateString);
                    if (Callback != null) content = Callback(content);

                    StaticPageUtils.SaveFile(content, savePath, outEncoding);
                }
            }
            catch (Exception ex)
            {
                StaticPageUtils.SaveError(ex);
                //����������������ִ��
            }
            finally
            {
                //����������ʱ��
                staticPageDependency.LastUpdateTime = updateTime;
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

                Update(DateTime.MaxValue);
            });
        }
    }

    /// <summary>
    /// ��ѯ��Ϣ
    /// </summary>
    public sealed class StaticPageParamInfo
    {
        private string paramName;
        /// <summary>
        /// ������
        /// </summary>
        internal string ParamName
        {
            get { return paramName; }
        }

        private GetValuesEventHandler getValues;
        /// <summary>
        /// ��ȡֵί��
        /// </summary>
        public GetValuesEventHandler GetValues
        {
            get { return getValues; }
        }

        public StaticPageParamInfo(string paramName, int startPage, int endPage)
        {
            this.paramName = paramName;
            List<object> list = new List<object>();
            for (int index = startPage; index <= endPage; index++)
            {
                list.Add(index);
            }
            this.getValues = delegate() { return list.ToArray(); };
        }

        public StaticPageParamInfo(string paramName, StartValueEventHandler startValue, EndValueEventHandler endValue)
            : this(paramName, startValue(), endValue())
        { }

        public StaticPageParamInfo(string paramName, object[] values)
        {
            this.paramName = paramName;
            this.getValues = delegate() { return values; };
        }

        public StaticPageParamInfo(string paramName, GetValuesEventHandler getValues)
        {
            this.paramName = paramName;
            this.getValues = getValues;
        }
    }

    /// <summary>
    /// ��ҳ��̬ҳ����
    /// </summary>
    public sealed class ParamStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// ����ҳ��ʱ�ص�
        /// </summary>
        public event CallbackEventHandler Callback;

        #region ����

        private string templatePath;
        private string savePath;
        private string query;
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

        /// <summary>
        /// ��ǰ�Ƿ���Ը���
        /// </summary>
        public bool NeedUpdate(DateTime updateTime)
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
            this.outEncoding = new UTF8Encoding();
            this.inEncoding = new UTF8Encoding();
            this.staticPageDependency = new SlidingUpdateTime(new TimeSpan(0, 20, 0));
            this.isRemote = false;
            this.updateComplete = true;
        }

        /// <summary>
        /// ��ʼ����̬ҳ������
        /// </summary>
        /// <param name="templatePath">ģ��·��</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="validateString">��֤�ַ���</param>
        public ParamStaticPageItem(string templatePath, string savePath, string validateString, params StaticPageParamInfo[] paramInfos)
            : this()
        {
            this.templatePath = templatePath;
            this.savePath = savePath;
            this.validateString = validateString;
            this.paramInfos = paramInfos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath">ģ��·��</param>
        /// <param name="savePath">�����ļ�·��</param>
        /// <param name="createSpan">���ɼ��ʱ��</param>
        public ParamStaticPageItem(string templatePath, string savePath, string validateString, IUpdateDependency staticPageDependency, params StaticPageParamInfo[] paramInfos)
            : this(templatePath, savePath, validateString, paramInfos)
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
        public ParamStaticPageItem(string templatePath, string query, string savePath, string validateString, params StaticPageParamInfo[] paramInfos)
            : this(templatePath, savePath, validateString, paramInfos)
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
            Update(DateTime.MaxValue);
        }

        /// <summary>
        /// ��ҳ����и���
        /// </summary>
        public void Update(DateTime updateTime)
        {
            updateComplete = false;

            try
            {
                dict = new Dictionary<string, IList<object>>();
                dictPosition = new Dictionary<string, int>();

                foreach (StaticPageParamInfo paramInfo in paramInfos)
                {
                    if (!dict.ContainsKey(paramInfo.ParamName))
                    {
                        dict.Add(paramInfo.ParamName, new List<object>(paramInfo.GetValues()));
                        dictPosition.Add(paramInfo.ParamName, 0);
                    }
                }

                int count = GetPageCount(dict);
                for (int index = 0; index < count; index++)
                {
                    try
                    {
                        if (isRemote)
                        {
                            string content = StaticPageUtils.GetRemotePageString(GetRealPath(templatePath),
                                inEncoding, validateString);
                            if (Callback != null) content = Callback(content);

                            StaticPageUtils.SaveFile(content, GetRealPath(savePath), outEncoding);
                        }
                        else
                        {
                            string content = StaticPageUtils.GetLocalPageString(templatePath, GetRealPath(query),
                                inEncoding, validateString);
                            if (Callback != null) content = Callback(content);

                            StaticPageUtils.SaveFile(content, GetRealPath(savePath), outEncoding);
                        }
                    }
                    catch (Exception ex)
                    {
                        StaticPageUtils.SaveError(ex);
                        //����������������ִ��
                    }
                    finally
                    {
                        SetPosition(dict.Keys.Count - 1);
                    }
                }
            }
            catch
            {
                //���������;
            }
            finally
            {
                //����������ʱ��
                staticPageDependency.LastUpdateTime = updateTime;
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

                Update(DateTime.MaxValue);
            });
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
    }
}
