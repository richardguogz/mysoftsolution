using System.Security.Cryptography;
using System.Text;

namespace MySoft.Common
{
    /// <summary>
    /// SymmCrypto : ʵ��.Net����µĴ���Կ�ļ���/�����㷨��װ�ࡣ
    /// </summary>
    public class SymmCrypto
    {
        /// <summary>
        /// ����/�����㷨�ķ�ʽ
        /// </summary>
        public enum SymmProvEnum : int
        {
            /// <summary>
            /// DES�㷨
            /// </summary>
            DES,

            /// <summary>
            /// RC2�㷨
            /// </summary>
            RC2,

            /// <summary>
            /// Rijndael�㷨
            /// </summary>
            Rijndael
        }

        private SymmetricAlgorithm mobjCryptoService;

        /// <remarks> 
        /// ʹ��.Net SymmetricAlgorithm ��Ĺ�����. 
        /// </remarks> 
        public SymmCrypto(SymmProvEnum NetSelected)
        {
            switch (NetSelected)
            {
                case SymmProvEnum.DES:
                    mobjCryptoService = new DESCryptoServiceProvider();
                    break;
                case SymmProvEnum.RC2:
                    mobjCryptoService = new RC2CryptoServiceProvider();
                    break;
                case SymmProvEnum.Rijndael:
                    mobjCryptoService = new RijndaelManaged();
                    break;
            }
        }

        /// <remarks> 
        /// ʹ���Զ���SymmetricAlgorithm��Ĺ�����. 
        /// </remarks> 
        public SymmCrypto(SymmetricAlgorithm ServiceProvider)
        {
            mobjCryptoService = ServiceProvider;
        }

        /// <remarks> 
        /// Depending on the legal key size limitations of  
        /// a specific CryptoService provider and length of  
        /// the private key provided, padding the secret key  
        /// with space character to meet the legal size of the algorithm. 
        /// </remarks> 
        private byte[] GetLegalKey(string Key)
        {
            string sTemp;
            if (mobjCryptoService.LegalKeySizes.Length > 0)
            {
                int lessSize = 0, moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;
                // key sizes are in bits 
                while (Key.Length * 8 > moreSize)
                {
                    lessSize = moreSize;
                    moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
                }
                sTemp = Key.PadRight(moreSize / 16, ' ');
            }
            else
                sTemp = Key;

            // convert the secret key to byte array 
            return Encoding.UTF8.GetBytes(sTemp);
        }


        /// <summary>
        /// ���ַ���������Կ����
        /// </summary>
        public string Encrypting(string Source, string Key)
        {
            byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(Source);
            // create a MemoryStream so that the process can be done without I/O files 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            byte[] bytKey = GetLegalKey(Key);

            // set the private key 
            mobjCryptoService.Key = bytKey;
            mobjCryptoService.IV = bytKey;

            // create an Encryptor from the Provider Service instance 
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();

            // create Crypto Stream that transforms a stream using the encryption 
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

            // write out encrypted content into MemoryStream 
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();

            // get the output and trim the '\0' bytes 
            byte[] bytOut = ms.GetBuffer();
            int i = 0;
            for (i = 0; i < bytOut.Length; i++)
                if (bytOut[i] == 0)
                    break;

            // convert into Base64 so that the result can be used in xml 
            return System.Convert.ToBase64String(bytOut, 0, i);
        }

        /// <summary>
        /// ���ַ���������Կ����
        /// </summary>
        public string Decrypting(string Source, string Key)
        {
            // �� Base64 ת��Ϊ������
            byte[] bytIn = System.Convert.FromBase64String(Source);

            // Ϊ������ڴ�ռ�
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

            byte[] bytKey = GetLegalKey(Key);

            // ���ý�����Կ
            mobjCryptoService.Key = bytKey;
            mobjCryptoService.IV = bytKey;

            // create a Decryptor from the Provider Service instance
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

            // create Crypto Stream that transforms a stream using the decryption
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            // read out the result from the Crypto Stream
            System.IO.StreamReader sr = new System.IO.StreamReader(cs);
            return sr.ReadToEnd();
        }
    }

}
