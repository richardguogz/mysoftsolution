using System;
using System.Text;
using System.Security.Cryptography;

namespace MySoft.Common
{
    /// <summary>
    /// RSACryption : ����RSA���ԳƼ��ܷ�ʽ�ļ���/���ܼ�����ǩ����
    /// </summary>
    public class RSACrypto
    {
        #region RSA ���ܽ���

        #region RSA ����Կ����
        /// <summary>
        /// ����˽Կ �͹�Կ 
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="xmlPublicKey"></param>
        public void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                xmlKeys = rsa.ToXmlString(true);
                xmlPublicKey = rsa.ToXmlString(false);
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region RSA�ļ��ܺ���
        //############################################################################## 
        //RSA ��ʽ���� 
        //˵��KEY������XML����ʽ,���ص����ַ��� 
        //����һ����Ҫ˵�������ü��ܷ�ʽ�� ���� ���Ƶģ��� 
        //############################################################################## 
        /*

                /// <summary>
                /// RSA�ļ��ܺ���
                /// </summary>
                /// <param name="xmlPublicKey"></param>
                /// <param name="m_strEncryptString"></param>
                /// <returns></returns>
                public string RSAEncrypt1(string xmlPublicKey,string m_strEncryptString )
                { 
                    try 
                    { 
                        byte[] PlainTextBArray; 
                        byte[] CypherTextBArray; 
                        string Result; 
                        System.Security.Cryptography.RSACryptoServiceProvider rsa=new RSACryptoServiceProvider(); 
                        rsa.FromXmlString(xmlPublicKey); 

                        PlainTextBArray = Encoding.UTF8.GetBytes(m_strEncryptString);
                        //System.Web.HttpContext.Current.Response.Write(PlainTextBArray.Length + "<br>");

                        CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                    //	System.Web.HttpContext.Current.Response.Write(CypherTextBArray.Length + "<br>");

                        Result = Convert.ToBase64String(CypherTextBArray);
                        System.Web.HttpContext.Current.Response.Write(Result.Length);
                        //Result = Encoding.UTF8.GetString(CypherTextBArray);
                        return Result; 
                    } 
                    catch(Exception ex) 
                    { 
                        throw ex; 
                    } 
                } 
        */
        /// <summary>
        /// RSA�ļ��ܺ���
        /// </summary>
        /// <param name="xmlPublicKey"></param>
        /// <param name="m_strEncryptString"></param>
        /// <returns></returns>
        public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                //PlainTextBArray = (new UnicodeEncoding()).GetBytes(m_strEncryptString);
                PlainTextBArray = Encoding.UTF8.GetBytes(m_strEncryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                //	System.Web.HttpContext.Current.Response.Write(CypherTextBArray.Length + "<br>");
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSA�ļ��ܺ��� 
        /// </summary>
        /// <param name="xmlPublicKey"></param>
        /// <param name="EncryptString"></param>
        /// <returns></returns>
        public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {
            try
            {
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                CypherTextBArray = rsa.Encrypt(EncryptString, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region RSA�Ľ��ܺ���
        /// <summary>
        /// RSA�Ľ��ܺ��� 
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <param name="m_strDecryptString"></param>
        /// <returns></returns>
        public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
                Result = Encoding.UTF8.GetString(DypherTextBArray);
                return Result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSA�Ľ��ܺ���
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <param name="DecryptString"></param>
        /// <returns></returns>

        public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            try
            {
                byte[] DypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPrivateKey);
                DypherTextBArray = rsa.Decrypt(DecryptString, false);
                Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
                return Result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region RSA ����ǩ��

        #region ��ȡHash������

        /// <summary>
        /// ��ȡHash������ 
        /// </summary>
        /// <param name="m_strSource"></param>
        /// <param name="HashData"></param>
        /// <returns></returns>
        public bool GetHash(string m_strSource, ref byte[] HashData)
        {
            try
            {
                //���ַ�����ȡ��Hash���� 
                byte[] Buffer;
                System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
                Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
                HashData = MD5.ComputeHash(Buffer);

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ��ȡHash������
        /// </summary>
        /// <param name="m_strSource"></param>
        /// <param name="strHashData"></param>
        /// <returns></returns>
        public bool GetHash(string m_strSource, ref string strHashData)
        {
            try
            {
                //���ַ�����ȡ��Hash���� 
                byte[] Buffer;
                byte[] HashData;
                System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
                Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
                HashData = MD5.ComputeHash(Buffer);

                strHashData = Convert.ToBase64String(HashData);
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ��ȡHash������ 
        /// </summary>
        /// <param name="objFile"></param>
        /// <param name="HashData"></param>
        /// <returns></returns>
        public bool GetHash(System.IO.FileStream objFile, ref byte[] HashData)
        {
            try
            {
                //���ļ���ȡ��Hash���� 
                System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
                HashData = MD5.ComputeHash(objFile);
                objFile.Close();

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ��ȡHash������
        /// </summary>
        /// <param name="objFile"></param>
        /// <param name="strHashData"></param>
        /// <returns></returns>
        public bool GetHash(System.IO.FileStream objFile, ref string strHashData)
        {
            try
            {
                //���ļ���ȡ��Hash���� 
                byte[] HashData;
                System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
                HashData = MD5.ComputeHash(objFile);
                objFile.Close();

                strHashData = Convert.ToBase64String(HashData);

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region RSAǩ��
        /// <summary>
        /// RSAǩ��
        /// </summary>
        /// <param name="p_strKeyPrivate"></param>
        /// <param name="HashbyteSignature"></param>
        /// <param name="EncryptedSignatureData"></param>
        /// <returns></returns>

        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //����ǩ�����㷨ΪMD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //ִ��ǩ�� 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSAǩ�� 
        /// </summary>
        /// <param name="p_strKeyPrivate"></param>
        /// <param name="HashbyteSignature"></param>
        /// <param name="m_strEncryptedSignatureData"></param>
        /// <returns></returns>
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
        {
            try
            {
                byte[] EncryptedSignatureData;

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //����ǩ�����㷨ΪMD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //ִ��ǩ�� 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSAǩ��
        /// </summary>
        /// <param name="p_strKeyPrivate"></param>
        /// <param name="m_strHashbyteSignature"></param>
        /// <param name="EncryptedSignatureData"></param>
        /// <returns></returns>

        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {
            try
            {
                byte[] HashbyteSignature;

                HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //����ǩ�����㷨ΪMD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //ִ��ǩ�� 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSAǩ��
        /// </summary>
        /// <param name="p_strKeyPrivate"></param>
        /// <param name="m_strHashbyteSignature"></param>
        /// <param name="m_strEncryptedSignatureData"></param>
        /// <returns></returns>

        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
        {
            try
            {
                byte[] HashbyteSignature;
                byte[] EncryptedSignatureData;

                HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPrivate);
                System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
                //����ǩ�����㷨ΪMD5 
                RSAFormatter.SetHashAlgorithm("MD5");
                //ִ��ǩ�� 
                EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

                m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion


        #region RSA ǩ����֤

        /// <summary>
        ///  RSA ǩ����֤ 
        /// </summary>
        /// <param name="p_strKeyPublic"></param>
        /// <param name="HashbyteDeformatter"></param>
        /// <param name="DeformatterData"></param>
        /// <returns></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //ָ�����ܵ�ʱ��HASH�㷨ΪMD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSA ǩ����֤
        /// </summary>
        /// <param name="p_strKeyPublic"></param>
        /// <param name="p_strHashbyteDeformatter"></param>
        /// <param name="DeformatterData"></param>
        /// <returns></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
        {
            try
            {
                byte[] HashbyteDeformatter;

                HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //ָ�����ܵ�ʱ��HASH�㷨ΪMD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSA ǩ����֤
        /// </summary>
        /// <param name="p_strKeyPublic"></param>
        /// <param name="HashbyteDeformatter"></param>
        /// <param name="p_strDeformatterData"></param>
        /// <returns></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
        {
            try
            {
                byte[] DeformatterData;

                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //ָ�����ܵ�ʱ��HASH�㷨ΪMD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                DeformatterData = Convert.FromBase64String(p_strDeformatterData);

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// RSA ǩ����֤
        /// </summary>
        /// <param name="p_strKeyPublic"></param>
        /// <param name="p_strHashbyteDeformatter"></param>
        /// <param name="p_strDeformatterData"></param>
        /// <returns></returns>
        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
        {
            try
            {
                byte[] DeformatterData;
                byte[] HashbyteDeformatter;

                HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
                System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

                RSA.FromXmlString(p_strKeyPublic);
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
                //ָ�����ܵ�ʱ��HASH�㷨ΪMD5 
                RSADeformatter.SetHashAlgorithm("MD5");

                DeformatterData = Convert.FromBase64String(p_strDeformatterData);

                if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }


        #endregion

        #endregion
    }
}
