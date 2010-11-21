namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_Leave 主键列：ID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_Leave : Entity {
            
            protected Int32 _ID;
            
            protected String _CompanyID;
            
            protected String _Name;
            
            protected String _Telephone;
            
            protected String _Email;
            
            protected String _Title;
            
            protected String _Body;
            
            protected String _PostIP;
            
            protected DateTime _AddTime;
            
            public Int32 ID {
                get {
                    return this._ID;
                }
                set {
                    this.OnPropertyValueChange(_.ID, _ID, value);
                    this._ID = value;
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
            
            public String Name {
                get {
                    return this._Name;
                }
                set {
                    this.OnPropertyValueChange(_.Name, _Name, value);
                    this._Name = value;
                }
            }
            
            public String Telephone {
                get {
                    return this._Telephone;
                }
                set {
                    this.OnPropertyValueChange(_.Telephone, _Telephone, value);
                    this._Telephone = value;
                }
            }
            
            public String Email {
                get {
                    return this._Email;
                }
                set {
                    this.OnPropertyValueChange(_.Email, _Email, value);
                    this._Email = value;
                }
            }
            
            public String Title {
                get {
                    return this._Title;
                }
                set {
                    this.OnPropertyValueChange(_.Title, _Title, value);
                    this._Title = value;
                }
            }
            
            public String Body {
                get {
                    return this._Body;
                }
                set {
                    this.OnPropertyValueChange(_.Body, _Body, value);
                    this._Body = value;
                }
            }
            
            public String PostIP {
                get {
                    return this._PostIP;
                }
                set {
                    this.OnPropertyValueChange(_.PostIP, _PostIP, value);
                    this._PostIP = value;
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
                return new Table<t_Leave>("t_Leave");
            }
            
            /// <summary>
            /// 获取实体中的标识列
            /// </summary>
            protected override Field GetIdentityField() {
                return _.ID;
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
                        _.CompanyID,
                        _.Name,
                        _.Telephone,
                        _.Email,
                        _.Title,
                        _.Body,
                        _.PostIP,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._ID,
                        this._CompanyID,
                        this._Name,
                        this._Telephone,
                        this._Email,
                        this._Title,
                        this._Body,
                        this._PostIP,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.ID))) {
                    this._ID = reader.GetInt32(_.ID);
                }
                if ((false == reader.IsDBNull(_.CompanyID))) {
                    this._CompanyID = reader.GetString(_.CompanyID);
                }
                if ((false == reader.IsDBNull(_.Name))) {
                    this._Name = reader.GetString(_.Name);
                }
                if ((false == reader.IsDBNull(_.Telephone))) {
                    this._Telephone = reader.GetString(_.Telephone);
                }
                if ((false == reader.IsDBNull(_.Email))) {
                    this._Email = reader.GetString(_.Email);
                }
                if ((false == reader.IsDBNull(_.Title))) {
                    this._Title = reader.GetString(_.Title);
                }
                if ((false == reader.IsDBNull(_.Body))) {
                    this._Body = reader.GetString(_.Body);
                }
                if ((false == reader.IsDBNull(_.PostIP))) {
                    this._PostIP = reader.GetString(_.PostIP);
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
                if ((false == typeof(t_Leave).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_Leave>();
                
                /// <summary>
                /// 字段名：ID - 数据类型：Int32
                /// </summary>
                public static Field ID = new Field<t_Leave>("ID");
                
                /// <summary>
                /// 字段名：CompanyID - 数据类型：String
                /// </summary>
                public static Field CompanyID = new Field<t_Leave>("CompanyID");
                
                /// <summary>
                /// 字段名：Name - 数据类型：String
                /// </summary>
                public static Field Name = new Field<t_Leave>("Name");
                
                /// <summary>
                /// 字段名：Telephone - 数据类型：String
                /// </summary>
                public static Field Telephone = new Field<t_Leave>("Telephone");
                
                /// <summary>
                /// 字段名：Email - 数据类型：String
                /// </summary>
                public static Field Email = new Field<t_Leave>("Email");
                
                /// <summary>
                /// 字段名：Title - 数据类型：String
                /// </summary>
                public static Field Title = new Field<t_Leave>("Title");
                
                /// <summary>
                /// 字段名：Body - 数据类型：String
                /// </summary>
                public static Field Body = new Field<t_Leave>("Body");
                
                /// <summary>
                /// 字段名：PostIP - 数据类型：String
                /// </summary>
                public static Field PostIP = new Field<t_Leave>("PostIP");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_Leave>("AddTime");
            }
        }
    }
    