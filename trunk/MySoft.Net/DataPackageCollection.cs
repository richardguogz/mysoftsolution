namespace MySoft.Net
{
    using System;
    using System.IO;

    internal class DataPackageCollection
    {
        private DataPackage[] _datas;
        private string mID = null;

        public DataPackageCollection(int count)
        {
            this._datas = new DataPackage[count];
        }

        public void Add(DataPackage dp)
        {
            this._datas[dp.Sequence] = dp;
            if (this.mID == null)
            {
                this.mID = dp.DataID;
            }
        }

        public byte[] GetData()
        {
            MemoryStream stream = new MemoryStream();
            foreach (DataPackage package in this._datas)
            {
                stream.Write(package.Data, 0, package.Data.Length);
            }
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public static DataPackage[] GetDataPackages(byte[] bytes, int packasize)
        {
            int num = packasize - 0x30;
            int num2 = 0;
            if ((bytes.Length % num) > 0)
            {
                num2 = (bytes.Length / num) + 1;
            }
            else
            {
                num2 = bytes.Length / num;
            }
            DataPackage[] packageArray = new DataPackage[num2];
            string text = Guid.NewGuid().ToString();
            for (int i = 0; i < num2; i++)
            {
                packageArray[i] = new DataPackage();
                packageArray[i].Count = num2;
                packageArray[i].DataID = text;
                packageArray[i].Data = new byte[num];
                packageArray[i].Sequence = i;
                int index = 0;
                for (int j = num * i; j < (num * (i + 1)); j++)
                {
                    if (j < bytes.Length)
                    {
                        packageArray[i].Data[index] = bytes[j];
                    }
                    else
                    {
                        break;
                    }
                    index++;
                }
            }
            return packageArray;
        }

        public string ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool IsFull
        {
            get
            {
                for (int i = 0; i < this._datas.Length; i++)
                {
                    if (this._datas[i] == null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

