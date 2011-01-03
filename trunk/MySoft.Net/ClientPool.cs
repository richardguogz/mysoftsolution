namespace MySoft.Net
{
    using System.Collections;

    public class ClientPool
    {
        private Hashtable _pool = Hashtable.Synchronized(new Hashtable());

        public void Add(IChannel msg)
        {
            lock (_pool.SyncRoot)
            {
                this._pool.Add(msg.ID, msg);
            }
        }

        public void Clear()
        {
            this._pool.Clear();
        }

        public ICollection GetClients()
        {
            lock (_pool.SyncRoot)
            {
                ArrayList list = new ArrayList();
                foreach (object obj2 in this._pool.Keys)
                {
                    list.Add(this._pool[obj2]);
                }
                return list;
            }
        }

        public void Remove(IChannel msg)
        {
            lock (_pool.SyncRoot)
            {
                this._pool.Remove(msg.ID);
            }
        }

        public int Count
        {
            get
            {
                return this._pool.Count;
            }
        }

        public IChannel this[string id]
        {
            get
            {
                lock (_pool.SyncRoot)
                {
                    return (Channel)this._pool[id.ToLower()];
                }
            }
        }
    }
}

