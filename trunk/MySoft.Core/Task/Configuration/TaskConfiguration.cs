using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

namespace MySoft.Core.Task
{
    /// <summary>
    /// 计划任务配置
    /// <remarks>
    /// <code>
    /// <configuration>
    ///     <configSections>
    /// 	    <sectionGroup name="configFramework">
    /// 		    <section name="task" type="MySoft.Core.Task.TaskConfigurationHandler, MySoft.Core"/>
    /// 	    </sectionGroup>
    ///     </configSections>
    ///        ......
    ///     <shumi.framework>
    /// 	    <task>
    ///             <job name="job1" beginDate="2008-1-1" endDate="2010-1-1" beginTime="" endTime="" timespan="" interval="" assemblyName="" className=""/>
    /// 	    </task>
    ///     </configFramework>
    /// </configuration>
    /// </code>
    /// </remarks>
    /// </summary>
    public class TaskConfiguration : ConfigurationBase
    {
        /// <summary>
        /// 获取定时任务配置
        /// </summary>
        /// <returns></returns>
        public static TaskConfiguration GetConfig()
        {
            object obj = ConfigurationManager.GetSection("configFramework/task");

            if (obj != null)
                return (TaskConfiguration)obj;
            else
                return null;
        }

        private Dictionary<string, Job> _Jobs = new Dictionary<string, Job>();

        /// <summary>
        /// 任务集合
        /// </summary>
        public Dictionary<string, Job> Jobs
        {
            get { return _Jobs; }
            set { _Jobs = value; }
        }

        /// <summary>
        /// 从配置文件加载配置值
        /// </summary>
        /// <param name="node"></param>
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            if (node == null) return;

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Comment) continue;

                if (n.Name == "job")
                {
                    XmlAttributeCollection ac = n.Attributes;
                    Job job = new Job();

                    job.Name = ac["name"].Value;

                    if (ac["beginDate"] != null && ac["beginDate"].Value.Trim() != string.Empty)
                        job.BeginDate = Convert.ToDateTime(ac["beginDate"].Value);
                    else
                        job.BeginDate = DateTime.MinValue;

                    if (ac["endDate"] != null && ac["endDate"].Value.Trim() != string.Empty)
                        job.EndDate = Convert.ToDateTime(ac["endDate"].Value);
                    else
                        job.EndDate = DateTime.MaxValue;

                    if (ac["beginTime"] != null && ac["beginTime"].Value.Trim() != string.Empty)
                        job.BeginTime = ac["beginTime"].Value;
                    else
                        job.BeginTime = "0:00";

                    if (ac["endTime"] != null && ac["endTime"].Value.Trim() != string.Empty)
                        job.EndTime = ac["endTime"].Value;
                    else
                        job.EndTime = "23:59";

                    if (ac["interval"] != null && ac["interval"].Value != null)
                        job.Interval = Convert.ToInt32(ac["interval"].Value);
                    else if (ac["timespan"] != null && ac["timespan"].Value != null)
                        job.Interval = (int)TimeSpan.Parse(ac["timespan"].Value).TotalMilliseconds;

                    job.AssemblyName = ac["assemblyName"].Value;
                    job.ClassName = ac["className"].Value;

                    if (!_Jobs.ContainsKey(job.Name))
                    {
                        _Jobs.Add(job.Name, job);
                    }
                }
            }
        }
    }
}
