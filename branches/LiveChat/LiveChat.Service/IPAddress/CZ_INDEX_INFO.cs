using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LiveChat.Service
{
    //�����࣬���ڱ���IP������Ϣ
    public class CZ_INDEX_INFO
    {
        public UInt32 IpSet;
        public UInt32 IpEnd;
        public UInt32 Offset;

        public CZ_INDEX_INFO()
        {
            IpSet = 0;
            IpEnd = 0;
            Offset = 0;
        }
    }
}
