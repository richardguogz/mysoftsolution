
/* 
 * ��Ŀ����:	CustomService (�Զ������)
 * ��Ȩ����:	������
 * �ٷ���վ:	http://www.seaskyer.net/
 * ����֧��:	http://bbs.seaskyer.net/
 * ���ܼ��:    
 *				ͨ�� HTTP �� POST ��ʽ�������ݣ���ҪӦ�����������ݵļ��ܴ��䣬�û������֤���������ϵ���Ӻ��޸�
 *			�����ǲ��� RSA �� DES ���ϵķ�ʽ������������ߵ��ŵ㻥����ʵ�ִ������ݵĿ��ټ��ܺͰ�ȫ����
 * �ṩ����:
 *	Send();			��ָ�� Uri ��ַ�� POST ��ʽ��������
 *
 *	Receive();		��ȡ�� POST ��ʽ���͹����������ݺ� Header �е���Ϣ
 *
 *	GetResponseStream(); ���ͷ���ȡ���շ����ص���Ϣ
 *
 *	Encrypt();	ʹ�� DES �� RSA ���ϵļ��ܺ���
 *
 *	Decrypt();	ʹ�� DES �� RSA ���ϵĽ��ܺ���
 *
 *
 * ��������:	ŭ��.Net
 * Email   :	hktkmaster@163.com
 * QQ�� �� :	17251920			QQȺ���� :	711255
 * ��    ע:
 * 
 *		��ӭ��Һ����ǽ��������ۡ�
 *		���ǵ���ּ�ǣ��ø�����˶��ܹ�ʹ����ѡ���Դ������չ��ϵͳ��
 * 
 *														���칤����(Seasky Studio.)
 */


