/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com QQ:547386448
 *  Updated 2011-04-9 
 */
using System;
using System.Text;
using System.Threading;

namespace MySoft.Net.Sockets
{
    /// <summary>
    /// 数据包在读取前需要额外的处理回调方法。（例如解密，解压缩等）
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public delegate byte[] RDataExtraHandle(byte[] data);

    /// <summary>
    /// 数据包读取类
    /// （此类的功能是讲通讯数据包重新转换成.NET 数据类型）
    /// </summary>
    public class BufferReader
    {
        private int current;

        public byte[] Data { get; set; }

        private int startIndex;
        private int endlengt;

        /// <summary>
        /// 额外处理是否调用成功，可以判断是否解密成功
        /// </summary>
        public bool IsDataExtraSuccess { get; set; }

        /// <summary>
        /// 数据包长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 当前其位置
        /// </summary>
        public int Postion
        {
            get
            {
                return current;
            }

            set
            {
                Interlocked.Exchange(ref current, value);
            }
        }

        public void Reset()
        {
            current = 0;
        }


        public BufferReader(Byte[] data)
        {
            Data = data;
            this.Length = Data.Length;
            current = 0;
            IsDataExtraSuccess = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex">需要载入数据额外处理的开始位置</param>
        /// <param name="length">需要载入数据额外处理的数据长度 -1为，开始INDEX到结束位置,-2就是保留最后1位</param>
        ///  <param name="dataExtraCallBack"> 数据包在读取前需要额外的处理回调方法。（例如加密，解压缩等）</param>
        public BufferReader(Byte[] data, int startIndex, int length, RDataExtraHandle dataExtraCallBack)
        {
            try
            {
                this.startIndex = startIndex;
                this.Length = data.Length;
                if (length < 0)
                {
                    endlengt = (data.Length + length + 1) - startIndex;
                }
                else
                {
                    endlengt = length;
                }

                byte[] handBytes = new byte[this.startIndex];

                Array.Copy(data, 0, handBytes, 0, handBytes.Length); //首先保存不需要解密的数组

                byte[] endBytes = new byte[data.Length - (startIndex + endlengt)];

                Array.Copy(data, (startIndex + endlengt), endBytes, 0, endBytes.Length); //首先保存不需要解密的数组

                byte[] NeedExByte = new byte[endlengt];

                Array.Copy(data, startIndex, NeedExByte, 0, NeedExByte.Length);

                if (dataExtraCallBack != null)
                    NeedExByte = dataExtraCallBack(NeedExByte);

                Data = new byte[handBytes.Length + NeedExByte.Length + endBytes.Length]; //重新整合解密完毕后的数据包

                Array.Copy(handBytes, 0, Data, 0, handBytes.Length);
                Array.Copy(NeedExByte, 0, Data, handBytes.Length, NeedExByte.Length);
                Array.Copy(endBytes, 0, Data, (handBytes.Length + NeedExByte.Length), endBytes.Length);
                current = 0;
                IsDataExtraSuccess = true;
            }
            catch
            {
                IsDataExtraSuccess = false;
            }
        }

        #region 整数
        /// <summary>
        /// 读取内存流中的头2位并转换成整型
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadInt16(out short value)
        {

            try
            {
                value = BitConverter.ToInt16(Data, current);
                current = Interlocked.Add(ref current, 2);
                return true;
            }
            catch
            {
                value = 0;
                return false;
            }
        }


        /// <summary>
        /// 读取内存流中的头4位并转换成整型
        /// </summary>
        /// <param name="ms">内存流</param>
        /// <returns></returns>
        public bool ReadInt32(out int value)
        {
            try
            {
                value = BitConverter.ToInt32(Data, current);
                current = Interlocked.Add(ref current, 4);
                return true;
            }
            catch
            {
                value = 0;
                return false;
            }
        }


        /// <summary>
        /// 读取内存流中的头8位并转换成长整型
        /// </summary>
        /// <param name="ms">内存流</param>
        /// <returns></returns>
        public bool ReadInt64(out long value)
        {
            try
            {
                value = BitConverter.ToInt64(Data, current);
                current = Interlocked.Add(ref current, 8);
                return true;
            }
            catch
            {
                value = 0;
                return false;
            }
        }

        /// <summary>
        /// 读取内存流中的首位
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadByte(out byte value)
        {
            try
            {
                value = (byte)Data[current];
                current = Interlocked.Increment(ref current);
                return true;
            }
            catch
            {
                value = 0;
                return false;
            }
        }

        #endregion

        #region 浮点数


        /// <summary>
        /// 读取内存流中的头4位并转换成单精度浮点数
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadFloat(out float value)
        {

            try
            {
                value = BitConverter.ToSingle(Data, current);
                current = Interlocked.Add(ref current, 4);
                return true;
            }
            catch
            {
                value = 0.0f;
                return false;
            }
        }


        /// <summary>
        /// 读取内存流中的头8位并转换成浮点数
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadDouble(out double value)
        {

            try
            {
                value = BitConverter.ToDouble(Data, current);
                current = Interlocked.Add(ref current, 8);
                return true;
            }
            catch
            {
                value = 0.0;
                return false;
            }
        }


        #endregion

        #region 布尔值
        /// <summary>
        /// 读取内存流中的头1位并转换成布尔值
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadBoolean(out bool value)
        {

            try
            {
                value = BitConverter.ToBoolean(Data, current);
                current = Interlocked.Add(ref current, 1);
                return true;
            }
            catch
            {
                value = false;
                return false;
            }
        }

        #endregion

        #region 字符串

        /// <summary>
        /// 读取内存流中一段字符串
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadString(out string value)
        {
            int lengt;
            try
            {
                if (ReadInt32(out lengt))
                {

                    Byte[] buf = new Byte[lengt];

                    Array.Copy(Data, current, buf, 0, buf.Length);

                    value = Encoding.Unicode.GetString(buf, 0, buf.Length);

                    current = Interlocked.Add(ref current, lengt);

                    return true;

                }
                else
                {
                    value = "";
                    return false;
                }
            }
            catch
            {
                value = "";
                return false;
            }

        }
        #endregion

        #region 数据
        /// <summary>
        /// 读取内存流中一段数据
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadByteArray(out byte[] value)
        {
            int lengt;
            try
            {
                if (ReadInt32(out lengt))
                {
                    value = new Byte[lengt];
                    Array.Copy(Data, current, value, 0, value.Length);
                    current = Interlocked.Add(ref current, lengt);
                    return true;

                }
                else
                {
                    value = null;
                    return false;
                }
            }
            catch
            {
                value = null;
                return false;
            }

        }
        #endregion

        #region 对象

        /// <summary>
        /// 读取一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ReadObject(out object obj)
        {
            byte[] data;
            if (this.ReadByteArray(out data))
            {
                obj = SerializationManager.DeserializeBin(data);
                return true;
            }
            else
            {
                obj = null;
                return false;
            }
        }

        #endregion
    }
}
