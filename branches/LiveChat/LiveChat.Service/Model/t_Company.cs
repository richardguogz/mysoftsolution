namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_Company �����У�ID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_Company : Entity {
            
            protected Int32 _ID;
            
            protected String _CompanyID;
            
            protected String _CompanyName;
            
            protected DateTime _AddTime;
            
            protected String _WebSite;
            
            protected String _ChatWebSite;
            
            protected String _CompanyLogo;
            
            protected Boolean _IsHeadquarters;
            
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
            
            public String CompanyName {
                get {
                    return this._CompanyName;
                }
                set {
                    this.OnPropertyValueChange(_.CompanyName, _CompanyName, value);
                    this._CompanyName = value;
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
            
            public String WebSite {
                get {
                    return this._WebSite;
                }
                set {
                    this.OnPropertyValueChange(_.WebSite, _WebSite, value);
                    this._WebSite = value;
                }
            }
            
            public String ChatWebSite {
                get {
                    return this._ChatWebSite;
                }
                set {
                    this.OnPropertyValueChange(_.ChatWebSite, _ChatWebSite, value);
                    this._ChatWebSite = value;
                }
            }
            
            public String CompanyLogo {
                get {
                    return this._CompanyLogo;
                }
                set {
                    this.OnPropertyValueChange(_.CompanyLogo, _CompanyLogo, value);
                    this._CompanyLogo = value;
                }
            }
            
            public Boolean IsHeadquarters {
                get {
                    return this._IsHeadquarters;
                }
                set {
                    this.OnPropertyValueChange(_.IsHeadquarters, _IsHeadquarters, value);
                    this._IsHeadquarters = value;
                }
            }
            
            /// <summary>
            /// ��ȡʵ���Ӧ�ı���
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_Company>("t_Company");
            }
            
            /// <summary>
            /// ��ȡʵ���еı�ʶ��
            /// </summary>
            protected override Field GetIdentityField() {
                return _.ID;
            }
            
            /// <summary>
            /// ��ȡʵ���е�������
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.ID};
            }
            
            /// <summary>
            /// ��ȡ����Ϣ
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.ID,
                        _.CompanyID,
                        _.CompanyName,
                        _.AddTime,
                        _.WebSite,
                        _.ChatWebSite,
                        _.CompanyLogo,
                        _.IsHeadquarters};
            }
            
            /// <summary>
            /// ��ȡ������
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._ID,
                        this._CompanyID,
                        this._CompanyName,
                        this._AddTime,
                        this._WebSite,
                        this._ChatWebSite,
                        this._CompanyLogo,
                        this._IsHeadquarters};
            }
            
            /// <summary>
            /// ����ǰʵ�帳ֵ
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.ID))) {
                    this._ID = reader.GetInt32(_.ID);
                }
                if ((false == reader.IsDBNull(_.CompanyID))) {
                    this._CompanyID = reader.GetString(_.CompanyID);
                }
                if ((false == reader.IsDBNull(_.CompanyName))) {
                    this._CompanyName = reader.GetString(_.CompanyName);
                }
                if ((false == reader.IsDBNull(_.AddTime))) {
                    this._AddTime = reader.GetDateTime(_.AddTime);
                }
                if ((false == reader.IsDBNull(_.WebSite))) {
                    this._WebSite = reader.GetString(_.WebSite);
                }
                if ((false == reader.IsDBNull(_.ChatWebSite))) {
                    this._ChatWebSite = reader.GetString(_.ChatWebSite);
                }
                if ((false == reader.IsDBNull(_.CompanyLogo))) {
                    this._CompanyLogo = reader.GetString(_.CompanyLogo);
                }
                if ((false == reader.IsDBNull(_.IsHeadquarters))) {
                    this._IsHeadquarters = reader.GetBoolean(_.IsHeadquarters);
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_Company).IsAssignableFrom(obj.GetType()))) {
                    return false;
                }
                if ((((object)(this)) == ((object)(obj)))) {
                    return true;
                }
                return false;
            }
            
            public class _ {
                
                /// <summary>
                /// ��ʾѡ�������У���*��ͬ
                /// </summary>
                public static AllField All = new AllField<t_Company>();
                
                /// <summary>
                /// �ֶ�����ID - �������ͣ�Int32
                /// </summary>
                public static Field ID = new Field<t_Company>("ID");
                
                /// <summary>
                /// �ֶ�����CompanyID - �������ͣ�String
                /// </summary>
                public static Field CompanyID = new Field<t_Company>("CompanyID");
                
                /// <summary>
                /// �ֶ�����CompanyName - �������ͣ�String
                /// </summary>
                public static Field CompanyName = new Field<t_Company>("CompanyName");
                
                /// <summary>
                /// �ֶ�����AddTime - �������ͣ�DateTime
                /// </summary>
                public static Field AddTime = new Field<t_Company>("AddTime");
                
                /// <summary>
                /// �ֶ�����WebSite - �������ͣ�String
                /// </summary>
                public static Field WebSite = new Field<t_Company>("WebSite");
                
                /// <summary>
                /// �ֶ�����ChatWebSite - �������ͣ�String
                /// </summary>
                public static Field ChatWebSite = new Field<t_Company>("ChatWebSite");
                
                /// <summary>
                /// �ֶ�����CompanyLogo - �������ͣ�String
                /// </summary>
                public static Field CompanyLogo = new Field<t_Company>("CompanyLogo");
                
                /// <summary>
                /// �ֶ�����IsHeadquarters - �������ͣ�Boolean
                /// </summary>
                public static Field IsHeadquarters = new Field<t_Company>("IsHeadquarters");
            }
        }
    }
    