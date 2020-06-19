using System;
using System.Text;
using System.Data.SqlClient;

namespace EmailSaver.Data
{
	internal static class ExceptionManager
	{
		public static String BuildMessage(Exception exception, SqlCommand command)
		{
			var builder = new StringBuilder(1024);

			builder.AppendLine(exception.Message + "\n");
			builder.AppendLine($"Command type: {command.CommandType}");
			builder.AppendLine($"Command text: {command.CommandText}");

			if (command.Parameters.Count > 0)
			{
				builder.AppendLine("Parameters:");
				foreach (SqlParameter parameter in command.Parameters)
				{
					String value = parameter.Value == null ? "NULL" : parameter.Value.ToString();
					builder.AppendLine($"	{parameter.ParameterName} = {value}");
				}
			}

			builder.AppendLine($"Database: {command.Connection.Database}");
			builder.AppendLine($"Connection string: {command.Connection.ConnectionString}");

			return builder.ToString();
		}
	}
}