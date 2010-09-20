namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_Reply 主键列：ReplyID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_Reply : Entity {
            
            protected Int32 _ReplyID;
            
            protected String _CompanyID;
            
            protected String _Title;
            
            protected String _Content;
            
            protected DateTime _AddTime;
            
            public Int32 ReplyID {
                get {
                    return this._ReplyID;
                }
                set {
                    this.OnPropertyValueChange(_.ReplyID, _ReplyID, value);
                    this._ReplyID = value;
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
            
            public String Content {
                get {
                    return this._Content;
                }
                set {
                    this.OnPropertyValueChange(_.Content, _Content, value);
                    this._Content = value;
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
                return new Table<t_Reply>("t_Reply");
            }
            
            /// <summary>
            /// 获取实体中的标识列
            /// </summary>
            protected override Field GetIdentityField() {
                return _.ReplyID;
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.ReplyID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.ReplyID,
                        _.CompanyID,
                        _.Title,
                        _.Content,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._ReplyID,
                        this._CompanyID,
                        this._Title,
                        this._Content,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.ReplyID))) {
                    this._ReplyID = reader.GetInt32(_.ReplyID);
                }
                if ((false == reader.IsDBNull(_.CompanyID))) {
                    this._CompanyID = reader.GetString(_.CompanyID);
                }
                if ((false == reader.IsDBNull(_.Title))) {
                    this._Title = reader.GetString(_.Title);
                }
                if ((false == reader.IsDBNull(_.Content))) {
                    this._Content = reader.GetString(_.Content);
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
                if ((false == typeof(t_Reply).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_Reply>();
                
                /// <summary>
                /// 字段名：ReplyID - 数据类型：Int32
                /// </summary>
                public static Field ReplyID = new Field<t_Reply>("ReplyID");
                
                /// <summary>
                /// 字段名：CompanyID - 数据类型：String
                /// </summary>
                public static Field CompanyID = new Field<t_Reply>("CompanyID");
                
                /// <summary>
                /// 字段名：Title - 数据类型：String
                /// </summary>
                public static Field Title = new Field<t_Reply>("Title");
                
                /// <summary>
                /// 字段名：Content - 数据类型：String
                /// </summary>
                public static Field Content = new Field<t_Reply>("Content");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_Reply>("AddTime");
            }
        }
    }
    