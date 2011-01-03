using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace MySoft.Web
{
    /// <summary>
    /// 静态页子项接口
    /// </summary>
    public interface IStaticPageItem
    {
        /// <summary>
        /// 开始处理
        /// </summary>
        event ExcutingEventHandler OnStart;

        /// <summary>
        /// 结束处理
        /// </summary>
        event ExcutingEventHandler OnEnd;

        /// <summary>
        /// 生成时的回调
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
    /// 执行生成时的委托
    /// </summary>
    /// <param name="createTime"></param>
    /// <param name="dynamicurl"></param>
    /// <param name="staticurl"></param>
    public delegate void ExcutingEventHandler(DateTime createTime, string dynamicurl, string staticurl);

    /// <summary>
    /// 生成页面时委托
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
    /// 获取开始结束值的委托
    /// </summary>
    /// <returns></returns>
    public delegate int StartEndValueEventHandler();

    /// <summary>
    /// 通用静态页子项
    /// </summary>
    public sealed class SingleStaticPageItem : IStaticPageItem
    {
        /// <summary>
        /// 回调
        /// </summary>
        public event ExcutingEventHandler OnStart;

        /// <summary>
        /// 结束处理
        /// </summary>
        public event ExcutingEventHandler OnEnd;

        /// <summary>
        /// 生成时的回调
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
            this.outEncoding = Encoding.UTF8;
            this.inEncoding = Encoding.UTF8;
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

                //开始生成
                if (OnStart != null) OnStart(createTime, dynamicurl, RemoveRootPath(staticurl));

                //生成时回调
                if (Callback != null) content = Callback(content);

                string extension = Path.GetExtension(staticurl);
                if (extension != null && extension.ToLower() == ".js")
                {
                    //加入静态页生成元素
                    content = string.Format("{3}\r\n\r\n//<!-- 生成方式：主动生成 -->\r\n//<!-- 更新时间：{0} -->\r\n//<!-- 动态URL：{1} -->\r\n//<!-- 静态URL：{2} -->",
                                        createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                }
                else
                {
                    //加入静态页生成元素
                    content = string.Format("{3}\r\n\r\n<!-- 生成方式：主动生成 -->\r\n<!-- 更新时间：{0} -->\r\n<!-- 动态URL：{1} -->\r\n<!-- 静态URL：{2} -->",
                                        createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                }

                StaticPageManager.SaveFile(content, staticurl, outEncoding);

                //结束生成
                if (OnEnd != null) OnEnd(createTime, dynamicurl, RemoveRootPath(staticurl));
            }
            catch (Exception ex)
            {
                StaticPageManager.SaveError(ex, string.Format("生成静态文件{0}失败！", RemoveRootPath(staticurl)));
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
            }, timeSpan);
        }

        /// <summary>
        /// 去除根目录
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
    /// 参数信息
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

        public StaticPageParamInfo(string paramName, StartEndValueEventHandler startValue, StartEndValueEventHandler endValue)
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
        /// 回调
        /// </summary>
        public event ExcutingEventHandler OnStart;

        /// <summary>
        /// 结束处理
        /// </summary>
        public event ExcutingEventHandler OnEnd;

        /// <summary>
        /// 生成时的回调
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
            this.outEncoding = Encoding.UTF8;
            this.inEncoding = Encoding.UTF8;
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
                    string dynamicurl = GetRealPath(templatePath);
                    string staticurl = GetRealPath(savePath);

                    try
                    {
                        string content = null;

                        if (isRemote)
                        {
                            content = StaticPageManager.GetRemotePageString(dynamicurl, inEncoding, validateString);
                        }
                        else
                        {
                            string queryurl = GetRealPath(query);
                            content = StaticPageManager.GetLocalPageString(dynamicurl, queryurl, inEncoding, validateString);

                            if (!string.IsNullOrEmpty(queryurl))
                                dynamicurl = string.Format("{0}?{1}", dynamicurl, queryurl);
                        }

                        DateTime createTime = DateTime.Now;

                        //开始生成
                        if (OnStart != null) OnStart(createTime, dynamicurl, RemoveRootPath(staticurl));

                        //生成时回调
                        if (Callback != null) content = Callback(content);

                        string extension = Path.GetExtension(staticurl);
                        if (extension != null && extension.ToLower() == ".js")
                        {
                            //加入静态页生成元素
                            content = string.Format("{3}\r\n\r\n//<!-- 生成方式：主动生成 -->\r\n//<!-- 更新时间：{0} -->\r\n//<!-- 动态URL：{1} -->\r\n//<!-- 静态URL：{2} -->",
                                                createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                        }
                        else
                        {
                            //加入静态页生成元素
                            content = string.Format("{3}\r\n\r\n<!-- 生成方式：主动生成 -->\r\n<!-- 更新时间：{0} -->\r\n<!-- 动态URL：{1} -->\r\n<!-- 静态URL：{2} -->",
                                                createTime.ToString("yyyy-MM-dd HH:mm:ss"), dynamicurl, RemoveRootPath(staticurl), content.Trim());
                        }

                        StaticPageManager.SaveFile(content, staticurl, outEncoding);

                        //结束生成
                        if (OnEnd != null) OnEnd(createTime, dynamicurl, RemoveRootPath(staticurl));
                    }
                    catch (Exception ex)
                    {
                        StaticPageManager.SaveError(ex, string.Format("生成静态文件{0}失败！", RemoveRootPath(staticurl)));
                        //如果出错，则继续往下执行
                    }
                    finally
                    {
                        SetPosition(dict.Keys.Count - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                StaticPageManager.SaveError(ex, "调用静态页生成方法Update时发生异常：" + ex.Message);
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
            }, timeSpan);
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

        /// <summary>
        /// 去除根目录
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
