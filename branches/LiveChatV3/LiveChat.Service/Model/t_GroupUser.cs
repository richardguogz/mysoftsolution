namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_GroupUser 主键列：GroupID,UserID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_GroupUser : Entity {
            
            protected Guid _GroupID;
            
            protected String _UserID;
            
            protected String _MemoName;
            
            protected DateTime _AddTime;
            
            public Guid GroupID {
                get {
                    return this._GroupID;
                }
                set {
                    this.OnPropertyValueChange(_.GroupID, _GroupID, value);
                    this._GroupID = value;
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
            
            public String MemoName {
                get {
                    return this._MemoName;
                }
                set {
                    this.OnPropertyValueChange(_.MemoName, _MemoName, value);
                    this._MemoName = value;
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
                return new Table<t_GroupUser>("t_GroupUser");
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.GroupID,
                        _.UserID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.GroupID,
                        _.UserID,
                        _.MemoName,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._GroupID,
                        this._UserID,
                        this._MemoName,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.GroupID))) {
                    this._GroupID = reader.GetGuid(_.GroupID);
                }
                if ((false == reader.IsDBNull(_.UserID))) {
                    this._UserID = reader.GetString(_.UserID);
                }
                if ((false == reader.IsDBNull(_.MemoName))) {
                    this._MemoName = reader.GetString(_.MemoName);
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
                if ((false == typeof(t_GroupUser).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_GroupUser>();
                
                /// <summary>
                /// 字段名：GroupID - 数据类型：Guid
                /// </summary>
                public static Field GroupID = new Field<t_GroupUser>("GroupID");
                
                /// <summary>
                /// 字段名：UserID - 数据类型：String
                /// </summary>
                public static Field UserID = new Field<t_GroupUser>("UserID");
                
                /// <summary>
                /// 字段名：MemoName - 数据类型：String
                /// </summary>
                public static Field MemoName = new Field<t_GroupUser>("MemoName");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_GroupUser>("AddTime");
            }
        }
    }
    