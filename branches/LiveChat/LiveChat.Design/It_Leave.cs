using System;
using System.Collections.Generic;
using System.Text;
using MySoft.Data.Design;

namespace LiveChat.Design
{
	public interface t_Leave : IEntity
	{
		[PrimaryKey]
		int ID { get; }
        string CompanyID { get; set; }
		string Name { get; set; }
		string Telephone { get; set; }
		string Email { get; set; }
		string Title { get; set; }
		string Body { get; set; }
		string PostIP { get; set; }
		DateTime AddTime { get; set; }
	}
}
