namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_SGroup 主键列：GroupID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_SGroup : Entity {
            
            protected Guid _GroupID;
            
            protected String _CreateID;
            
            protected String _ManagerID;
            
            protected String _GroupName;
            
            protected Int32 _MaxPerson;
            
            protected DateTime _AddTime;
            
            protected String _Description;
            
            protected String _Notification;
            
            public Guid GroupID {
                get {
                    return this._GroupID;
                }
                set {
                    this.OnPropertyValueChange(_.GroupID, _GroupID, value);
                    this._GroupID = value;
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
            
            public String ManagerID {
                get {
                    return this._ManagerID;
                }
                set {
                    this.OnPropertyValueChange(_.ManagerID, _ManagerID, value);
                    this._ManagerID = value;
                }
            }
            
            public String GroupName {
                get {
                    return this._GroupName;
                }
                set {
                    this.OnPropertyValueChange(_.GroupName, _GroupName, value);
                    this._GroupName = value;
                }
            }
            
            public Int32 MaxPerson {
                get {
                    return this._MaxPerson;
                }
                set {
                    this.OnPropertyValueChange(_.MaxPerson, _MaxPerson, value);
                    this._MaxPerson = value;
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
            
            public String Description {
                get {
                    return this._Description;
                }
                set {
                    this.OnPropertyValueChange(_.Description, _Description, value);
                    this._Description = value;
                }
            }
            
            public String Notification {
                get {
                    return this._Notification;
                }
                set {
                    this.OnPropertyValueChange(_.Notification, _Notification, value);
                    this._Notification = value;
                }
            }
            
            /// <summary>
            /// 获取实体对应的表名
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_SGroup>("t_SGroup");
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.GroupID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.GroupID,
                        _.CreateID,
                        _.ManagerID,
                        _.GroupName,
                        _.MaxPerson,
                        _.AddTime,
                        _.Description,
                        _.Notification};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._GroupID,
                        this._CreateID,
                        this._ManagerID,
                        this._GroupName,
                        this._MaxPerson,
                        this._AddTime,
                        this._Description,
                        this._Notification};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.GroupID))) {
                    this._GroupID = reader.GetGuid(_.GroupID);
                }
                if ((false == reader.IsDBNull(_.CreateID))) {
                    this._CreateID = reader.GetString(_.CreateID);
                }
                if ((false == reader.IsDBNull(_.ManagerID))) {
                    this._ManagerID = reader.GetString(_.ManagerID);
                }
                if ((false == reader.IsDBNull(_.GroupName))) {
                    this._GroupName = reader.GetString(_.GroupName);
                }
                if ((false == reader.IsDBNull(_.MaxPerson))) {
                    this._MaxPerson = reader.GetInt32(_.MaxPerson);
                }
                if ((false == reader.IsDBNull(_.AddTime))) {
                    this._AddTime = reader.GetDateTime(_.AddTime);
                }
                if ((false == reader.IsDBNull(_.Description))) {
                    this._Description = reader.GetString(_.Description);
                }
                if ((false == reader.IsDBNull(_.Notification))) {
                    this._Notification = reader.GetString(_.Notification);
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_SGroup).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_SGroup>();
                
                /// <summary>
                /// 字段名：GroupID - 数据类型：Guid
                /// </summary>
                public static Field GroupID = new Field<t_SGroup>("GroupID");
                
                /// <summary>
                /// 字段名：CreateID - 数据类型：String
                /// </summary>
                public static Field CreateID = new Field<t_SGroup>("CreateID");
                
                /// <summary>
                /// 字段名：ManagerID - 数据类型：String
                /// </summary>
                public static Field ManagerID = new Field<t_SGroup>("ManagerID");
                
                /// <summary>
                /// 字段名：GroupName - 数据类型：String
                /// </summary>
                public static Field GroupName = new Field<t_SGroup>("GroupName");
                
                /// <summary>
                /// 字段名：MaxPerson - 数据类型：Int32
                /// </summary>
                public static Field MaxPerson = new Field<t_SGroup>("MaxPerson");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_SGroup>("AddTime");
                
                /// <summary>
                /// 字段名：Description - 数据类型：String
                /// </summary>
                public static Field Description = new Field<t_SGroup>("Description");
                
                /// <summary>
                /// 字段名：Notification - 数据类型：String
                /// </summary>
                public static Field Notification = new Field<t_SGroup>("Notification");
            }
        }
    }
    