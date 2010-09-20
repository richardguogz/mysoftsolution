namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_UGroup �����У�GroupID
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
            /// ��ȡʵ���Ӧ�ı���
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_UGroup>("t_UGroup");
            }
            
            /// <summary>
            /// ��ȡʵ���е�������
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.GroupID};
            }
            
            /// <summary>
            /// ��ȡ����Ϣ
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
            /// ��ȡ������
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
            /// ����ǰʵ�帳ֵ
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
                /// ��ʾѡ�������У���*��ͬ
                /// </summary>
                public static AllField All = new AllField<t_UGroup>();
                
                /// <summary>
                /// �ֶ�����GroupID - �������ͣ�Guid
                /// </summary>
                public static Field GroupID = new Field<t_UGroup>("GroupID");
                
                /// <summary>
                /// �ֶ�����CompanyID - �������ͣ�String
                /// </summary>
                public static Field CompanyID = new Field<t_UGroup>("CompanyID");
                
                /// <summary>
                /// �ֶ�����GroupName - �������ͣ�String
                /// </summary>
                public static Field GroupName = new Field<t_UGroup>("GroupName");
                
                /// <summary>
                /// �ֶ�����MaxPerson - �������ͣ�Int32(�ɿ�)
                /// </summary>
                public static Field MaxPerson = new Field<t_UGroup>("MaxPerson");
                
                /// <summary>
                /// �ֶ�����AddTime - �������ͣ�DateTime(�ɿ�)
                /// </summary>
                public static Field AddTime = new Field<t_UGroup>("AddTime");
            }
        }
    }
    