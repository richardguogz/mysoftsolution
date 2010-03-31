namespace MySoft.Net.Message
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public abstract class MessageAdapter : IMessage
    {
        private string mID = Guid.NewGuid().ToString();
        private DateTime mTime = DateTime.Now;

        private static object DeserializeObject(byte[] pBytes)
        {
            object obj2 = null;
            if (pBytes != null)
            {
                MemoryStream serializationStream = new MemoryStream(pBytes);
                serializationStream.Position = 0;
                obj2 = new BinaryFormatter().Deserialize(serializationStream);
                serializationStream.Close();
            }
            return obj2;
        }

        public void Load(byte[] bytes)
        {
            object obj2 = DeserializeObject(bytes);
            this.OnLoad(obj2);
        }

        protected virtual void OnLoad(object obj)
        {
            object[] objArray = (object[]) obj;
            this.ID = (string) objArray[0];
            this.Time = (DateTime) objArray[1];
        }

        protected virtual object OnSave()
        {
            return new object[] { this.ID, this.Time };
        }

        public byte[] Save()
        {
            return SerializeObject(this.OnSave());
        }

        private static byte[] SerializeObject(object pObj)
        {
            if (pObj == null)
            {
                return null;
            }
            MemoryStream serializationStream = new MemoryStream();
            new BinaryFormatter().Serialize(serializationStream, pObj);
            serializationStream.Position = 0;
            byte[] buffer = new byte[serializationStream.Length];
            serializationStream.Read(buffer, 0, buffer.Length);
            serializationStream.Close();
            return buffer;
        }

        public string ID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.mTime;
            }
            set
            {
                this.mTime = value;
            }
        }
    }
}

