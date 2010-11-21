namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_SeatFriendGroup �����У�SeatID,GroupID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_SeatFriendGroup : Entity {
            
            protected String _SeatID;
            
            protected Guid _GroupID;
            
            protected String _GroupName;
            
            protected String _Remark;
            
            protected DateTime _AddTime;
            
            public String SeatID {
                get {
                    return this._SeatID;
                }
                set {
                    this.OnPropertyValueChange(_.SeatID, _SeatID, value);
                    this._SeatID = value;
                }
            }
            
            public Guid GroupID {
                get {
                    return this._GroupID;
                }
                set {
                    this.OnPropertyValueChange(_.GroupID, _GroupID, value);
                    this._GroupID = value;
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
            
            public String Remark {
                get {
                    return this._Remark;
                }
                set {
                    this.OnPropertyValueChange(_.Remark, _Remark, value);
                    this._Remark = value;
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
                return new Table<t_SeatFriendGroup>("t_SeatFriendGroup");
            }
            
            /// <summary>
            /// ��ȡʵ���е�������
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.SeatID,
                        _.GroupID};
            }
            
            /// <summary>
            /// ��ȡ����Ϣ
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.SeatID,
                        _.GroupID,
                        _.GroupName,
                        _.Remark,
                        _.AddTime};
            }
            
            /// <summary>
            /// ��ȡ������
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._SeatID,
                        this._GroupID,
                        this._GroupName,
                        this._Remark,
                        this._AddTime};
            }
            
            /// <summary>
            /// ����ǰʵ�帳ֵ
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.SeatID))) {
                    this._SeatID = reader.GetString(_.SeatID);
                }
                if ((false == reader.IsDBNull(_.GroupID))) {
                    this._GroupID = reader.GetGuid(_.GroupID);
                }
                if ((false == reader.IsDBNull(_.GroupName))) {
                    this._GroupName = reader.GetString(_.GroupName);
                }
                if ((false == reader.IsDBNull(_.Remark))) {
                    this._Remark = reader.GetString(_.Remark);
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
                if ((false == typeof(t_SeatFriendGroup).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_SeatFriendGroup>();
                
                /// <summary>
                /// �ֶ�����SeatID - �������ͣ�String
                /// </summary>
                public static Field SeatID = new Field<t_SeatFriendGroup>("SeatID");
                
                /// <summary>
                /// �ֶ�����GroupID - �������ͣ�Guid
                /// </summary>
                public static Field GroupID = new Field<t_SeatFriendGroup>("GroupID");
                
                /// <summary>
                /// �ֶ�����GroupName - �������ͣ�String
                /// </summary>
                public static Field GroupName = new Field<t_SeatFriendGroup>("GroupName");
                
                /// <summary>
                /// �ֶ�����Remark - �������ͣ�String
                /// </summary>
                public static Field Remark = new Field<t_SeatFriendGroup>("Remark");
                
                /// <summary>
                /// �ֶ�����AddTime - �������ͣ�DateTime
                /// </summary>
                public static Field AddTime = new Field<t_SeatFriendGroup>("AddTime");
            }
        }
    }
    