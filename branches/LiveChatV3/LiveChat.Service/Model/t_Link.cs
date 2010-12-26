namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_Link 主键列：LinkID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_Link : Entity {
            
            protected Int32 _LinkID;
            
            protected String _CompanyID;
            
            protected String _Title;
            
            protected String _Url;
            
            protected DateTime _AddTime;
            
            public Int32 LinkID {
                get {
                    return this._LinkID;
                }
                set {
                    this.OnPropertyValueChange(_.LinkID, _LinkID, value);
                    this._LinkID = value;
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
            
            public String Title {
                get {
                    return this._Title;
                }
                set {
                    this.OnPropertyValueChange(_.Title, _Title, value);
                    this._Title = value;
                }
            }
            
            public String Url {
                get {
                    return this._Url;
                }
                set {
                    this.OnPropertyValueChange(_.Url, _Url, value);
                    this._Url = value;
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
                return new Table<t_Link>("t_Link");
            }
            
            /// <summary>
            /// 获取实体中的标识列
            /// </summary>
            protected override Field GetIdentityField() {
                return _.LinkID;
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.LinkID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.LinkID,
                        _.CompanyID,
                        _.Title,
                        _.Url,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._LinkID,
                        this._CompanyID,
                        this._Title,
                        this._Url,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.LinkID))) {
                    this._LinkID = reader.GetInt32(_.LinkID);
                }
                if ((false == reader.IsDBNull(_.CompanyID))) {
                    this._CompanyID = reader.GetString(_.CompanyID);
                }
                if ((false == reader.IsDBNull(_.Title))) {
                    this._Title = reader.GetString(_.Title);
                }
                if ((false == reader.IsDBNull(_.Url))) {
                    this._Url = reader.GetString(_.Url);
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
                if ((false == typeof(t_Link).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_Link>();
                
                /// <summary>
                /// 字段名：LinkID - 数据类型：Int32
                /// </summary>
                public static Field LinkID = new Field<t_Link>("LinkID");
                
                /// <summary>
                /// 字段名：CompanyID - 数据类型：String
                /// </summary>
                public static Field CompanyID = new Field<t_Link>("CompanyID");
                
                /// <summary>
                /// 字段名：Title - 数据类型：String
                /// </summary>
                public static Field Title = new Field<t_Link>("Title");
                
                /// <summary>
                /// 字段名：Url - 数据类型：String
                /// </summary>
                public static Field Url = new Field<t_Link>("Url");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_Link>("AddTime");
            }
        }
    }
