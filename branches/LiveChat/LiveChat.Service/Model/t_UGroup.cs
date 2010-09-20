namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_UGroup 主键列：GroupID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_UGroup : Entity {
            
            protected Guid _GroupID;
            
            protected String _CompanyID;
            
            protected String _GroupName;
            
            protected Int32? _MaxPerson;
            
            protected DateTime? _AddTime;
            
            public Guid GroupID {
                get {
                    return this._GroupID;
                }
                set {
                    this.OnPropertyValueChange(_.GroupID, _GroupID, value);
                    this._GroupID = value;
                }
            }
            
            public String CompanyID {
                get {
                    return this._CompanyID;
                }
                set {
                    this.OnPropertyValueChange(_.CompanyID, _CompanyID, value);
                    this._CompanyID = value;
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
            
            public Int32? MaxPerson {
                get {
                    return this._MaxPerson;
                }
                set {
                    this.OnPropertyValueChange(_.MaxPerson, _MaxPerson, value);
                    this._MaxPerson = value;
                }
            }
            
            public DateTime? AddTime {
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
                return new Table<t_UGroup>("t_UGroup");
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
                        _.CompanyID,
                        _.GroupName,
                        _.MaxPerson,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._GroupID,
                        this._CompanyID,
                        this._GroupName,
                        this._MaxPerson,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.GroupID))) {
                    this._GroupID = reader.GetGuid(_.GroupID);
                }
                if ((false == reader.IsDBNull(_.CompanyID))) {
                    this._CompanyID = reader.GetString(_.CompanyID);
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
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_UGroup).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_UGroup>();
                
                /// <summary>
                /// 字段名：GroupID - 数据类型：Guid
                /// </summary>
                public static Field GroupID = new Field<t_UGroup>("GroupID");
                
                /// <summary>
                /// 字段名：CompanyID - 数据类型：String
                /// </summary>
                public static Field CompanyID = new Field<t_UGroup>("CompanyID");
                
                /// <summary>
                /// 字段名：GroupName - 数据类型：String
                /// </summary>
                public static Field GroupName = new Field<t_UGroup>("GroupName");
                
                /// <summary>
                /// 字段名：MaxPerson - 数据类型：Int32(可空)
                /// </summary>
                public static Field MaxPerson = new Field<t_UGroup>("MaxPerson");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime(可空)
                /// </summary>
                public static Field AddTime = new Field<t_UGroup>("AddTime");
            }
        }
    }
    