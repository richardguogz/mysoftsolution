namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_UGroupRequest 主键列：RequestID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_UGroupRequest : Entity {
            
            protected Int32 _RequestID;
            
            protected String _UserID;
            
            protected Guid _GroupID;
            
            protected String _Request;
            
            protected String _Refuse;
            
            protected Int32 _ConfirmState;
            
            protected DateTime _AddTime;
            
            public Int32 RequestID {
                get {
                    return this._RequestID;
                }
                set {
                    this.OnPropertyValueChange(_.RequestID, _RequestID, value);
                    this._RequestID = value;
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
            
            public Guid GroupID {
                get {
                    return this._GroupID;
                }
                set {
                    this.OnPropertyValueChange(_.GroupID, _GroupID, value);
                    this._GroupID = value;
                }
            }
            
            public String Request {
                get {
                    return this._Request;
                }
                set {
                    this.OnPropertyValueChange(_.Request, _Request, value);
                    this._Request = value;
                }
            }
            
            public String Refuse {
                get {
                    return this._Refuse;
                }
                set {
                    this.OnPropertyValueChange(_.Refuse, _Refuse, value);
                    this._Refuse = value;
                }
            }
            
            public Int32 ConfirmState {
                get {
                    return this._ConfirmState;
                }
                set {
                    this.OnPropertyValueChange(_.ConfirmState, _ConfirmState, value);
                    this._ConfirmState = value;
                }
            }
            
            public DateTime AddTime {
                get {
                    return this._AddTime;
                }
                set {
                    this.OnPropertyValueChange(_.AddTime, _AddTime, value);
                    this._AddTime = value;
                }
            }
            
            /// <summary>
            /// 获取实体对应的表名
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_UGroupRequest>("t_UGroupRequest");
            }
            
            /// <summary>
            /// 获取实体中的标识列
            /// </summary>
            protected override Field GetIdentityField() {
                return _.RequestID;
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.RequestID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.RequestID,
                        _.UserID,
                        _.GroupID,
                        _.Request,
                        _.Refuse,
                        _.ConfirmState,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._RequestID,
                        this._UserID,
                        this._GroupID,
                        this._Request,
                        this._Refuse,
                        this._ConfirmState,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.RequestID))) {
                    this._RequestID = reader.GetInt32(_.RequestID);
                }
                if ((false == reader.IsDBNull(_.UserID))) {
                    this._UserID = reader.GetString(_.UserID);
                }
                if ((false == reader.IsDBNull(_.GroupID))) {
                    this._GroupID = reader.GetGuid(_.GroupID);
                }
                if ((false == reader.IsDBNull(_.Request))) {
                    this._Request = reader.GetString(_.Request);
                }
                if ((false == reader.IsDBNull(_.Refuse))) {
                    this._Refuse = reader.GetString(_.Refuse);
                }
                if ((false == reader.IsDBNull(_.ConfirmState))) {
                    this._ConfirmState = reader.GetInt32(_.ConfirmState);
                }
                if ((false == reader.IsDBNull(_.AddTime))) {
                    this._AddTime = reader.GetDateTime(_.AddTime);
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_UGroupRequest).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_UGroupRequest>();
                
                /// <summary>
                /// 字段名：RequestID - 数据类型：Int32
                /// </summary>
                public static Field RequestID = new Field<t_UGroupRequest>("RequestID");
                
                /// <summary>
                /// 字段名：UserID - 数据类型：String
                /// </summary>
                public static Field UserID = new Field<t_UGroupRequest>("UserID");
                
                /// <summary>
                /// 字段名：GroupID - 数据类型：Guid
                /// </summary>
                public static Field GroupID = new Field<t_UGroupRequest>("GroupID");
                
                /// <summary>
                /// 字段名：Request - 数据类型：String
                /// </summary>
                public static Field Request = new Field<t_UGroupRequest>("Request");
                
                /// <summary>
                /// 字段名：Refuse - 数据类型：String
                /// </summary>
                public static Field Refuse = new Field<t_UGroupRequest>("Refuse");
                
                /// <summary>
                /// 字段名：ConfirmState - 数据类型：Int32
                /// </summary>
                public static Field ConfirmState = new Field<t_UGroupRequest>("ConfirmState");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_UGroupRequest>("AddTime");
            }
        }
    }
    