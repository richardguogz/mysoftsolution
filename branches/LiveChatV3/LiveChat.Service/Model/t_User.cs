namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_User 主键列：UserID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_User : Entity {
            
            protected String _UserID;
            
            protected String _UserName;
            
            protected LiveChat.Entity.UserType _UserType;
            
            protected DateTime? _LastChatTime;
            
            protected Int32? _ChatCount;
            
            public String UserID {
                get {
                    return this._UserID;
                }
                set {
                    this.OnPropertyValueChange(_.UserID, _UserID, value);
                    this._UserID = value;
                }
            }
            
            public String UserName {
                get {
                    return this._UserName;
                }
                set {
                    this.OnPropertyValueChange(_.UserName, _UserName, value);
                    this._UserName = value;
                }
            }
            
            public LiveChat.Entity.UserType UserType {
                get {
                    return this._UserType;
                }
                set {
                    this.OnPropertyValueChange(_.UserType, _UserType, value);
                    this._UserType = value;
                }
            }
            
            public DateTime? LastChatTime {
                get {
                    return this._LastChatTime;
                }
                set {
                    this.OnPropertyValueChange(_.LastChatTime, _LastChatTime, value);
                    this._LastChatTime = value;
                }
            }
            
            public Int32? ChatCount {
                get {
                    return this._ChatCount;
                }
                set {
                    this.OnPropertyValueChange(_.ChatCount, _ChatCount, value);
                    this._ChatCount = value;
                }
            }
            
            /// <summary>
            /// 获取实体对应的表名
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_User>("t_User");
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.UserID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.UserID,
                        _.UserName,
                        _.UserType,
                        _.LastChatTime,
                        _.ChatCount};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._UserID,
                        this._UserName,
                        this._UserType,
                        this._LastChatTime,
                        this._ChatCount};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.UserID))) {
                    this._UserID = reader.GetString(_.UserID);
                }
                if ((false == reader.IsDBNull(_.UserName))) {
                    this._UserName = reader.GetString(_.UserName);
                }
                if ((false == reader.IsDBNull(_.UserType))) {
                    this._UserType = ((LiveChat.Entity.UserType)(reader.GetInt32(_.UserType)));
                }
                if ((false == reader.IsDBNull(_.LastChatTime))) {
                    this._LastChatTime = reader.GetDateTime(_.LastChatTime);
                }
                if ((false == reader.IsDBNull(_.ChatCount))) {
                    this._ChatCount = reader.GetInt32(_.ChatCount);
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_User).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_User>();
                
                /// <summary>
                /// 字段名：UserID - 数据类型：String
                /// </summary>
                public static Field UserID = new Field<t_User>("UserID");
                
                /// <summary>
                /// 字段名：UserName - 数据类型：String
                /// </summary>
                public static Field UserName = new Field<t_User>("UserName");
                
                /// <summary>
                /// 字段名：UserType - 数据类型：UserType
                /// </summary>
                public static Field UserType = new Field<t_User>("UserType");
                
                /// <summary>
                /// 字段名：LastChatTime - 数据类型：DateTime(可空)
                /// </summary>
                public static Field LastChatTime = new Field<t_User>("LastChatTime");
                
                /// <summary>
                /// 字段名：ChatCount - 数据类型：Int32(可空)
                /// </summary>
                public static Field ChatCount = new Field<t_User>("ChatCount");
            }
        }
    }
    