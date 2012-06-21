using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace MySoft.Common
{
    /// <summary>
    /// Function �����ܺ����࣬�ַ����������
    /// </summary>
    public abstract class FunctionHelper
    {
        #region ������ʽ��ʹ��

        /// <summary>
        /// �ж�������ַ����Ƿ���ȫƥ������
        /// </summary>
        /// <param name="RegexExpression">������ʽ</param>
        /// <param name="str">���жϵ��ַ���</param>
        /// <returns></returns>
        public static bool IsValiable(string RegexExpression, string str)
        {
            bool blResult = false;

            Regex rep = new Regex(RegexExpression, RegexOptions.IgnoreCase);

            //blResult = rep.IsMatch(str);
            Match mc = rep.Match(str);

            if (mc.Success)
            {
                if (mc.Value == str) blResult = true;
            }


            return blResult;
        }

        /// <summary>
        /// ת�������е�URL·��Ϊ����URL·��
        /// </summary>
        /// <param name="sourceString">Դ����</param>
        /// <param name="replaceURL">�滻Ҫ��ӵ�URL</param>
        /// <returns>string</returns>
        public static string ConvertURL(string sourceString, string replaceURL)
        {
            Regex rep = new Regex(" (src|href|background|value)=('|\"|)([^('|\"|)http://].*?)('|\"| |>)");
            sourceString = rep.Replace(sourceString, " $1=$2" + replaceURL + "$3$4");
            return sourceString;
        }

        /// <summary>
        /// ��ȡ����������ͼƬ����HTTP��ͷ��URL��ַ
        /// </summary>
        /// <param name="sourceString">��������</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetImgFileUrl(string sourceString)
        {
            ArrayList imgArray = new ArrayList();

            Regex r = new Regex("<IMG(.*?)src=('|\"|)(http://.*?)('|\"| |>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!imgArray.Contains(mc[i].Result("$3")))
                {
                    imgArray.Add(mc[i].Result("$3"));
                }
            }

            return imgArray;
        }

        /// <summary>
        /// ��ȡ�����������ļ�����HTTP��ͷ��URL��ַ
        /// </summary>
        /// <param name="sourceString">��������</param>
        /// <returns>ArrayList</returns>
        public static Hashtable GetFileUrlPath(string sourceString)
        {
            Hashtable url = new Hashtable();

            Regex r = new Regex(" (src|href|background|value)=('|\"|)(http://.*?)('|\"| |>)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!url.ContainsValue(mc[i].Result("$3")))
                {
                    url.Add(i, mc[i].Result("$3"));
                }
            }

            return url;
        }

        /// <summary>
        /// ��ȡһ��SQL����е�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public static ArrayList SqlParame(string sql)
        {
            ArrayList list = new ArrayList();
            Regex r = new Regex(@"@(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }

        /// <summary>
        /// ��ȡһ��SQL����е�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public static ArrayList OracleParame(string sql)
        {
            ArrayList list = new ArrayList();
            Regex r = new Regex(@":(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }

        /// <summary>
        /// ��HTML����ת���ɴ��ı�
        /// </summary>
        /// <param name="sourceHTML">HTML����</param>
        /// <returns></returns>
        public static string ConvertText(string sourceHTML)
        {
            string strResult = "";
            Regex r = new Regex("<(.*?)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sourceHTML);

            if (mc.Count == 0)
            {
                strResult = sourceHTML;
            }
            else
            {
                strResult = sourceHTML;

                for (int i = 0; i < mc.Count; i++)
                {
                    strResult = strResult.Replace(mc[i].ToString(), "");
                }
            }

            return strResult;
        }
        #endregion

        #region �Զ��崦��

        /// <summary>
        /// ��ȡ web.config �ļ���ָ�� key ��ֵ
        /// </summary>
        /// <param name="keyName">key����</param>
        /// <returns></returns>
        public static string GetAppSettings(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }


        /// <summary>
        /// ����ָ����ʽ���ʱ��
        /// </summary>
        /// <param name="NowDate">ʱ��</param>
        /// <param name="type">�������</param>
        /// <returns></returns>
        public static string WriteDate(string NowDate, int type)
        {
            double TimeZone = 0;
            DateTime NewDate = DateTime.Parse(NowDate).AddHours(TimeZone);
            string strResult = "";

            switch (type)
            {
                case 1:
                    strResult = NewDate.ToString();
                    break;
                case 2:
                    strResult = NewDate.ToShortDateString().ToString();
                    break;
                case 3:
                    strResult = NewDate.Year + "��" + NewDate.Month + "��" + NewDate.Day + "�� " + NewDate.Hour + "��" + NewDate.Minute + "��" + NewDate.Second + "��";
                    break;
                case 4:
                    strResult = NewDate.Year + "��" + NewDate.Month + "��" + NewDate.Day + "��";
                    break;
                case 5:
                    strResult = NewDate.Year + "��" + NewDate.Month + "��" + NewDate.Day + "�� " + NewDate.Hour + "��" + NewDate.Minute + "��";
                    break;
                case 6:
                    strResult = NewDate.Year + "-" + NewDate.Month + "-" + NewDate.Day + "  " + NewDate.Hour + ":" + NewDate.Minute;
                    break;
                default:
                    strResult = NewDate.ToString();
                    break;
            }
            return strResult;
        }


        private static int Instr(string strA, string strB)
        {
            if (string.Compare(strA, strA.Replace(strB, "")) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// �жϿͻ��˲���ϵͳ�������������
        /// </summary>
        /// <param name="Info">�ͻ��˷��ص�ͷ��Ϣ(Request.UserAgent)</param>
        /// <param name="Type">��ȡ���ͣ�1Ϊ����ϵͳ�� 2Ϊ�����</param>
        /// <returns></returns>
        public static string GetInfo(string Info, int Type)
        {

            string GetInfo = "";
            switch (Type)
            {
                case 1:
                    if (Instr(Info, @"NT 5.1") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows XP";
                    }
                    else if (Instr(Info, @"Tel") > 0)
                    {
                        GetInfo = "����ϵͳ��Telport";
                    }
                    else if (Instr(Info, @"webzip") > 0)
                    {
                        GetInfo = "����ϵͳ������ϵͳ��webzip";
                    }
                    else if (Instr(Info, @"flashget") > 0)
                    {
                        GetInfo = "����ϵͳ��flashget";
                    }
                    else if (Instr(Info, @"offline") > 0)
                    {
                        GetInfo = "����ϵͳ��offline";
                    }
                    else if (Instr(Info, @"NT 5") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows 2000";
                    }
                    else if (Instr(Info, @"NT 4") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows NT4";
                    }
                    else if (Instr(Info, @"98") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows 98";
                    }
                    else if (Instr(Info, @"95") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows 95";
                    }
                    else
                    {
                        GetInfo = "����ϵͳ��δ֪";
                    }
                    break;
                case 2:
                    if (Instr(Info, @"NetCaptor 6.5.0") > 0)
                    {
                        GetInfo = "� �� ����NetCaptor 6.5.0";
                    }
                    else if (Instr(Info, @"MyIe 3.1") > 0)
                    {
                        GetInfo = "� �� ����MyIe 3.1";
                    }
                    else if (Instr(Info, @"NetCaptor 6.5.0RC1") > 0)
                    {
                        GetInfo = "� �� ����NetCaptor 6.5.0RC1";
                    }
                    else if (Instr(Info, @"NetCaptor 6.5.PB1") > 0)
                    {
                        GetInfo = "� �� ����NetCaptor 6.5.PB1";
                    }
                    else if (Instr(Info, @"MSIE 6.0b") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 6.0b";
                    }
                    else if (Instr(Info, @"MSIE 6.0") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 6.0";
                    }
                    else if (Instr(Info, @"MSIE 5.5") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 5.5";
                    }
                    else if (Instr(Info, @"MSIE 5.01") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 5.01";
                    }
                    else if (Instr(Info, @"MSIE 5.0") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 5.0";
                    }
                    else if (Instr(Info, @"MSIE 4.0") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 4.0";
                    }
                    else
                    {
                        GetInfo = "� �� ����δ֪";
                    }
                    break;
            }
            return GetInfo;
        }


        /// <summary>
        /// ��ȡ������������MAC��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetMAC_Address()
        {
            string strResult = "";

            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True") strResult = mo["MacAddress"].ToString();
            }

            return strResult;
        }

        /// <summary>
        /// ת���ļ�·���в������ַ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string</returns>
        public static string ConvertDirURL(string path)
        {
            return AddLast(path.Replace("/", "\\"), "\\");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string</returns>
        public static string ConvertXmlString(string str)
        {
            return "<![CDATA[" + str + "]]>";
        }

        /// <summary>
        /// ת��һ��double�����ִ�Ϊʱ�䣬��ʼ 0 Ϊ 1970-01-01 08:00:00
        /// ԭ����ǣ�ÿ��һ�����������ִ����ۼ�һ
        /// </summary>
        /// <param name="d">double ������</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = DateTime.MinValue;

            DateTime startTime = DateTime.Parse("1970-01-01 08:00:00");

            time = startTime.AddSeconds(d);

            return time;
        }

        /// <summary>
        /// ת��ʱ��Ϊһ��double�����ִ�����ʼ 0 Ϊ 1970-01-01 08:00:00
        /// ԭ����ǣ�ÿ��һ�����������ִ����ۼ�һ
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(DateTime time)
        {
            double intResult = 0;

            DateTime startTime = DateTime.Parse("1970-01-01 08:00:00");

            intResult = (time - startTime).TotalSeconds;

            return intResult;
        }

        /// <summary>
        /// ��ȡһ��URL�����õ��ļ����ƣ�������׺����
        /// </summary>
        /// <param name="url">URL��ַ</param>
        /// <returns>string</returns>
        public static string GetFileName(string url)
        {
            //string[] Name = FunctionHelper.SplitArray(url,'/');
            //return Name[Name.Length - 1];

            return System.IO.Path.GetFileName(url);
        }

        /// <summary>
        /// ���ĳһ�ַ����ĵ�һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£������ڸ��ַ���ǰ��������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string AddFirst(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.StartsWith(Str))
            {
                strResult = Strings;
            }
            else
            {
                strResult = String.Concat(Str, Strings);
            }
            return strResult;
        }


        /// <summary>
        /// ���ĳһ�ַ��������һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£������ڸ��ַ���ĩβ��������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string AddLast(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.EndsWith(Str))
            {
                strResult = Strings;
            }
            else
            {
                strResult = String.Concat(Strings, Str);
            }
            return strResult;
        }

        /// <summary>
        /// ���ĳһ�ַ����ĵ�һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£���ͬ��ȥ������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string DelFirst(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.Length == 0) throw new Exception("ԭʼ�ַ�������Ϊ��");

            if (Strings.StartsWith(Str))
            {
                strResult = Strings.Substring(Str.Length, Strings.Length - 1);
            }
            else
            {
                strResult = Strings;
            }

            return strResult;
        }

        /// <summary>
        /// ���ĳһ�ַ��������һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£���ͬ��ȥ������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string DelLast(string Strings, string Str)
        {
            string strResult = "";

            if (Strings.EndsWith(Str))
            {
                strResult = Strings.Substring(0, Strings.Length - Str.Length);
            }
            else
            {
                strResult = Strings;
            }

            return strResult;
        }

        /// <summary>
        /// ��ȡһ��Ŀ¼�ľ���·����������WEBӦ�ó���
        /// </summary>
        /// <param name="folderPath">Ŀ¼·��</param>
        /// <returns></returns>
        public static string GetRealPath(string folderPath)
        {
            string strResult = "";

            if (folderPath.IndexOf(":\\") > 0)
            {
                strResult = AddLast(folderPath, "\\");
            }
            else
            {
                if (folderPath.StartsWith("~/"))
                {
                    strResult = AddLast(System.Web.HttpContext.Current.Server.MapPath(folderPath), "\\");
                }
                else
                {
                    string webPath = System.Web.HttpContext.Current.Request.ApplicationPath + "/";
                    strResult = AddLast(System.Web.HttpContext.Current.Server.MapPath(webPath + folderPath), "\\");
                }
            }

            return strResult;
        }

        /// <summary>
        /// ��ȡһ���ļ��ľ���·����������WEBӦ�ó���
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>string</returns>
        public static string GetRealFile(string filePath)
        {
            string strResult = "";

            //strResult = ((file.IndexOf(@":\") > 0 || file.IndexOf(":/") > 0) ? file : System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath + "/" + file));
            strResult = ((filePath.IndexOf(":\\") > 0) ?
                filePath :
                System.Web.HttpContext.Current.Server.MapPath(filePath));

            return strResult;
        }

        /// <summary>
        /// ���ַ������� HTML �������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }


        /// <summary>
        /// �� HTML �ַ������н������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        public static string HtmlDecode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        /// �Խű�������д���
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertScript(string str)
        {
            string strResult = "";
            if (str != "")
            {
                StringReader sr = new StringReader(str);
                string rl;
                do
                {
                    strResult += sr.ReadLine();
                } while ((rl = sr.ReadLine()) != null);
            }

            strResult = strResult.Replace("\"", "&quot;");

            return strResult;
        }


        /// <summary>
        /// ��һ���ַ�����ĳһ�ض��ַ��ָ���ַ�������
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="str">�ָ��ַ�</param>
        /// <returns>string[]</returns>
        public static string[] SplitArray(string Strings, char str)
        {
            string[] strArray = Strings.Trim().Split(new char[] { str });

            return strArray;
        }

        /*
                /// <summary>
                /// ��һ���ַ�����ĳһ�ַ��ָ������
                /// </summary>
                /// <param name="Strings">�ַ���</param>
                /// <param name="str">�ָ��ַ�</param>
                /// <returns>string[]</returns>
                public static string[] SplitArray(string Strings, string str)
                {
                    Regex r = new Regex(str);
                    string[] strArray = r.Split(Strings.Trim());

                    return strArray;
                }

        */

        /// <summary>
        /// ���һ���ַ������Ƿ������һ���Թ̶��ָ���ָ���ַ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="Strings">�̶��ָ���ָ���ַ���</param>
        /// <param name="Str">�ָ��</param>
        /// <returns></returns>
        public static bool InArray(string str, string Strings, char Str)
        {
            bool blResult = false;

            string[] array = SplitArray(Strings, Str);
            for (int i = 0; i < array.Length; i++)
            {
                if (str == array[i])
                {
                    blResult = true;
                    break;
                }
            }

            return blResult;
        }

        /*
                /// <summary>
                /// ���һ���ַ������Ƿ������һ���Թ̶��ָ���ָ���ַ�����
                /// </summary>
                /// <param name="str">�ַ���</param>
                /// <param name="Strings">�̶��ָ���ָ���ַ���</param>
                /// <param name="Str">�ָ��</param>
                /// <returns></returns>
                public static bool InArray(string str, string Strings, string Str)
                {
                    bool blResult = false;

                    string[] array = SplitArray(Strings, Str);
                    for(int i = 0; i < array.Length; i++)
                    {
                        if(str == array[i])
                        {
                            blResult = true;
                            break;
                        }
                    }

                    return blResult;
                }
                */

        /// <summary>
        /// ���һ���ַ������Ƿ������һ���Թ̶��ָ���ָ���ַ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="array">�ַ�������</param>
        /// <returns></returns>
        public static bool InArray(string str, string[] array)
        {
            bool blResult = false;

            for (int i = 0; i < array.Length; i++)
            {
                if (str == array[i])
                {
                    blResult = true;
                    break;
                }
            }

            return blResult;
        }


        /// <summary>
        /// ���ֵ�Ƿ���Ч��Ϊ null �� "" ��Ϊ��Ч
        /// </summary>
        /// <param name="obj">Ҫ����ֵ</param>
        /// <returns></returns>
        public static bool CheckValiable(object obj)
        {
            if (Object.Equals(obj, null) || Object.Equals(obj, string.Empty))
                return false;
            else
                return true;
        }
        #endregion

        #region �����ȡ��ҳ����SQL���

        #region ConstructSplitSQL
        /// <summary>
        /// ��ȡ��ҳ����SQL���(����������ֶα��뽨���������Ż���ҳ��ȡ��ʽ)
        /// </summary>
        /// <param name="tblName">����������</param>
        /// <param name="fldName">����������ֶ�</param>
        /// <param name="PageIndex">��ǰҳ</param>
        /// <param name="PageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="totalRecord">�ܼ�¼��</param>
        /// <param name="OrderType">����ʽ(0����1Ϊ����)</param>
        /// <param name="strWhere">������������䣬����Ҫ�ټ�WHERE�ؼ���</param>
        /// <returns></returns>
        public static string ConstructSplitSQL(string tblName,
                                                string fldName,
                                                int PageIndex,
                                                int PageSize,
                                                int totalRecord,
                                                int OrderType,
                                                string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";
            string rtnFields = "*";

            // ���������������ַ���
            if (strWhere != "")
            {
                // ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // �������
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + "ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // �������
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + "ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // �쳣����
            {
                throw new DataException("δָ���κ��������͡�0����1Ϊ����");
            }

            return strSQL;
        }


        /// <summary>
        /// ��ȡ��ҳ����SQL���(����������ֶα��뽨������)
        /// </summary>
        /// <param name="tblName">��������</param>
        /// <param name="fldName">���������ֶ�����</param>
        /// <param name="PageIndex">��ǰҳ</param>
        /// <param name="PageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="rtnFields">�����ֶμ��ϣ��м��ö��Ÿ񿪡�����ȫ���á�*��</param>
        /// <param name="OrderType">����ʽ(0����1Ϊ����)</param>
        /// <param name="strWhere">������������䣬����Ҫ�ټ�WHERE�ؼ���</param>
        /// <returns></returns>
        public static string ConstructSplitSQL(string tblName,
                                                string fldName,
                                                int PageIndex,
                                                int PageSize,
                                                string rtnFields,
                                                int OrderType,
                                                string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // ���������������ַ���
            if (strWhere != "")
            {
                // ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // �������
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + "ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // �������
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + "ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // �쳣����
            {
                throw new DataException("δָ���κ��������͡�0����1Ϊ����");
            }

            return strSQL;
        }


        /// <summary>
        /// ��ȡ��ҳ����SQL���(����������ֶα��뽨������)
        /// </summary>
        /// <param name="tblName">��������</param>
        /// <param name="fldName">���������ֶ�����</param>
        /// <param name="unionCondition">�������ӵ�����������: LEFT JOIN UserInfo u ON (u.UserID = b.UserID)</param>
        /// <param name="PageIndex">��ǰҳ</param>
        /// <param name="PageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="rtnFields">�����ֶμ��ϣ��м��ö��Ÿ񿪡�����ȫ���á�*��</param>
        /// <param name="OrderType">����ʽ��0����1Ϊ����</param>
        /// <param name="strWhere">������������䣬����Ҫ�ټ�WHERE�ؼ���</param>
        /// <returns></returns>
        public static string ConstructSplitSQL(string tblName,
            string fldName,
            string unionCondition,
            int PageIndex,
            int PageSize,
            string rtnFields,
            int OrderType,
            string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // ���������������ַ���
            if (strWhere != "")
            {
                // ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // �������
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + "ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // �������
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + "ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + unionCondition + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // �쳣����
            {
                throw new DataException("δָ���κ��������͡�0����1Ϊ����");
            }

            return strSQL;
        }
        #endregion


        #region ConstructSplitSQL_TOP


        /// <summary>
        /// ��ȡ��ҳ����SQL���(����������ֶα��뽨������)
        /// </summary>
        /// <param name="tblName">��������</param>
        /// <param name="fldName">���������ֶ�����</param>
        /// <param name="PageIndex">��ǰҳ</param>
        /// <param name="PageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="rtnFields">�����ֶμ��ϣ��м��ö��Ÿ񿪡�����ȫ���á�*��</param>
        /// <param name="OrderType">����ʽ(0����1Ϊ����)</param>
        /// <param name="strWhere">������������䣬����Ҫ�ټ�WHERE�ؼ���</param>
        /// <returns></returns>
        public static string ConstructSplitSQL_TOP(string tblName,
                                                    string fldName,
                                                    int PageIndex,
                                                    int PageSize,
                                                    string rtnFields,
                                                    int OrderType,
                                                    string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // ���������������ַ���
            if (strWhere != "")
            {
                // ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            // �������
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += strWhere + " ORDER BY " + fldName + " ASC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                }
            }
            // �������
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += strWhere + " ORDER BY " + fldName + " DESC";
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                }
            }
            else // �쳣����
            {
                throw new DataException("δָ���κ��������͡�0����1Ϊ����");
            }

            return strSQL;
        }

        #endregion


        #region ConstructSplitSQL_sort(ָ������ı��ʽ)

        /// <summary>
        /// ��ȡ��ҳ����SQL���(����������ֶα��뽨������)
        /// </summary>
        /// <param name="tblName">��������</param>
        /// <param name="fldName">���������ֶ�����</param>
        /// <param name="PageIndex">��ǰҳ</param>
        /// <param name="PageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="rtnFields">�����ֶμ��ϣ��м��ö��Ÿ񿪡�����ȫ���á�*��</param>
        /// <param name="OrderType">����ʽ(0����1Ϊ����)</param>
        /// <param name="sort">������ʽ</param>
        /// <param name="strWhere">������������䣬����Ҫ�ټ�WHERE�ؼ���</param>
        /// <returns></returns>
        public static string ConstructSplitSQL_sort(string tblName,
            string fldName,
            int PageIndex,
            int PageSize,
            string rtnFields,
            int OrderType,
            string sort,
            string strWhere)
        {
            string strSQL = "";
            string strOldWhere = "";

            // ���������������ַ���
            if (strWhere != "")
            {
                // ȥ�����Ϸ����ַ�����ֹSQLע��ʽ����
                strWhere = strWhere.Replace("'", "''");
                strWhere = strWhere.Replace("--", "");
                strWhere = strWhere.Replace(";", "");

                strOldWhere = " AND " + strWhere + " ";

                strWhere = " WHERE " + strWhere + " ";
            }

            if (sort != "") sort = " ORDER BY " + sort;

            // �������
            if (OrderType == 0)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " >= ( SELECT MAX(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " ASC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " ASC";
                    strSQL += strWhere + sort;
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " > ( SELECT MAX(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + sort + " ) AS T )) ";

                    strSQL += strOldWhere + sort;
                }
            }
            // �������
            else if (OrderType == 1)
            {
                if (PageIndex == 1)
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    //strSQL += "WHERE (" + fldName + " <= ( SELECT MIN(" + fldName + ") FROM (SELECT TOP 1 " + fldName + " FROM " + tblName + strWhere + " ORDER BY " + fldName + " DESC ) AS T )) ";

                    //strSQL += strOldWhere + "ORDER BY " + fldName + " DESC";
                    strSQL += strWhere + sort;
                }
                else
                {
                    strSQL += "SELECT TOP " + PageSize + " " + rtnFields + " FROM " + tblName + " ";

                    strSQL += "WHERE (" + fldName + " < ( SELECT MIN(" + fldName + ") FROM (SELECT TOP " + ((PageIndex - 1) * PageSize) + " " + fldName + " FROM " + tblName + strWhere + sort + " ) AS T )) ";

                    strSQL += strOldWhere + sort;
                }
            }
            else // �쳣����
            {
                throw new DataException("δָ���������������͡�0����1Ϊ����");
            }

            return strSQL;
        }

        #endregion

        #endregion
    }
}
