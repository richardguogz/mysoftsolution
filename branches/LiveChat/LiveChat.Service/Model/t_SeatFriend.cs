namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_SeatFriend 主键列：SeatID,FriendID
        /// </summary>
        [SerializableAttribute()]
        public partial class t_SeatFriend : Entity {
            
            protected String _SeatID;
            
            protected String _FriendID;
            
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
            
            public String FriendID {
                get {
                    return this._FriendID;
                }
                set {
                    this.OnPropertyValueChange(_.FriendID, _FriendID, value);
                    this._FriendID = value;
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
                return new Table<t_SeatFriend>("t_SeatFriend");
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.SeatID,
                        _.FriendID};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.SeatID,
                        _.FriendID,
                        _.AddTime};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._SeatID,
                        this._FriendID,
                        this._AddTime};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.SeatID))) {
                    this._SeatID = reader.GetString(_.SeatID);
                }
                if ((false == reader.IsDBNull(_.FriendID))) {
                    this._FriendID = reader.GetString(_.FriendID);
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
                if ((false == typeof(t_SeatFriend).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_SeatFriend>();
                
                /// <summary>
                /// 字段名：SeatID - 数据类型：String
                /// </summary>
                public static Field SeatID = new Field<t_SeatFriend>("SeatID");
                
                /// <summary>
                /// 字段名：FriendID - 数据类型：String
                /// </summary>
                public static Field FriendID = new Field<t_SeatFriend>("FriendID");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_SeatFriend>("AddTime");
            }
        }
    }
    