using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace MySoft.Common
{
    /// <summary>
    /// CustomService : �Զ������
    /// </summary>
    public class CustomService
    {
        HttpResponse Response = HttpContext.Current.Response;
        HttpRequest Request = HttpContext.Current.Request;
        //		HttpServerUtility	Server		= HttpContext.Current.Server;

        #region ���ͺͽ�������Ϣ


        #region ��������


        /// <summary>
        /// ��ָ�� Uri ��ַ�� POST ��ʽ�����ı�����
        /// </summary>
        /// <param name="PostUrl">Ŀ��Uri��ַ</param>
        /// <param name="Content">Ҫ���͵��ı�����</param>
        /// <returns>WebResponse</returns>
        public WebResponse Send(string PostUrl, string Content)
        {
            WebResponse res = null;

            try
            {
                WebRequest req = WebRequest.Create(PostUrl);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";


                byte[] bytes = Encoding.UTF8.GetBytes(Content);
                req.ContentLength = bytes.Length;

                Stream newStream = req.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                res = req.GetResponse();

            }
            catch (Exception exc)
            {
                Response.Write(exc.ToString());
            }

            return res;
        }

        /// <summary>
        /// ��ָ�� Uri ��ַ�� POST ��ʽ���� byte[] ����
        /// </summary>
        /// <param name="PostUrl">Ŀ��Uri��ַ</param>
        /// <param name="Content">Ҫ���͵��ı�����</param>
        /// <returns>WebResponse</returns>
        public WebResponse Send(string PostUrl, byte[] Content)
        {
            WebResponse res = null;

            try
            {
                WebRequest req = WebRequest.Create(PostUrl);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";


                byte[] bytes = Content;
                req.ContentLength = bytes.Length;

                Stream newStream = req.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                res = req.GetResponse();

            }
            catch (Exception exc)
            {
                Response.Write(exc.ToString());
            }

            return res;
        }

        /// <summary>
        /// ��ָ�� Uri ��ַ�� POST ��ʽ�����ı�����
        /// </summary>
        /// <param name="PostUrl">Ŀ��Uri��ַ</param>
        /// <param name="Content">Ҫ���͵��ı�����</param>
        /// <param name="headerCollection">Ҫͨ�� Header ���͵����ݼ���</param>
        /// <returns>WebResponse</returns>
        public WebResponse Send(string PostUrl, string Content, Hashtable headerCollection)
        {
            WebResponse res = null;

            try
            {
                WebRequest req = WebRequest.Create(PostUrl);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                // ������ݵ�Header��
                IDictionaryEnumerator myEnumerator = headerCollection.GetEnumerator();

                while (myEnumerator.MoveNext())
                {
                    req.Headers.Add(myEnumerator.Key.ToString(), myEnumerator.Value.ToString());
                }

                byte[] bytes = Encoding.UTF8.GetBytes(Content);
                req.ContentLength = bytes.Length;

                Stream newStream = req.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                res = req.GetResponse();

            }
            catch (Exception exc)
            {
                Response.Write(exc.ToString());
            }

            return res;
        }


        /// <summary>
        /// ��ָ�� Uri ��ַ�� POST ��ʽ���Ͷ���������
        /// </summary>
        /// <param name="PostUrl">Ŀ��Uri��ַ</param>
        /// <param name="Content">Ҫ���͵Ķ���������</param>
        /// <param name="headerCollection">Ҫͨ�� Header ���͵����ݼ���</param>
        /// <returns>WebResponse</returns>
        public WebResponse Send(string PostUrl, byte[] Content, Hashtable headerCollection)
        {
            WebResponse res = null;

            try
            {

                WebRequest req = WebRequest.Create(PostUrl);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                // ������ݵ�Header��
                IDictionaryEnumerator myEnumerator = headerCollection.GetEnumerator();

                while (myEnumerator.MoveNext())
                {
                    req.Headers.Add(myEnumerator.Key.ToString(), myEnumerator.Value.ToString());
                }

                byte[] bytes = Content;
                req.ContentLength = bytes.Length;

                Stream newStream = req.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();

                res = req.GetResponse();
            }
            catch (Exception exc)
            {
                Response.Write(exc.ToString());
            }

            return res;
        }
        #endregion


        #region ��������
        /// <summary>
        /// ��ȡ�� POST ��ʽ���͹����������ݺ� Header �е���Ϣ
        /// </summary>
        /// <param name="RsaDESSTRING">����DES���ܽ��ܵ� Key �� IV �ļ���</param>
        /// <returns>string</returns>
        public string ReceiveToString(out string RsaDESSTRING)
        {
            if (Request.RequestType != "POST")
            {
                RsaDESSTRING = "";

                return "";
            }


            try
            {
                Stream stream = Request.InputStream;
                string strResult = "";

                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                char[] read = new char[256];
                int count = sr.Read(read, 0, 256);
                int i = 0;
                while (count > 0)
                {
                    i += Encoding.UTF8.GetByteCount(read, 0, 256);
                    string str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }



                RsaDESSTRING = FunctionUtils.CheckValiable(Request.Headers["CS_DESSTRING"]) ? Request.Headers["CS_DESSTRING"] : "";

                return strResult;

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// ��ȡ�� POST ��ʽ���͹����������ݺ� Header �е���Ϣ
        /// </summary>
        /// <param name="RsaDESSTRING">����DES���ܽ��ܵ� Key �� IV �ļ���</param>
        /// <returns>byte[]</returns>
        public byte[] ReceiveToBytes(out string RsaDESSTRING)
        {
            if (Request.RequestType != "POST")
            {
                RsaDESSTRING = "";

                return null;
            }


            try
            {
                Stream stream = Request.InputStream;




                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                byte[] buffer = new byte[(int)stream.Length];
                stream.Write(buffer, 0, buffer.Length);



                RsaDESSTRING = FunctionUtils.CheckValiable(Request.Headers["CS_DESSTRING"]) ? Request.Headers["CS_DESSTRING"] : "";

                return buffer;

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        #endregion


        #region ���ͷ���ȡ���շ����ص���Ϣ
        /// <summary>
        /// ���ͷ���ȡ���շ����ص���Ϣ
        /// </summary>
        /// <param name="res">���ظ����ͷ��� Response ����</param>
        /// <returns>string</returns>
        public string GetResponseStream(WebResponse res)
        {
            string strResult = "";

            Stream ResponseStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(ResponseStream, Encoding.UTF8);

            Char[] read = new Char[256];

            // Read 256 charcters at a time.    
            int count = sr.Read(read, 0, 256);

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                string str = new String(read, 0, count);
                strResult += str;
                count = sr.Read(read, 0, 256);
            }


            // �ͷ���Դ
            sr.Close();
            res.Close();

            return strResult;
        }
        #endregion


        #endregion

        #region ���ݴ���ļ���/����


        #region ���ݼ���
        /// <summary>
        /// �����ı������� string
        /// </summary>
        /// <param name="Content">��������</param>
        /// <param name="publicKey">��Կ(XML��ʽ)</param>
        /// <param name="desKey">DES��Կ</param>
        /// <param name="desIV">DES����</param>
        /// <param name="rsaDes">��RSA���ܺ��desKey��desIV�ļ���</param>
        /// <returns>string</returns>
        public string EncryptString(string Content, string publicKey, string desKey, string desIV, out string rsaDes)
        {
            string strResult = "";

            if (FunctionUtils.CheckValiable(publicKey))
            {
                // DES��������
                DESCrypto DC = new DESCrypto();
                strResult = DC.EncryptString(Content, desKey, desIV);

                // ����DES��Կ�ͳ�ʼ������
                RSACrypto RC = new RSACrypto();

                string des = desKey + "��" + desIV;
                rsaDes = RC.RSAEncrypt(publicKey, des);
            }
            else
            {
                rsaDes = "";
                strResult = Content;
            }


            return strResult;
        }


        /// <summary>
        /// �����ı������� byte[]
        /// </summary>
        /// <param name="Content">��������</param>
        /// <param name="publicKey">��Կ(XML��ʽ)</param>
        /// <param name="desKey">DES��Կ</param>
        /// <param name="desIV">DES����</param>
        /// <param name="rsaDes">��RSA���ܺ��desKey��desIV�ļ���</param>
        /// <returns>byte[]</returns>
        public byte[] EncryptBytes(string Content, string publicKey, string desKey, string desIV, out string rsaDes)
        {
            byte[] byteResult = null;

            if (FunctionUtils.CheckValiable(publicKey))
            {
                // DES��������
                DESCrypto DC = new DESCrypto();
                byteResult = DC.EncryptBytes(Content, desKey, desIV);

                // ����DES��Կ�ͳ�ʼ������
                RSACrypto RC = new RSACrypto();

                string des = desKey + "��" + desIV;
                rsaDes = RC.RSAEncrypt(publicKey, des);

            }
            else
            {
                rsaDes = "";
                byteResult = Encoding.UTF8.GetBytes(Content);
            }


            return byteResult;
        }


        /// <summary>
        /// ���� byte[] ������ string
        /// </summary>
        /// <param name="Content">��������</param>
        /// <param name="publicKey">��Կ(XML��ʽ)</param>
        /// <param name="desKey">DES��Կ</param>
        /// <param name="desIV">DES����</param>
        /// <param name="rsaDes">��RSA���ܺ��desKey��desIV�ļ���</param>
        /// <returns>string</returns>
        public string EncryptString(byte[] Content, string publicKey, string desKey, string desIV, out string rsaDes)
        {
            string strResult = "";

            if (FunctionUtils.CheckValiable(publicKey))
            {
                // DES��������
                DESCrypto DC = new DESCrypto();
                strResult = DC.EncryptString(Content, desKey, desIV);

                // ����DES��Կ�ͳ�ʼ������
                RSACrypto RC = new RSACrypto();

                string des = desKey + "��" + desIV;
                rsaDes = RC.RSAEncrypt(publicKey, des);
            }
            else
            {
                rsaDes = "";
                strResult = Encoding.UTF8.GetString(Content);
            }


            return strResult;
        }


        /// <summary>
        /// ���� byte[] ������ byte[]
        /// </summary>
        /// <param name="Content">��������</param>
        /// <param name="publicKey">��Կ(XML��ʽ)</param>
        /// <param name="desKey">DES��Կ</param>
        /// <param name="desIV">DES����</param>
        /// <param name="rsaDes">��RSA���ܺ��desKey��desIV�ļ���</param>
        /// <returns>byte[]</returns>
        public byte[] EncryptBytes(byte[] Content, string publicKey, string desKey, string desIV, out string rsaDes)
        {
            byte[] byteResult = null;

            if (FunctionUtils.CheckValiable(publicKey))
            {
                // DES��������
                DESCrypto DC = new DESCrypto();
                byteResult = DC.EncryptBytes(Content, desKey, desIV);

                // ����DES��Կ�ͳ�ʼ������
                RSACrypto RC = new RSACrypto();

                string des = desKey + "��" + desIV;
                rsaDes = RC.RSAEncrypt(publicKey, des);
            }
            else
            {
                rsaDes = "";
                byteResult = Content;
            }


            return byteResult;
        }
        #endregion


        #region ���ݽ���
        /// <summary>
        /// ���ܺ���
        /// </summary>
        /// <param name="Content">��������</param>
        /// <param name="privateKey">˽Կ(XML��ʽ)</param>
        /// <param name="rsaDes">��RSA���ܺ��desKey��desIV�ļ���</param>
        /// <param name="desKey">��RSA���ܺ��desKey</param>
        /// <param name="desIV">��RSA���ܺ��desKey</param>
        /// <returns>string</returns>
        public string DecryptString(string Content, string privateKey, string rsaDes, out string desKey, out string desIV)
        {
            string strResult = "";

            if (FunctionUtils.CheckValiable(rsaDes))
            {
                // ����DES��Կ�ͳ�ʼ������
                RSACrypto RC = new RSACrypto();

                string des = RC.RSADecrypt(privateKey, rsaDes);

                string[] desArray = FunctionUtils.SplitArray(des, '��');

                desKey = desArray[0];
                desIV = desArray[1];


                // DES��������
                DESCrypto DC = new DESCrypto();
                strResult = DC.DecryptString(Content, desKey, desIV);
            }
            else
            {
                desKey = "";
                desIV = "";
                strResult = Content;
            }


            return strResult;
        }



        /// <summary>
        /// ���ܺ���
        /// </summary>
        /// <param name="Content">��������</param>
        /// <param name="privateKey">˽Կ(XML��ʽ)</param>
        /// <param name="rsaDes">��RSA���ܺ��desKey��desIV�ļ���</param>
        /// <param name="desKey">��RSA���ܺ��desKey</param>
        /// <param name="desIV">��RSA���ܺ��desKey</param>
        /// <returns>byte[]</returns>
        public byte[] DecryptBytes(string Content, string privateKey, string rsaDes, out string desKey, out string desIV)
        {
            byte[] byteResult = null;

            if (FunctionUtils.CheckValiable(rsaDes))
            {
                // ����DES��Կ�ͳ�ʼ������
                RSACrypto RC = new RSACrypto();

                string des = RC.RSADecrypt(privateKey, rsaDes);

                string[] desArray = FunctionUtils.SplitArray(des, '��');

                desKey = desArray[0];
                desIV = desArray[1];


                // DES��������
                DESCrypto DC = new DESCrypto();
                byteResult = DC.DecryptBytes(Content, desKey, desIV);

            }
            else
            {
                desKey = "";
                desIV = "";

                byteResult = Convert.FromBase64String(Content);
            }


            return byteResult;
        }
        #endregion


        #endregion
    }
}
