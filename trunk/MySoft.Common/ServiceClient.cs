using System;
using System.Collections;
using System.Net;

namespace MySoft.Common
{
    /// <summary>
    /// ServiceMethod : �ͻ��˿��Ե��÷�����ࡣ
    /// </summary>
    public abstract class ServiceClient
    {
        private static readonly CustomService cs = new CustomService();

        /// <summary>
        /// ���� byte[] ���ݣ���ѡ����ܻ����ķ���
        /// </summary>
        /// <param name="PostUri"></param>
        /// <param name="Content">�����ܵ�����</param>
        /// <param name="IsEncryptoSend">�Ƿ���ܷ���</param>
        /// <param name="enableKeyIV">�Ƿ�����DES�ļ����㷨Key,IVʹ��һ�µ�</param>
        /// <returns>WebResponse</returns>
        public static WebResponse Send(string PostUri, string Content, bool IsEncryptoSend, bool enableKeyIV)
        {
            if (IsEncryptoSend)
            {
                string publicKey = FileHelper.ReadFile(FunctionHelper.GetRealFile(FunctionHelper.GetAppSettings("publicKey")));

                string desKey = FunctionHelper.Text.RandomSTR(8);
                string desIV = desKey;

                string rsaDes = "";

                string encryptContent = cs.EncryptString(Content, publicKey, desKey, desIV, out rsaDes);

                Hashtable ht = new Hashtable();
                ht.Add("CS_DESSTRING", rsaDes);

                return cs.Send(PostUri, encryptContent, ht);
            }
            else
            {
                return cs.Send(PostUri, Content);
            }
        }


        /// <summary>
        /// ���� byte[] ���ݣ���ѡ����ܻ����ķ���
        /// </summary>
        /// <param name="PostUri"></param>
        /// <param name="pui">�����ܵ�����</param>
        /// <param name="IsEncryptoSend">�Ƿ���ܷ���</param>
        /// <param name="enableKeyIV">�Ƿ�����DES�ļ����㷨Key,IVʹ��һ�µ�</param>
        /// <returns>WebResponse</returns>
        public static WebResponse Send(string PostUri, PostUserInfo pui, bool IsEncryptoSend, bool enableKeyIV)
        {
            byte[] bufferContent = FormatterHelper.Serialize(pui);
            string str = Convert.ToBase64String(bufferContent);

            if (IsEncryptoSend)
            {
                string publicKey = FileHelper.ReadFile(FunctionHelper.GetRealFile(FunctionHelper.GetAppSettings("publicKey")));

                string desKey = FunctionHelper.Text.RandomSTR(8);
                string desIV = desKey;

                string rsaDes = "";


                string encryptContent = cs.EncryptString(bufferContent, publicKey, desKey, desIV, out rsaDes);

                Hashtable ht = new Hashtable();
                ht.Add("CS_DESSTRING", rsaDes);

                return cs.Send(PostUri, encryptContent, ht);
            }
            else
            {
                return cs.Send(PostUri, str);
            }
        }


        /// <summary>
        /// ���ͷ���ȡ���շ����ص���Ϣ
        /// </summary>
        /// <param name="res">���ظ����ͷ��� Response ����</param>
        /// <param name="CS_RESULT">���ز������</param>
        /// <returns>string</returns>
        public static string GetResponseContent(WebResponse res, out string CS_RESULT)
        {
            CS_RESULT = FunctionHelper.CheckValiable(res.Headers["CS_RESULT"]) ? res.Headers["CS_RESULT"] : "";

            return cs.GetResponseStream(res);
        }

        /// <summary> 
        /// ����ָ������Ϣ������Զ��WebService���� 
        /// </summary> 
        /// <param name="url">WebService��http��ʽ�ĵ�ַ</param> 
        /// <param name="namespace">�����õ�WebService�������ռ�</param> 
        /// <param name="classname">�����õ�WebService�������������������ռ�ǰ׺��</param> 
        /// <param name="methodname">�����õ�WebService�ķ�����</param> 
        /// <param name="args">�����б�</param> 
        /// <returns>WebService��ִ�н��</returns> 
        /// <remarks> 
        /// �������ʧ�ܣ������׳�Exception������õ�ʱ���ʵ��ػ��쳣�� 
        /// �쳣��Ϣ���ܻᷢ���������ط��� 
        /// 1����̬����WebService��ʱ��CompileAssemblyʧ�ܡ� 
        /// 2��WebService����ִ��ʧ�ܡ� 
        /// </remarks> 
        /// <example> 
        /// <code> 
        /// object obj = InvokeWebservice("http://localhost/GSP_WorkflowWebservice/common.asmx","Genersoft.Platform.Service.Workflow","Common","GetToolType",new object[]{"1"}); 
        /// </code> 
        /// </example> 
        public static object InvokeWebservice(string url, string @namespace, string classname, string methodname, params object[] args)
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                System.IO.Stream stream = wc.OpenRead(url + "?WSDL");
                System.Web.Services.Description.ServiceDescription sd = System.Web.Services.Description.ServiceDescription.Read(stream);
                System.Web.Services.Description.ServiceDescriptionImporter sdi = new System.Web.Services.Description.ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                System.CodeDom.CodeNamespace cn = new System.CodeDom.CodeNamespace(@namespace);
                System.CodeDom.CodeCompileUnit ccu = new System.CodeDom.CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);

                Microsoft.CSharp.CSharpCodeProvider icc = new Microsoft.CSharp.CSharpCodeProvider();
                System.CodeDom.Compiler.CompilerParameters cplist = new System.CodeDom.Compiler.CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                System.CodeDom.Compiler.CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
    }
}
