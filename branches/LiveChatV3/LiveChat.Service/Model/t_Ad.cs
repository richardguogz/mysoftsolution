namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_Ad �����У�ID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_Ad : Entity {
            
            protected Int32 _ID;
            
            protected String _CompanyID;
            
            protected String _AdName;
            
            protected String _AdTitle;
            
            protected String _AdArea;
            
            protected String _AdImgUrl;
            
            protected String _AdUrl;
            
            protected String _AdLogoImgUrl;
            
            protected String _AdLogoUrl;
            
            protected String _AdText;
            
            protected String _AdTextUrl;
            
            protected Boolean _IsDefault;
            
            protected DateTime _AddTime;
            
            protected Boolean _IsCommon;
            
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
            
            public String AdName {
                get {
                    return this._AdName;
                }
                set {
                    this.OnPropertyValueChange(_.AdName, _AdName, value);
                    this._AdName = value;
                }
            }
            
            public String AdTitle {
                get {
                    return this._AdTitle;
                }
                set {
                    this.OnPropertyValueChange(_.AdTitle, _AdTitle, value);
                    this._AdTitle = value;
                }
            }
            
            public String AdArea {
                get {
                    return this._AdArea;
                }
                set {
                    this.OnPropertyValueChange(_.AdArea, _AdArea, value);
                    this._AdArea = value;
                }
            }
            
            public String AdImgUrl {
                get {
                    return this._AdImgUrl;
                }
                set {
                    this.OnPropertyValueChange(_.AdImgUrl, _AdImgUrl, value);
                    this._AdImgUrl = value;
                }
            }
            
            public String AdUrl {
                get {
                    return this._AdUrl;
                }
                set {
                    this.OnPropertyValueChange(_.AdUrl, _AdUrl, value);
                    this._AdUrl = value;
                }
            }
            
            public String AdLogoImgUrl {
                get {
                    return this._AdLogoImgUrl;
                }
                set {
                    this.OnPropertyValueChange(_.AdLogoImgUrl, _AdLogoImgUrl, value);
                    this._AdLogoImgUrl = value;
                }
            }
            
            public String AdLogoUrl {
                get {
                    return this._AdLogoUrl;
                }
                set {
                    this.OnPropertyValueChange(_.AdLogoUrl, _AdLogoUrl, value);
                    this._AdLogoUrl = value;
                }
            }
            
            public String AdText {
                get {
                    return this._AdText;
                }
                set {
                    this.OnPropertyValueChange(_.AdText, _AdText, value);
                    this._AdText = value;
                }
            }
            
            public String AdTextUrl {
                get {
                    return this._AdTextUrl;
                }
                set {
                    this.OnPropertyValueChange(_.AdTextUrl, _AdTextUrl, value);
                    this._AdTextUrl = value;
                }
            }
            
            public Boolean IsDefault {
                get {
                    return this._IsDefault;
                }
                set {
                    this.OnPropertyValueChange(_.IsDefault, _IsDefault, value);
                    this._IsDefault = value;
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
            
            public Boolean IsCommon {
                get {
                    return this._IsCommon;
                }
                set {
                    this.OnPropertyValueChange(_.IsCommon, _IsCommon, value);
                    this._IsCommon = value;
                }
            }
            
            /// <summary>
            /// ��ȡʵ���Ӧ�ı���
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_Ad>("t_Ad");
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
                        _.AdName,
                        _.AdTitle,
                        _.AdArea,
                        _.AdImgUrl,
                        _.AdUrl,
                        _.AdLogoImgUrl,
                        _.AdLogoUrl,
                        _.AdText,
                        _.AdTextUrl,
                        _.IsDefault,
                        _.AddTime,
                        _.IsCommon};
            }
            
            /// <summary>
            /// ��ȡ������
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._ID,
                        this._CompanyID,
                        this._AdName,
                        this._AdTitle,
                        this._AdArea,
                        this._AdImgUrl,
                        this._AdUrl,
                        this._AdLogoImgUrl,
                        this._AdLogoUrl,
                        this._AdText,
                        this._AdTextUrl,
                        this._IsDefault,
                        this._AddTime,
                        this._IsCommon};
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
                if ((false == reader.IsDBNull(_.AdName))) {
                    this._AdName = reader.GetString(_.AdName);
                }
                if ((false == reader.IsDBNull(_.AdTitle))) {
                    this._AdTitle = reader.GetString(_.AdTitle);
                }
                if ((false == reader.IsDBNull(_.AdArea))) {
                    this._AdArea = reader.GetString(_.AdArea);
                }
                if ((false == reader.IsDBNull(_.AdImgUrl))) {
                    this._AdImgUrl = reader.GetString(_.AdImgUrl);
                }
                if ((false == reader.IsDBNull(_.AdUrl))) {
                    this._AdUrl = reader.GetString(_.AdUrl);
                }
                if ((false == reader.IsDBNull(_.AdLogoImgUrl))) {
                    this._AdLogoImgUrl = reader.GetString(_.AdLogoImgUrl);
                }
                if ((false == reader.IsDBNull(_.AdLogoUrl))) {
                    this._AdLogoUrl = reader.GetString(_.AdLogoUrl);
                }
                if ((false == reader.IsDBNull(_.AdText))) {
                    this._AdText = reader.GetString(_.AdText);
                }
                if ((false == reader.IsDBNull(_.AdTextUrl))) {
                    this._AdTextUrl = reader.GetString(_.AdTextUrl);
                }
                if ((false == reader.IsDBNull(_.IsDefault))) {
                    this._IsDefault = reader.GetBoolean(_.IsDefault);
                }
                if ((false == reader.IsDBNull(_.AddTime))) {
                    this._AddTime = reader.GetDateTime(_.AddTime);
                }
                if ((false == reader.IsDBNull(_.IsCommon))) {
                    this._IsCommon = reader.GetBoolean(_.IsCommon);
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_Ad).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_Ad>();
                
                /// <summary>
                /// �ֶ�����ID - �������ͣ�Int32
                /// </summary>
                public static Field ID = new Field<t_Ad>("ID");
                
                /// <summary>
                /// �ֶ�����CompanyID - �������ͣ�String
                /// </summary>
                public static Field CompanyID = new Field<t_Ad>("CompanyID");
                
                /// <summary>
                /// �ֶ�����AdName - �������ͣ�String
                /// </summary>
                public static Field AdName = new Field<t_Ad>("AdName");
                
                /// <summary>
                /// �ֶ�����AdTitle - �������ͣ�String
                /// </summary>
                public static Field AdTitle = new Field<t_Ad>("AdTitle");
                
                /// <summary>
                /// �ֶ�����AdArea - �������ͣ�String
                /// </summary>
                public static Field AdArea = new Field<t_Ad>("AdArea");
                
                /// <summary>
                /// �ֶ�����AdImgUrl - �������ͣ�String
                /// </summary>
                public static Field AdImgUrl = new Field<t_Ad>("AdImgUrl");
                
                /// <summary>
                /// �ֶ�����AdUrl - �������ͣ�String
                /// </summary>
                public static Field AdUrl = new Field<t_Ad>("AdUrl");
                
                /// <summary>
                /// �ֶ�����AdLogoImgUrl - �������ͣ�String
                /// </summary>
                public static Field AdLogoImgUrl = new Field<t_Ad>("AdLogoImgUrl");
                
                /// <summary>
                /// �ֶ�����AdLogoUrl - �������ͣ�String
                /// </summary>
                public static Field AdLogoUrl = new Field<t_Ad>("AdLogoUrl");
                
                /// <summary>
                /// �ֶ�����AdText - �������ͣ�String
                /// </summary>
                public static Field AdText = new Field<t_Ad>("AdText");
                
                /// <summary>
                /// �ֶ�����AdTextUrl - �������ͣ�String
                /// </summary>
                public static Field AdTextUrl = new Field<t_Ad>("AdTextUrl");
                
                /// <summary>
                /// �ֶ�����IsDefault - �������ͣ�Boolean
                /// </summary>
                public static Field IsDefault = new Field<t_Ad>("IsDefault");
                
                /// <summary>
                /// �ֶ�����AddTime - �������ͣ�DateTime
                /// </summary>
                public static Field AddTime = new Field<t_Ad>("AddTime");
                
                /// <summary>
                /// �ֶ�����IsCommon - �������ͣ�Boolean
                /// </summary>
                public static Field IsCommon = new Field<t_Ad>("IsCommon");
            }
        }
    }
    