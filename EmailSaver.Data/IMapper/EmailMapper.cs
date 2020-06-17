using System;
using System.Data.SqlClient;

namespace EmailSaver.Data
{
	internal class EmailMapper : IMapper<Email>
	{
		public Email ReadIteam(SqlDataReader reader)
		{
			return new Email((Guid) reader["id"], (DateTime) reader["registration_date"], (String) reader["sender"],
				(String) reader["recipient"], (String) reader["subject"], (String) reader["text"],
				(String) reader["tags"]);
		}
	}
}