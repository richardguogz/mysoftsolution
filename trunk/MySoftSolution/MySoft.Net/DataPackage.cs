namespace MySoft.Net
{
    using System;
    using System.Text;

    internal class DataPackage
    {
        private int mCount;
        private byte[] mData;
        private string mDataID;
        private int mSequence;

        public static byte[] GetData(DataPackage dp)
        {
            byte[] array = new byte[0x30 + dp.Data.Length];
            BitConverter.GetBytes(0xff01).CopyTo(array, 0);
            BitConverter.GetBytes(dp.Count).CopyTo(array, 4);
            Encoding.ASCII.GetBytes(dp.DataID).CopyTo(array, 8);
            BitConverter.GetBytes(dp.Sequence).CopyTo(array, 0x2c);
            dp.Data.CopyTo(array, 0x30);
            return array;
        }

        public static DataPackage GetDataPackage(byte[] bytes)
        {
            DataPackage package = new DataPackage();
            package.Count = BitConverter.ToInt32(bytes, 4);
            package.DataID = Encoding.ASCII.GetString(bytes, 8, 0x24);
            package.Sequence = BitConverter.ToInt32(bytes, 0x2c);
            package.Data = new byte[bytes.Length - 0x30];
            for (int i = 0x30; i < bytes.Length; i++)
            {
                package.Data[i - 0x30] = bytes[i];
            }
            return package;
        }

        public static bool IsDataPackage(byte[] bytes)
        {
            if (bytes.Length < 4)
            {
                return false;
            }
            return (BitConverter.ToInt32(bytes, 0) == 0xff01);
        }

        public int Count
        {
            get
            {
                return this.mCount;
            }
            set
            {
                this.mCount = value;
            }
        }

        public byte[] Data
        {
            get
            {
                return this.mData;
            }
            set
            {
                this.mData = value;
            }
        }

        public string DataID
        {
            get
            {
                return this.mDataID;
            }
            set
            {
                this.mDataID = value;
            }
        }

        public int Sequence
        {
            get
            {
                return this.mSequence;
            }
            set
            {
                this.mSequence = value;
            }
        }
    }
}

