namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_Area 主键列：ID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_Area : Entity {
            
            protected Int32 _ID;
            
            protected String _AreaID;
            
            protected String _AreaName;
            
            protected String _ParentID;
            
            protected DateTime? _AddTime;
            
            public Int32 ID {
                get {
                    return this._ID;
                }
                set {
                    this.OnPropertyValueChange(_.ID, _ID, value);
                    this._ID = value;
                }
            }
            
            public String AreaID {
                get {
                    return this._AreaID;
                }
                set {
                    this.OnPropertyValueChange(_.AreaID, _AreaID, value);
                    this._AreaID = value;
                }
            }
            
            public String AreaName {
                get {
                    return this._AreaName;
                }
                set {
                    this.OnPropertyValueChange(_.AreaName, _AreaName, value);
                    this._AreaName = value;
                }
            }
            
            public String ParentID {
                get {
                    return this._ParentID;
                }
                set {
                    this.OnPropertyValueChange(_.ParentID, _ParentID, value);
                    this._ParentID = value;
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
                return new Table<t_Area>("t_Area");
            }
            
            /// <summary>
            /// 获取实体中的标识列
            /// </summary>
            protected override Field GetIdentityField() {
                return _.AreaID;
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.ID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.ID,
                        _.AreaID,
                        _.AreaName,
                        _.ParentID,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._ID,
                        this._AreaID,
                        this._AreaName,
                        this._ParentID,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.ID))) {
                    this._ID = reader.GetInt32(_.ID);
                }
                if ((false == reader.IsDBNull(_.AreaID))) {
                    this._AreaID = reader.GetString(_.AreaID);
                }
                if ((false == reader.IsDBNull(_.AreaName))) {
                    this._AreaName = reader.GetString(_.AreaName);
                }
                if ((false == reader.IsDBNull(_.ParentID))) {
                    this._ParentID = reader.GetString(_.ParentID);
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
                if ((false == typeof(t_Area).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_Area>();
                
                /// <summary>
                /// 字段名：ID - 数据类型：Int32
                /// </summary>
                public static Field ID = new Field<t_Area>("ID");
                
                /// <summary>
                /// 字段名：AreaID - 数据类型：String
                /// </summary>
                public static Field AreaID = new Field<t_Area>("AreaID");
                
                /// <summary>
                /// 字段名：AreaName - 数据类型：String
                /// </summary>
                public static Field AreaName = new Field<t_Area>("AreaName");
                
                /// <summary>
                /// 字段名：ParentID - 数据类型：String
                /// </summary>
                public static Field ParentID = new Field<t_Area>("ParentID");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime(可空)
                /// </summary>
                public static Field AddTime = new Field<t_Area>("AddTime");
            }
        }
    }
    