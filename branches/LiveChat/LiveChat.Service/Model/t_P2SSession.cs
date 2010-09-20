namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_P2SSession �����У�SID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_P2SSession : Entity {
            
            protected Guid _SID;
            
            protected String _SessionID;
            
            protected String _CreateID;
            
            protected String _UserID;
            
            protected String _SeatID;
            
            protected String _RequestCode;
            
            protected String _FromIP;
            
            protected String _FromAddress;
            
            protected DateTime? _LastReceiveTime;
            
            protected String _RequestMessage;
            
            protected DateTime _StartTime;
            
            protected DateTime? _EndTime;
            
            protected LiveChat.Entity.SessionState _State;
            
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
            
            public String CreateID {
                get {
                    return this._CreateID;
                }
                set {
                    this.OnPropertyValueChange(_.CreateID, _CreateID, value);
                    this._CreateID = value;
                }
            }
            
            public String UserID {
                get {
                    return this._UserID;
                }
                set {
                    this.OnPropertyValueChange(_.UserID, _UserID, value);
                    this._UserID = value;
                }
            }
            
            public String SeatID {
                get {
                    return this._SeatID;
                }
                set {
                    this.OnPropertyValueChange(_.SeatID, _SeatID, value);
                    this._SeatID = value;
                }
            }
            
            public String RequestCode {
                get {
                    return this._RequestCode;
                }
                set {
                    this.OnPropertyValueChange(_.RequestCode, _RequestCode, value);
                    this._RequestCode = value;
                }
            }
            
            public String FromIP {
                get {
                    return this._FromIP;
                }
                set {
                    this.OnPropertyValueChange(_.FromIP, _FromIP, value);
                    this._FromIP = value;
                }
            }
            
            public String FromAddress {
                get {
                    return this._FromAddress;
                }
                set {
                    this.OnPropertyValueChange(_.FromAddress, _FromAddress, value);
                    this._FromAddress = value;
                }
            }
            
            public DateTime? LastReceiveTime {
                get {
                    return this._LastReceiveTime;
                }
                set {
                    this.OnPropertyValueChange(_.LastReceiveTime, _LastReceiveTime, value);
                    this._LastReceiveTime = value;
                }
            }
            
            public String RequestMessage {
                get {
                    return this._RequestMessage;
                }
                set {
                    this.OnPropertyValueChange(_.RequestMessage, _RequestMessage, value);
                    this._RequestMessage = value;
                }
            }
            
            public DateTime StartTime {
                get {
                    return this._StartTime;
                }
                set {
                    this.OnPropertyValueChange(_.StartTime, _StartTime, value);
                    this._StartTime = value;
                }
            }
            
            public DateTime? EndTime {
                get {
                    return this._EndTime;
                }
                set {
                    this.OnPropertyValueChange(_.EndTime, _EndTime, value);
                    this._EndTime = value;
                }
            }
            
            public LiveChat.Entity.SessionState State {
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
                return new Table<t_P2SSession>("t_P2SSession");
            }
            
            /// <summary>
            /// ��ȡʵ���е�������
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.SID};
            }
            
            /// <summary>
            /// ��ȡ����Ϣ
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.SID,
                        _.SessionID,
                        _.CreateID,
                        _.UserID,
                        _.SeatID,
                        _.RequestCode,
                        _.FromIP,
                        _.FromAddress,
                        _.LastReceiveTime,
                        _.RequestMessage,
                        _.StartTime,
                        _.EndTime,
                        _.State};
            }
            
            /// <summary>
            /// ��ȡ������
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._SID,
                        this._SessionID,
                        this._CreateID,
                        this._UserID,
                        this._SeatID,
                        this._RequestCode,
                        this._FromIP,
                        this._FromAddress,
                        this._LastReceiveTime,
                        this._RequestMessage,
                        this._StartTime,
                        this._EndTime,
                        this._State};
            }
            
            /// <summary>
            /// ����ǰʵ�帳ֵ
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.SID))) {
                    this._SID = reader.GetGuid(_.SID);
                }
                if ((false == reader.IsDBNull(_.SessionID))) {
                    this._SessionID = reader.GetString(_.SessionID);
                }
                if ((false == reader.IsDBNull(_.CreateID))) {
                    this._CreateID = reader.GetString(_.CreateID);
                }
                if ((false == reader.IsDBNull(_.UserID))) {
                    this._UserID = reader.GetString(_.UserID);
                }
                if ((false == reader.IsDBNull(_.SeatID))) {
                    this._SeatID = reader.GetString(_.SeatID);
                }
                if ((false == reader.IsDBNull(_.RequestCode))) {
                    this._RequestCode = reader.GetString(_.RequestCode);
                }
                if ((false == reader.IsDBNull(_.FromIP))) {
                    this._FromIP = reader.GetString(_.FromIP);
                }
                if ((false == reader.IsDBNull(_.FromAddress))) {
                    this._FromAddress = reader.GetString(_.FromAddress);
                }
                if ((false == reader.IsDBNull(_.LastReceiveTime))) {
                    this._LastReceiveTime = reader.GetDateTime(_.LastReceiveTime);
                }
                if ((false == reader.IsDBNull(_.RequestMessage))) {
                    this._RequestMessage = reader.GetString(_.RequestMessage);
                }
                if ((false == reader.IsDBNull(_.StartTime))) {
                    this._StartTime = reader.GetDateTime(_.StartTime);
                }
                if ((false == reader.IsDBNull(_.EndTime))) {
                    this._EndTime = reader.GetDateTime(_.EndTime);
                }
                if ((false == reader.IsDBNull(_.State))) {
                    this._State = ((LiveChat.Entity.SessionState)(reader.GetInt32(_.State)));
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_P2SSession).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_P2SSession>();
                
                /// <summary>
                /// �ֶ�����SID - �������ͣ�Guid
                /// </summary>
                public static Field SID = new Field<t_P2SSession>("SID");
                
                /// <summary>
                /// �ֶ�����SessionID - �������ͣ�String
                /// </summary>
                public static Field SessionID = new Field<t_P2SSession>("SessionID");
                
                /// <summary>
                /// �ֶ�����CreateID - �������ͣ�String
                /// </summary>
                public static Field CreateID = new Field<t_P2SSession>("CreateID");
                
                /// <summary>
                /// �ֶ�����UserID - �������ͣ�String
                /// </summary>
                public static Field UserID = new Field<t_P2SSession>("UserID");
                
                /// <summary>
                /// �ֶ�����SeatID - �������ͣ�String
                /// </summary>
                public static Field SeatID = new Field<t_P2SSession>("SeatID");
                
                /// <summary>
                /// �ֶ�����RequestCode - �������ͣ�String
                /// </summary>
                public static Field RequestCode = new Field<t_P2SSession>("RequestCode");
                
                /// <summary>
                /// �ֶ�����FromIP - �������ͣ�String
                /// </summary>
                public static Field FromIP = new Field<t_P2SSession>("FromIP");
                
                /// <summary>
                /// �ֶ�����FromAddress - �������ͣ�String
                /// </summary>
                public static Field FromAddress = new Field<t_P2SSession>("FromAddress");
                
                /// <summary>
                /// �ֶ�����LastReceiveTime - �������ͣ�DateTime(�ɿ�)
                /// </summary>
                public static Field LastReceiveTime = new Field<t_P2SSession>("LastReceiveTime");
                
                /// <summary>
                /// �ֶ�����RequestMessage - �������ͣ�String
                /// </summary>
                public static Field RequestMessage = new Field<t_P2SSession>("RequestMessage");
                
                /// <summary>
                /// �ֶ�����StartTime - �������ͣ�DateTime
                /// </summary>
                public static Field StartTime = new Field<t_P2SSession>("StartTime");
                
                /// <summary>
                /// �ֶ�����EndTime - �������ͣ�DateTime(�ɿ�)
                /// </summary>
                public static Field EndTime = new Field<t_P2SSession>("EndTime");
                
                /// <summary>
                /// �ֶ�����State - �������ͣ�SessionState
                /// </summary>
                public static Field State = new Field<t_P2SSession>("State");
            }
        }
    }
    