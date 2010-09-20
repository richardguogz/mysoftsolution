namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_Reply �����У�ReplyID
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
            /// ��ȡʵ���Ӧ�ı���
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_Reply>("t_Reply");
            }
            
            /// <summary>
            /// ��ȡʵ���еı�ʶ��
            /// </summary>
            protected override Field GetIdentityField() {
                return _.ReplyID;
            }
            
            /// <summary>
            /// ��ȡʵ���е�������
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.ReplyID};
            }
            
            /// <summary>
            /// ��ȡ����Ϣ
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
            /// ��ȡ������
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
            /// ����ǰʵ�帳ֵ
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
                /// ��ʾѡ�������У���*��ͬ
                /// </summary>
                public static AllField All = new AllField<t_Reply>();
                
                /// <summary>
                /// �ֶ�����ReplyID - �������ͣ�Int32
                /// </summary>
                public static Field ReplyID = new Field<t_Reply>("ReplyID");
                
                /// <summary>
                /// �ֶ�����CompanyID - �������ͣ�String
                /// </summary>
                public static Field CompanyID = new Field<t_Reply>("CompanyID");
                
                /// <summary>
                /// �ֶ�����Title - �������ͣ�String
                /// </summary>
                public static Field Title = new Field<t_Reply>("Title");
                
                /// <summary>
                /// �ֶ�����Content - �������ͣ�String
                /// </summary>
                public static Field Content = new Field<t_Reply>("Content");
                
                /// <summary>
                /// �ֶ�����AddTime - �������ͣ�DateTime
                /// </summary>
                public static Field AddTime = new Field<t_Reply>("AddTime");
            }
        }
    }
    