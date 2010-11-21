namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_Area �����У�ID
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
            /// ��ȡʵ���Ӧ�ı���
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_Area>("t_Area");
            }
            
            /// <summary>
            /// ��ȡʵ���еı�ʶ��
            /// </summary>
            protected override Field GetIdentityField() {
                return _.AreaID;
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
                        _.AreaID,
                        _.AreaName,
                        _.ParentID,
                        _.AddTime};
            }
            
            /// <summary>
            /// ��ȡ������
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
            /// ����ǰʵ�帳ֵ
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
                /// ��ʾѡ�������У���*��ͬ
                /// </summary>
                public static AllField All = new AllField<t_Area>();
                
                /// <summary>
                /// �ֶ�����ID - �������ͣ�Int32
                /// </summary>
                public static Field ID = new Field<t_Area>("ID");
                
                /// <summary>
                /// �ֶ�����AreaID - �������ͣ�String
                /// </summary>
                public static Field AreaID = new Field<t_Area>("AreaID");
                
                /// <summary>
                /// �ֶ�����AreaName - �������ͣ�String
                /// </summary>
                public static Field AreaName = new Field<t_Area>("AreaName");
                
                /// <summary>
                /// �ֶ�����ParentID - �������ͣ�String
                /// </summary>
                public static Field ParentID = new Field<t_Area>("ParentID");
                
                /// <summary>
                /// �ֶ�����AddTime - �������ͣ�DateTime(�ɿ�)
                /// </summary>
                public static Field AddTime = new Field<t_Area>("AddTime");
            }
        }
    }
    