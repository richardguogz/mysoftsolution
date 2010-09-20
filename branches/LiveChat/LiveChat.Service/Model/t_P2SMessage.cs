namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_P2SMessage �����У�ID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_P2SMessage : Entity {
            
            protected Guid _ID;
            
            protected Guid _SID;
            
            protected String _SessionID;
            
            protected String _SenderID;
            
            protected String _SenderName;
            
            protected String _ReceiverID;
            
            protected String _ReceiverName;
            
            protected String _SenderIP;
            
            protected DateTime _SendTime;
            
            protected LiveChat.Entity.MessageType _Type;
            
            protected String _Content;
            
            protected LiveChat.Entity.MessageState _State;
            
            public Guid ID {
                get {
                    return this._ID;
                }
                set {
                    this.OnPropertyValueChange(_.ID, _ID, value);
                    this._ID = value;
                }
            }
            
            public Guid SID {
                get {
                    return this._SID;
                }
                set {
                    this.OnPropertyValueChange(_.SID, _SID, value);
                    this._SID = value;
                }
            }
            
            public String SessionID {
                get {
                    return this._SessionID;
                }
                set {
                    this.OnPropertyValueChange(_.SessionID, _SessionID, value);
                    this._SessionID = value;
                }
            }
            
            public String SenderID {
                get {
                    return this._SenderID;
                }
                set {
                    this.OnPropertyValueChange(_.SenderID, _SenderID, value);
                    this._SenderID = value;
                }
            }
            
            public String SenderName {
                get {
                    return this._SenderName;
                }
                set {
                    this.OnPropertyValueChange(_.SenderName, _SenderName, value);
                    this._SenderName = value;
                }
            }
            
            public String ReceiverID {
                get {
                    return this._ReceiverID;
                }
                set {
                    this.OnPropertyValueChange(_.ReceiverID, _ReceiverID, value);
                    this._ReceiverID = value;
                }
            }
            
            public String ReceiverName {
                get {
                    return this._ReceiverName;
                }
                set {
                    this.OnPropertyValueChange(_.ReceiverName, _ReceiverName, value);
                    this._ReceiverName = value;
                }
            }
            
            public String SenderIP {
                get {
                    return this._SenderIP;
                }
                set {
                    this.OnPropertyValueChange(_.SenderIP, _SenderIP, value);
                    this._SenderIP = value;
                }
            }
            
            public DateTime SendTime {
                get {
                    return this._SendTime;
                }
                set {
                    this.OnPropertyValueChange(_.SendTime, _SendTime, value);
                    this._SendTime = value;
                }
            }
            
            public LiveChat.Entity.MessageType Type {
                get {
                    return this._Type;
                }
                set {
                    this.OnPropertyValueChange(_.Type, _Type, value);
                    this._Type = value;
                }
            }
            
            public String Content {
                get {
                    return this._Content;
                }
                set {
                    this.OnPropertyValueChange(_.Content, _Content, value);
                    this._Content = value;
                }
            }
            
            public LiveChat.Entity.MessageState State {
                get {
                    return this._State;
                }
                set {
                    this.OnPropertyValueChange(_.State, _State, value);
                    this._State = value;
                }
            }
            
            /// <summary>
            /// ��ȡʵ���Ӧ�ı���
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_P2SMessage>("t_P2SMessage");
            }
            
            /// <summary>
            /// ��ȡʵ���е�������
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.ID};
            }
            
            /// <summary>
            /// ��ȡ����Ϣ
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.ID,
                        _.SID,
                        _.SessionID,
                        _.SenderID,
                        _.SenderName,
                        _.ReceiverID,
                        _.ReceiverName,
                        _.SenderIP,
                        _.SendTime,
                        _.Type,
                        _.Content,
                        _.State};
            }
            
            /// <summary>
            /// ��ȡ������
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._ID,
                        this._SID,
                        this._SessionID,
                        this._SenderID,
                        this._SenderName,
                        this._ReceiverID,
                        this._ReceiverName,
                        this._SenderIP,
                        this._SendTime,
                        this._Type,
                        this._Content,
                        this._State};
            }
            
            /// <summary>
            /// ����ǰʵ�帳ֵ
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.ID))) {
                    this._ID = reader.GetGuid(_.ID);
                }
                if ((false == reader.IsDBNull(_.SID))) {
                    this._SID = reader.GetGuid(_.SID);
                }
                if ((false == reader.IsDBNull(_.SessionID))) {
                    this._SessionID = reader.GetString(_.SessionID);
                }
                if ((false == reader.IsDBNull(_.SenderID))) {
                    this._SenderID = reader.GetString(_.SenderID);
                }
                if ((false == reader.IsDBNull(_.SenderName))) {
                    this._SenderName = reader.GetString(_.SenderName);
                }
                if ((false == reader.IsDBNull(_.ReceiverID))) {
                    this._ReceiverID = reader.GetString(_.ReceiverID);
                }
                if ((false == reader.IsDBNull(_.ReceiverName))) {
                    this._ReceiverName = reader.GetString(_.ReceiverName);
                }
                if ((false == reader.IsDBNull(_.SenderIP))) {
                    this._SenderIP = reader.GetString(_.SenderIP);
                }
                if ((false == reader.IsDBNull(_.SendTime))) {
                    this._SendTime = reader.GetDateTime(_.SendTime);
                }
                if ((false == reader.IsDBNull(_.Type))) {
                    this._Type = ((LiveChat.Entity.MessageType)(reader.GetInt32(_.Type)));
                }
                if ((false == reader.IsDBNull(_.Content))) {
                    this._Content = reader.GetString(_.Content);
                }
                if ((false == reader.IsDBNull(_.State))) {
                    this._State = ((LiveChat.Entity.MessageState)(reader.GetInt32(_.State)));
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_P2SMessage).IsAssignableFrom(obj.GetType()))) {
                    return false;
                }
                if ((((object)(this)) == ((object)(obj)))) {
                    return true;
                }
                return false;
            }
            
            public class _ {
                
                /// <summary>
                /// ��ʾѡ�������У���*��ͬ
                /// </summary>
                public static AllField All = new AllField<t_P2SMessage>();
                
                /// <summary>
                /// �ֶ�����ID - �������ͣ�Guid
                /// </summary>
                public static Field ID = new Field<t_P2SMessage>("ID");
                
                /// <summary>
                /// �ֶ�����SID - �������ͣ�Guid
                /// </summary>
                public static Field SID = new Field<t_P2SMessage>("SID");
                
                /// <summary>
                /// �ֶ�����SessionID - �������ͣ�String
                /// </summary>
                public static Field SessionID = new Field<t_P2SMessage>("SessionID");
                
                /// <summary>
                /// �ֶ�����SenderID - �������ͣ�String
                /// </summary>
                public static Field SenderID = new Field<t_P2SMessage>("SenderID");
                
                /// <summary>
                /// �ֶ�����SenderName - �������ͣ�String
                /// </summary>
                public static Field SenderName = new Field<t_P2SMessage>("SenderName");
                
                /// <summary>
                /// �ֶ�����ReceiverID - �������ͣ�String
                /// </summary>
                public static Field ReceiverID = new Field<t_P2SMessage>("ReceiverID");
                
                /// <summary>
                /// �ֶ�����ReceiverName - �������ͣ�String
                /// </summary>
                public static Field ReceiverName = new Field<t_P2SMessage>("ReceiverName");
                
                /// <summary>
                /// �ֶ�����SenderIP - �������ͣ�String
                /// </summary>
                public static Field SenderIP = new Field<t_P2SMessage>("SenderIP");
                
                /// <summary>
                /// �ֶ�����SendTime - �������ͣ�DateTime
                /// </summary>
                public static Field SendTime = new Field<t_P2SMessage>("SendTime");
                
                /// <summary>
                /// �ֶ�����Type - �������ͣ�MessageType
                /// </summary>
                public static Field Type = new Field<t_P2SMessage>("Type");
                
                /// <summary>
                /// �ֶ�����Content - �������ͣ�String
                /// </summary>
                public static Field Content = new Field<t_P2SMessage>("Content");
                
                /// <summary>
                /// �ֶ�����State - �������ͣ�MessageState
                /// </summary>
                public static Field State = new Field<t_P2SMessage>("State");
            }
        }
    }
    