using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
	public interface t_S2SSession : IEntity
	{
		[PrimaryKey]
		Guid SID { get; set; }
		string SessionID { get; set; }
		string CreateID { get; set; }
		string SeatID { get; set; }
		string FriendID { get; set; }
		string FromIP { get; set; }
		string FromAddress { get; set; }
		DateTime? LastReceiveTime { get; set; }
	}
}
