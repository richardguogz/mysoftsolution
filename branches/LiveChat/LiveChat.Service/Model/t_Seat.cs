namespace LiveChat.Service {
        using System;
        using MySoft.Data;
        
        
        /// <summary>
        /// 表名：t_Seat 主键列：CompanyID,SeatCode
        /// </summary>
        [SerializableAttribute()]
        public partial class t_Seat : Entity {
            
            protected String _CompanyID;
            
            protected String _SeatCode;
            
            protected String _SeatName;
            
            protected String _Password;
            
            protected String _Sign;
            
            protected String _Introduction;
            
            protected LiveChat.Entity.SeatType _SeatType;
            
            protected DateTime _AddTime;
            
            protected DateTime? _LoginTime;
            
            protected DateTime? _LogoutTime;
            
            protected Int32 _LoginCount;
            
            protected String _Telephone;
            
            protected String _MobileNumber;
            
            protected String _Email;
            
            protected Byte[] _FaceImage;
            
            public String CompanyID {
                get {
                    return this._CompanyID;
                }
                set {
                    this.OnPropertyValueChange(_.CompanyID, _CompanyID, value);
                    this._CompanyID = value;
                }
            }
            
            public String SeatCode {
                get {
                    return this._SeatCode;
                }
                set {
                    this.OnPropertyValueChange(_.SeatCode, _SeatCode, value);
                    this._SeatCode = value;
                }
            }
            
            public String SeatName {
                get {
                    return this._SeatName;
                }
                set {
                    this.OnPropertyValueChange(_.SeatName, _SeatName, value);
                    this._SeatName = value;
                }
            }
            
            public String Password {
                get {
                    return this._Password;
                }
                set {
                    this.OnPropertyValueChange(_.Password, _Password, value);
                    this._Password = value;
                }
            }
            
            public String Sign {
                get {
                    return this._Sign;
                }
                set {
                    this.OnPropertyValueChange(_.Sign, _Sign, value);
                    this._Sign = value;
                }
            }
            
            public String Introduction {
                get {
                    return this._Introduction;
                }
                set {
                    this.OnPropertyValueChange(_.Introduction, _Introduction, value);
                    this._Introduction = value;
                }
            }
            
            public LiveChat.Entity.SeatType SeatType {
                get {
                    return this._SeatType;
                }
                set {
                    this.OnPropertyValueChange(_.SeatType, _SeatType, value);
                    this._SeatType = value;
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
            
            public DateTime? LoginTime {
                get {
                    return this._LoginTime;
                }
                set {
                    this.OnPropertyValueChange(_.LoginTime, _LoginTime, value);
                    this._LoginTime = value;
                }
            }
            
            public DateTime? LogoutTime {
                get {
                    return this._LogoutTime;
                }
                set {
                    this.OnPropertyValueChange(_.LogoutTime, _LogoutTime, value);
                    this._LogoutTime = value;
                }
            }
            
            public Int32 LoginCount {
                get {
                    return this._LoginCount;
                }
                set {
                    this.OnPropertyValueChange(_.LoginCount, _LoginCount, value);
                    this._LoginCount = value;
                }
            }
            
            public String Telephone {
                get {
                    return this._Telephone;
                }
                set {
                    this.OnPropertyValueChange(_.Telephone, _Telephone, value);
                    this._Telephone = value;
                }
            }
            
            public String MobileNumber {
                get {
                    return this._MobileNumber;
                }
                set {
                    this.OnPropertyValueChange(_.MobileNumber, _MobileNumber, value);
                    this._MobileNumber = value;
                }
            }
            
            public String Email {
                get {
                    return this._Email;
                }
                set {
                    this.OnPropertyValueChange(_.Email, _Email, value);
                    this._Email = value;
                }
            }
            
            public Byte[] FaceImage {
                get {
                    return this._FaceImage;
                }
                set {
                    this.OnPropertyValueChange(_.FaceImage, _FaceImage, value);
                    this._FaceImage = value;
                }
            }
            
            /// <summary>
            /// 获取实体对应的表名
            /// </summary>
            protected override Table GetTable() {
                return new Table<t_Seat>("t_Seat");
            }
            
            /// <summary>
            /// 获取实体中的主键列
            /// </summary>
            protected override Field[] GetPrimaryKeyFields() {
                return new Field[] {
                        _.CompanyID,
                        _.SeatCode};
            }
            
            /// <summary>
            /// 获取列信息
            /// </summary>
            protected override Field[] GetFields() {
                return new Field[] {
                        _.CompanyID,
                        _.SeatCode,
                        _.SeatName,
                        _.Password,
                        _.Sign,
                        _.Introduction,
                        _.SeatType,
                        _.AddTime,
                        _.LoginTime,
                        _.LogoutTime,
                        _.LoginCount,
                        _.Telephone,
                        _.MobileNumber,
                        _.Email,
                        _.FaceImage};
            }
            
            /// <summary>
            /// 获取列数据
            /// </summary>
            protected override object[] GetValues() {
                return new object[] {
                        this._CompanyID,
                        this._SeatCode,
                        this._SeatName,
                        this._Password,
                        this._Sign,
                        this._Introduction,
                        this._SeatType,
                        this._AddTime,
                        this._LoginTime,
                        this._LogoutTime,
                        this._LoginCount,
                        this._Telephone,
                        this._MobileNumber,
                        this._Email,
                        this._FaceImage};
            }
            
            /// <summary>
            /// 给当前实体赋值
            /// </summary>
            protected override void SetValues(IRowReader reader) {
                if ((false == reader.IsDBNull(_.CompanyID))) {
                    this._CompanyID = reader.GetString(_.CompanyID);
                }
                if ((false == reader.IsDBNull(_.SeatCode))) {
                    this._SeatCode = reader.GetString(_.SeatCode);
                }
                if ((false == reader.IsDBNull(_.SeatName))) {
                    this._SeatName = reader.GetString(_.SeatName);
                }
                if ((false == reader.IsDBNull(_.Password))) {
                    this._Password = reader.GetString(_.Password);
                }
                if ((false == reader.IsDBNull(_.Sign))) {
                    this._Sign = reader.GetString(_.Sign);
                }
                if ((false == reader.IsDBNull(_.Introduction))) {
                    this._Introduction = reader.GetString(_.Introduction);
                }
                if ((false == reader.IsDBNull(_.SeatType))) {
                    this._SeatType = ((LiveChat.Entity.SeatType)(reader.GetInt32(_.SeatType)));
                }
                if ((false == reader.IsDBNull(_.AddTime))) {
                    this._AddTime = reader.GetDateTime(_.AddTime);
                }
                if ((false == reader.IsDBNull(_.LoginTime))) {
                    this._LoginTime = reader.GetDateTime(_.LoginTime);
                }
                if ((false == reader.IsDBNull(_.LogoutTime))) {
                    this._LogoutTime = reader.GetDateTime(_.LogoutTime);
                }
                if ((false == reader.IsDBNull(_.LoginCount))) {
                    this._LoginCount = reader.GetInt32(_.LoginCount);
                }
                if ((false == reader.IsDBNull(_.Telephone))) {
                    this._Telephone = reader.GetString(_.Telephone);
                }
                if ((false == reader.IsDBNull(_.MobileNumber))) {
                    this._MobileNumber = reader.GetString(_.MobileNumber);
                }
                if ((false == reader.IsDBNull(_.Email))) {
                    this._Email = reader.GetString(_.Email);
                }
                if ((false == reader.IsDBNull(_.FaceImage))) {
                    this._FaceImage = reader.GetBytes(_.FaceImage);
                }
            }
            
            public override int GetHashCode() {
                return base.GetHashCode();
            }
            
            public override bool Equals(object obj) {
                if ((obj == null)) {
                    return false;
                }
                if ((false == typeof(t_Seat).IsAssignableFrom(obj.GetType()))) {
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
                public static AllField All = new AllField<t_Seat>();
                
                /// <summary>
                /// 字段名：CompanyID - 数据类型：String
                /// </summary>
                public static Field CompanyID = new Field<t_Seat>("CompanyID");
                
                /// <summary>
                /// 字段名：SeatCode - 数据类型：String
                /// </summary>
                public static Field SeatCode = new Field<t_Seat>("SeatCode");
                
                /// <summary>
                /// 字段名：SeatName - 数据类型：String
                /// </summary>
                public static Field SeatName = new Field<t_Seat>("SeatName");
                
                /// <summary>
                /// 字段名：Password - 数据类型：String
                /// </summary>
                public static Field Password = new Field<t_Seat>("Password");
                
                /// <summary>
                /// 字段名：Sign - 数据类型：String
                /// </summary>
                public static Field Sign = new Field<t_Seat>("Sign");
                
                /// <summary>
                /// 字段名：Introduction - 数据类型：String
                /// </summary>
                public static Field Introduction = new Field<t_Seat>("Introduction");
                
                /// <summary>
                /// 字段名：SeatType - 数据类型：SeatType
                /// </summary>
                public static Field SeatType = new Field<t_Seat>("SeatType");
                
                /// <summary>
                /// 字段名：AddTime - 数据类型：DateTime
                /// </summary>
                public static Field AddTime = new Field<t_Seat>("AddTime");
                
                /// <summary>
                /// 字段名：LoginTime - 数据类型：DateTime(可空)
                /// </summary>
                public static Field LoginTime = new Field<t_Seat>("LoginTime");
                
                /// <summary>
                /// 字段名：LogoutTime - 数据类型：DateTime(可空)
                /// </summary>
                public static Field LogoutTime = new Field<t_Seat>("LogoutTime");
                
                /// <summary>
                /// 字段名：LoginCount - 数据类型：Int32
                /// </summary>
                public static Field LoginCount = new Field<t_Seat>("LoginCount");
                
                /// <summary>
                /// 字段名：Telephone - 数据类型：String
                /// </summary>
                public static Field Telephone = new Field<t_Seat>("Telephone");
                
                /// <summary>
                /// 字段名：MobileNumber - 数据类型：String
                /// </summary>
                public static Field MobileNumber = new Field<t_Seat>("MobileNumber");
                
                /// <summary>
                /// 字段名：Email - 数据类型：String
                /// </summary>
                public static Field Email = new Field<t_Seat>("Email");
                
                /// <summary>
                /// 字段名：FaceImage - 数据类型：Byte[]
                /// </summary>
                public static Field FaceImage = new Field<t_Seat>("FaceImage");
            }
        }
    }
