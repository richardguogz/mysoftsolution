using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace KiShion.Web
{
    /// <summary>
    /// 静态页子项接口
    /// </summary>
    public interface IStaticPageItem
    {
        /// <summary>
        /// 生成页面时回调
        /// </summary>
        event CallbackEventHandler Callback;

        /// <summary>
        /// 静态页生成依赖
        /// </summary>
        IUpdateDependency StaticPageDependency { get; set; }

        /// <summary>
        /// 输出编码
        /// </summary>
        Encoding OutEncoding { get; set; }

        /// <summary>
        /// 输入编码
        /// </summary>
        Encoding InEncoding { get; set; }

        /// <summary>
        /// 是否为远程页面
        /// </summary>
        bool IsRemote { get; set; }

        /// <summary>
        /// 立即更新页面
        /// </summary>
        void Update();

        /// <summary>
        /// 对页面进行更新
        /// </summary>
        void Update(DateTime updateTime);

        /// <summary>
        /// 异步更新，TimeSpan表示延迟更新的时间，所有依赖失效
        /// </summary>
        void Update(TimeSpan timeSpan);

        /// <summary>
        /// 当前是否可以更新
        /// </summary>
        bool NeedUpdate(DateTime updateTime);
    }

    /// <summary>
    /// 生成页面时回调
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public delegate string CallbackEventHandler(string content);

    /// <summary>
    /// 返回值数组的委托
    /// </summary>
    /// <returns></returns>
    public delegate object[] GetValuesEventHandler();

    /// <summary>
    /// 返回开始数的委托
    /// </summary>
    /// <returns></returns>
    public delegate int StartValueEventHandler();

    /// <summary>
    /// 返回结束数的委托
    /// </summary>
    /// <returns></returns>
    public delegate int EndValueEventHandler();

    /// <summary>
    /// 通用静态页子项
    /// </summary>
    public sealed class SingleStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// 生成页面时回调
        /// </summary>
        public event CallbackEventHandler Callback;

        #region 属性

        private string query;
        private string templatePath;
        private string savePath;
        private string validateString;
        private bool updateComplete;

        private IUpdateDependency staticPageDependency;
        /// <summary>
        /// 静态页依赖方案
        /// </summary>
        public IUpdateDependency StaticPageDependency
        {
            get { return staticPageDependency; }
            set { staticPageDependency = value; }
        }

        private Encoding outEncoding;
        /// <summary>
        /// 输出编码
        /// </summary>
        public Encoding OutEncoding
        {
            get { return outEncoding; }
            set { outEncoding = value; }
        }

        private Encoding inEncoding;
        /// <summary>
        /// 输入编码
        /// </summary>
        public Encoding InEncoding
        {
            get { return inEncoding; }
            set { inEncoding = value; }
        }

        private bool isRemote;
        /// <summary>
        /// 是否为远程页面
        /// </summary>
        public bool IsRemote
        {
            get { return isRemote; }
            set { isRemote = value; }
        }

        /// <summary>
        /// 当前是否可以更新
        /// </summary>
        public bool NeedUpdate(DateTime updateTime)
        {
            //如果没更新完成，则返回
            if (!updateComplete) return false;

            //判断策略是否已经达到更新值
            return staticPageDependency.HasUpdate(updateTime);
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化静态页生成类
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
        /// 初始化静态页生成类
        /// </summary>
        /// <param name="templatePath">模板路径</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="validateString">验证字符串</param>
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
        /// <param name="templatePath">模板路径</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="createSpan">生成间隔时间</param>
        public SingleStaticPageItem(string templatePath, string savePath, string validateString, IUpdateDependency staticPageDependency)
            : this(templatePath, savePath, validateString)
        {
            this.staticPageDependency = staticPageDependency;
        }

        /// <summary>
        /// 初始化静态页生成类
        /// </summary>
        /// <param name="templatePath">模板页路径</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="createSpan">生成间隔时间</param>
        /// <param name="validateString">验证字符串</param>
        /// <param name="query">查询参数字符串</param>
        public SingleStaticPageItem(string templatePath, string query, string savePath, string validateString)
            : this(templatePath, savePath, validateString)
        {
            this.query = query;
        }


        /// <summary>
        /// 初始化静态页生成类
        /// </summary>
        /// <param name="templatePath">模板页路径</param>
        /// <param name="query">查询参数字符串</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="createSpan">生成间隔时间</param>
        /// <param name="validateString">验证字符</param>
        public SingleStaticPageItem(string templatePath, string query, string savePath, string validateString, IUpdateDependency staticPageDependency)
            : this(templatePath, query, savePath, validateString)
        {
            this.staticPageDependency = staticPageDependency;
        }

        #endregion

        /// <summary>
        /// 立即更新页面
        /// </summary>
        public void Update()
        {
            Update(DateTime.MaxValue);
        }

        /// <summary>
        /// 对页面进行更新
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
                //如果出错，则继续往下执行
            }
            finally
            {
                //设置最后更新时间
                staticPageDependency.LastUpdateTime = updateTime;
            }

            updateComplete = true;
        }

        /// <summary>
        /// 对页面进行更新
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
    /// 查询信息
    /// </summary>
    public sealed class StaticPageParamInfo
    {
        private string paramName;
        /// <summary>
        /// 参数名
        /// </summary>
        internal string ParamName
        {
            get { return paramName; }
        }

        private GetValuesEventHandler getValues;
        /// <summary>
        /// 获取值委托
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
    /// 分页静态页子项
    /// </summary>
    public sealed class ParamStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// 生成页面时回调
        /// </summary>
        public event CallbackEventHandler Callback;

        #region 属性

        private string templatePath;
        private string savePath;
        private string query;
        private string validateString;
        private bool updateComplete;

        private IUpdateDependency staticPageDependency;
        /// <summary>
        /// 静态页生成依赖
        /// </summary>
        public IUpdateDependency StaticPageDependency
        {
            get { return staticPageDependency; }
            set { staticPageDependency = value; }
        }

        private StaticPageParamInfo[] paramInfos;
        /// <summary>
        /// 静态页参数
        /// </summary>
        public StaticPageParamInfo[] ParamInfos
        {
            get { return paramInfos; }
            set { paramInfos = value; }
        }

        private Encoding outEncoding;
        /// <summary>
        /// 输出编码
        /// </summary>
        public Encoding OutEncoding
        {
            get { return outEncoding; }
            set { outEncoding = value; }
        }

        private Encoding inEncoding;
        /// <summary>
        /// 输入编码
        /// </summary>
        public Encoding InEncoding
        {
            get { return inEncoding; }
            set { inEncoding = value; }
        }

        private bool isRemote;
        /// <summary>
        /// 是否为远程页面
        /// </summary>
        public bool IsRemote
        {
            get { return isRemote; }
            set { isRemote = value; }
        }

        /// <summary>
        /// 当前是否可以更新
        /// </summary>
        public bool NeedUpdate(DateTime updateTime)
        {
            //如果没更新完成，则返回
            if (!updateComplete) return false;

            //判断策略是否已经达到更新值
            return staticPageDependency.HasUpdate(updateTime);
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化静态页生成类
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
        /// 初始化静态页生成类
        /// </summary>
        /// <param name="templatePath">模板路径</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="validateString">验证字符串</param>
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
        /// <param name="templatePath">模板路径</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="createSpan">生成间隔时间</param>
        public ParamStaticPageItem(string templatePath, string savePath, string validateString, IUpdateDependency staticPageDependency, params StaticPageParamInfo[] paramInfos)
            : this(templatePath, savePath, validateString, paramInfos)
        {
            this.staticPageDependency = staticPageDependency;
        }

        /// <summary>
        /// 初始化静态页生成类
        /// </summary>
        /// <param name="templatePath">模板页路径</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="createSpan">生成间隔时间</param>
        /// <param name="validateString">验证字符串</param>
        /// <param name="query">查询参数字符串</param>
        public ParamStaticPageItem(string templatePath, string query, string savePath, string validateString, params StaticPageParamInfo[] paramInfos)
            : this(templatePath, savePath, validateString, paramInfos)
        {
            this.query = query;
        }


        /// <summary>
        /// 初始化静态页生成类
        /// </summary>
        /// <param name="templatePath">模板页路径</param>
        /// <param name="query">查询参数字符串</param>
        /// <param name="savePath">生成文件路径</param>
        /// <param name="createSpan">生成间隔时间</param>
        /// <param name="validateString">验证字符</param>
        public ParamStaticPageItem(string templatePath, string query, string savePath, string validateString, IUpdateDependency staticPageDependency, params StaticPageParamInfo[] paramInfos)
            : this(templatePath, query, savePath, validateString, paramInfos)
        {
            this.staticPageDependency = staticPageDependency;
        }

        #endregion

        //保存用于更新的字典信息
        private Dictionary<string, IList<object>> dict;
        private Dictionary<string, int> dictPosition;

        /// <summary>
        /// 立即更新页面
        /// </summary>
        public void Update()
        {
            Update(DateTime.MaxValue);
        }

        /// <summary>
        /// 对页面进行更新
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
                        //如果出错，则继续往下执行
                    }
                    finally
                    {
                        SetPosition(dict.Keys.Count - 1);
                    }
                }
            }
            catch
            {
                //不处理错误;
            }
            finally
            {
                //设置最后更新时间
                staticPageDependency.LastUpdateTime = updateTime;
            }

            updateComplete = true;
        }

        /// <summary>
        /// 对页面进行更新
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
        /// 获取总循环次数
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
        /// 获取查询字符串
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
        /// 设置对应的坐标值
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
