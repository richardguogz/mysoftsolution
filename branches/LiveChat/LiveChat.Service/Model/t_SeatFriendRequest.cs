namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// ������t_SeatFriendRequest �����У�RequestID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_SeatFriendRequest : Entity {
            
            protected Int32 _RequestID;
            
            protected String _SeatID;
            
            protected String _FriendID;
            
            protected String _Request;
            
            protected String _Refuse;
            
            protected Int32 _ConfirmState;
            
            protected DateTime _AddTime;
            
            public Int32 RequestID {
                get {
                    return this._RequestID;
                }
                set {
                    this.OnPropertyValueChange(_.RequestID, _RequestID, value);
                    this._RequestID = value;
                }
            }
            
            public String SeatID {
                get {
                    return this._SeatID;
                }
                set {
                    this.OnPropertyValueChange(_.SeatID, _SeatID, value);
                    this._SeatID = value;
                }
            }
            
            public String FriendID {
                get {
                    return this._FriendID;
                }
                set {
                    this.OnPropertyValueChange(_.FriendID, _FriendID, value);
                    this._FriendID = value;
                }
            }
            
            public String Request {
                get {
                    return this._Request;
                }
                set {
                    this.OnPropertyValueChange(_.Request, _Request, value);
                    this._Request = value;
                }
            }
            
            public String Refuse {
                get {
                    return this._Refuse;
                }
                set {
                    this.OnPropertyValueChange(_.Refuse, _Refuse, value);
                    this._Refuse = value;
                }
            }
            
            public Int32 ConfirmState {
                get {
                    return this._ConfirmState;
                }
                set {
                    this.OnPropertyValueChange(_.ConfirmState, _ConfirmState, value);
                    this._ConfirmState = value;
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
                return new Table<t_SeatFriendRequest>("t_SeatFriendRequest");
            }
            
            /// <summary>
            /// ��ȡʵ���еı�ʶ��
            /// </summary>
            protected override Field GetIdentityField() {
                return _.RequestID;
            }
            
            /// <summary>
            /// ��ȡʵ���е�������
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.RequestID};
            }
            
            /// <summary>
            /// ��ȡ����Ϣ
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.RequestID,
                        _.SeatID,
                        _.FriendID,
                        _.Request,
                        _.Refuse,
                        _.ConfirmState,
                        _.AddTime};
            }
            
            /// <summary>
            /// ��ȡ������
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._RequestID,
                        this._SeatID,
                        this._FriendID,
                        this._Request,
                        this._Refuse,
                        this._ConfirmState,
                        this._AddTime};
            }
            
            /// <summary>
            /// ����ǰʵ�帳ֵ
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.RequestID))) {
                    this._RequestID = reader.GetInt32(_.RequestID);
                }
                if ((false == reader.IsDBNull(_.SeatID))) {
                    this._SeatID = reader.GetString(_.SeatID);
                }
                if ((false == reader.IsDBNull(_.FriendID))) {
                    this._FriendID = reader.GetString(_.FriendID);
                }
                if ((false == reader.IsDBNull(_.Request))) {
                    this._Request = reader.GetString(_.Request);
                }
                if ((false == reader.IsDBNull(_.Refuse))) {
                    this._Refuse = reader.GetString(_.Refuse);
                }
                if ((false == reader.IsDBNull(_.ConfirmState))) {
                    this._ConfirmState = reader.GetInt32(_.ConfirmState);
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
                if ((false == typeof(t_SeatFriendRequest).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_SeatFriendRequest>();
                
                /// <summary>
                /// �ֶ�����RequestID - �������ͣ�Int32
                /// </summary>
                public static Field RequestID = new Field<t_SeatFriendRequest>("RequestID");
                
                /// <summary>
                /// �ֶ�����SeatID - �������ͣ�String
                /// </summary>
                public static Field SeatID = new Field<t_SeatFriendRequest>("SeatID");
                
                /// <summary>
                /// �ֶ�����FriendID - �������ͣ�String
                /// </summary>
                public static Field FriendID = new Field<t_SeatFriendRequest>("FriendID");
                
                /// <summary>
                /// �ֶ�����Request - �������ͣ�String
                /// </summary>
                public static Field Request = new Field<t_SeatFriendRequest>("Request");
                
                /// <summary>
                /// �ֶ�����Refuse - �������ͣ�String
                /// </summary>
                public static Field Refuse = new Field<t_SeatFriendRequest>("Refuse");
                
                /// <summary>
                /// �ֶ�����ConfirmState - �������ͣ�Int32
                /// </summary>
                public static Field ConfirmState = new Field<t_SeatFriendRequest>("ConfirmState");
                
                /// <summary>
                /// �ֶ�����AddTime - �������ͣ�DateTime
                /// </summary>
                public static Field AddTime = new Field<t_SeatFriendRequest>("AddTime");
            }
        }
    }
