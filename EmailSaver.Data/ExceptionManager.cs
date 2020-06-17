using System;
using System.Text;
using System.Data.SqlClient;

namespace EmailSaver.Data
{
	internal class ExceptionManager
	{
		#region Singleton

		private static readonly Object _sync = new Object();
		private static volatile ExceptionManager _instance;

		public static ExceptionManager Instance
		{
			get
			{
				if (_instance != null) return _instance;

				lock (_sync)
				{
					if (_instance == null) _instance = new ExceptionManager();
				}

				return _instance;
			}
		}

		#endregion

		private readonly Action<String> _publisher;

		private ExceptionManager()
		{
			_publisher = Console.WriteLine;
		}

		public void Publish(Exception exception, SqlCommand command = null)
		{
			if (command == null)
			{
				_publisher(exception.Message + "\n" + exception.StackTrace);
				return;
			}

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
			builder.AppendLine($"\nStack trace:\n{exception.StackTrace}");

			_publisher(builder.ToString());
		}
	}
}