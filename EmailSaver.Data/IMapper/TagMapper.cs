using System;
using System.Data.SqlClient;

namespace EmailSaver.Data
{
	internal class TagMapper : IMapper<Tag>
	{
		public Tag ReadIteam(SqlDataReader reader)
		{
			return new Tag((Guid) reader["id"], (String) reader["name"]);
		}
	}
}