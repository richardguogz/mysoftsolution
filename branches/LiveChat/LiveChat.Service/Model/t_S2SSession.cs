namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_S2SSession 主键列：SID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_S2SSession : Entity {
            
            protected Guid _SID;
            
            protected String _SessionID;
            
            protected String _CreateID;
            
            protected String _SeatID;
            
            protected String _FriendID;
            
            protected String _FromIP;
            
            protected String _FromAddress;
            
            protected DateTime? _LastReceiveTime;
            
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
            
            public String SeatID {
                get {
                    return this._SeatID;
                }
                set {
                    this.OnPropertyValueChange(_.SeatID, _SeatID, value);
                    this._SeatID = value;
                }
            }
            
            public String FriendID {
                get {
                    return this._FriendID;
                }
                set {
                    this.OnPropertyValueChange(_.FriendID, _FriendID, value);
                    this._FriendID = value;
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
            
            /// <summary>
            /// 获取实体对应的表名
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_S2SSession>("t_S2SSession");
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.SID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.SID,
                        _.SessionID,
                        _.CreateID,
                        _.SeatID,
                        _.FriendID,
                        _.FromIP,
                        _.FromAddress,
                        _.LastReceiveTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._SID,
                        this._SessionID,
                        this._CreateID,
                        this._SeatID,
                        this._FriendID,
                        this._FromIP,
                        this._FromAddress,
                        this._LastReceiveTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
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
                if ((false == reader.IsDBNull(_.SeatID))) {
                    this._SeatID = reader.GetString(_.SeatID);
                }
                if ((false == reader.IsDBNull(_.FriendID))) {
                    this._FriendID = reader.GetString(_.FriendID);
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
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_S2SSession).IsAssignableFrom(obj.GetType()))) {
                    return false;
                }
                if ((((object)(this)) == ((object)(obj)))) {
                    return true;
                }
                return false;
            }
            
            public class _ {
                
                /// <summary>
                /// 表示选择所有列，与*等同
                /// </summary>
                public static AllField All = new AllField<t_S2SSession>();
                
                /// <summary>
                /// 字段名：SID - 数据类型：Guid
                /// </summary>
                public static Field SID = new Field<t_S2SSession>("SID");
                
                /// <summary>
                /// 字段名：SessionID - 数据类型：String
                /// </summary>
                public static Field SessionID = new Field<t_S2SSession>("SessionID");
                
                /// <summary>
                /// 字段名：CreateID - 数据类型：String
                /// </summary>
                public static Field CreateID = new Field<t_S2SSession>("CreateID");
                
                /// <summary>
                /// 字段名：SeatID - 数据类型：String
                /// </summary>
                public static Field SeatID = new Field<t_S2SSession>("SeatID");
                
                /// <summary>
                /// 字段名：FriendID - 数据类型：String
                /// </summary>
                public static Field FriendID = new Field<t_S2SSession>("FriendID");
                
                /// <summary>
                /// 字段名：FromIP - 数据类型：String
                /// </summary>
                public static Field FromIP = new Field<t_S2SSession>("FromIP");
                
                /// <summary>
                /// 字段名：FromAddress - 数据类型：String
                /// </summary>
                public static Field FromAddress = new Field<t_S2SSession>("FromAddress");
                
                /// <summary>
                /// 字段名：LastReceiveTime - 数据类型：DateTime(可空)
                /// </summary>
                public static Field LastReceiveTime = new Field<t_S2SSession>("LastReceiveTime");
            }
        }
    }
    