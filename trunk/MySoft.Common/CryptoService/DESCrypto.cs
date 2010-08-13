using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MySoft.Common
{
    /// <summary>
    /// DESCrypto : ����DES�ԳƼ��ܷ�ʽ�ļ���/���ܡ�
    /// </summary>
    public class DESCrypto
    {
        #region ���ܷ���
        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="inputByteArray">Ҫ���ܵ�byte[]����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <param name="sIV">����ʹ�õ���Կ��ʼ������</param>
        /// <returns>string</returns>
        public string EncryptString(byte[] inputByteArray, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //�������ܶ������Կ��ƫ����  
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sIV);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            string strResult = Convert.ToBase64String(ms.ToArray());

            return strResult;
        }


        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="pToEncrypt">Ҫ���ܵ��ı�����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <param name="sIV">����ʹ�õ���Կ��ʼ������</param>
        /// <returns>string</returns>
        public string EncryptString(string pToEncrypt, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //���ַ����ŵ�byte������  
            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);

            //�������ܶ������Կ��ƫ����  
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sIV);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            string strResult = Convert.ToBase64String(ms.ToArray());

            return strResult;
        }


        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="inputByteArray">Ҫ���ܵ�byte[]����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <param name="sIV">����ʹ�õ���Կ��ʼ������</param>
        /// <returns>byte[]</returns>
        public byte[] EncryptBytes(byte[] inputByteArray, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //�������ܶ������Կ��ƫ����  
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sIV);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }


        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="pToEncrypt">Ҫ���ܵ��ı�����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <param name="sIV">����ʹ�õ���Կ��ʼ������</param>
        /// <returns>byte[]</returns>
        public byte[] EncryptBytes(string pToEncrypt, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //���ַ����ŵ�byte������  
            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);

            //�������ܶ������Կ��ƫ����  
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sIV);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        #endregion

        #region ���ܷ���
        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="pToDecrypt">Ҫ���ܵ��ı�����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <returns>string</returns>
        public string DecryptString(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();


            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);

            //�������ܶ������Կ��ƫ����
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sKey);


            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  streamn
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            ms.Close();
            cs.Close();


            return Encoding.UTF8.GetString(ms.ToArray());
        }


        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="pToDecrypt">Ҫ���ܵ��ı�����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <param name="sIV">����ʹ�õ���Կ��ʼ������</param>
        /// <returns>string</returns>
        public string DecryptString(string pToDecrypt, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();


            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);


            //�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sIV);


            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  streamn
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            ms.Close();
            cs.Close();


            return Encoding.UTF8.GetString(ms.ToArray());
        }


        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="pToDecrypt">Ҫ���ܵ��ı�����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <returns>byte[]</returns>
        public byte[] DecryptBytes(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();


            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToDecrypt);

            //�������ܶ������Կ��ƫ����
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sKey);


            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  streamn
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            ms.Close();
            cs.Close();

            return ms.ToArray();
        }

        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="pToDecrypt">Ҫ���ܵ��ı�����</param>
        /// <param name="sKey">����ʹ�õ���Կ</param>
        /// <param name="sIV">����ʹ�õ���Կ��ʼ������</param>
        /// <returns>byte[]</returns>
        public byte[] DecryptBytes(string pToDecrypt, string sKey, string sIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();


            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);


            //�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�
            des.Key = Encoding.UTF8.GetBytes(sKey);
            des.IV = Encoding.UTF8.GetBytes(sIV);


            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  streamn
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            ms.Close();
            cs.Close();

            return ms.ToArray();
        }
        #endregion
    }
}
