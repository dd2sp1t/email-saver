using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Data
{
	internal class AdoHelper
	{
		#region Singleton

		private static readonly Object _sync = new Object();
		private static volatile AdoHelper _instance;

		public static AdoHelper Instance
		{
			get
			{
				if (_instance != null) return _instance;

				lock (_sync)
				{
					if (_instance == null) _instance = new AdoHelper();
				}

				return _instance;
			}
		}

		#endregion

		public String ConnectionString { get; }

		// todo: use config manager
		private AdoHelper()
		{
			ConnectionString =
				"data source=localhost; initial catalog=email_saver_db; " +
				"persist security info=True; Integrated Security=SSPI; " +
				"MultipleActiveResultSets=True;";

			if (!String.IsNullOrEmpty(ConnectionString)) return;

			throw new Exception("Connection string not found in config file.");
		}

		public async Task<TItem> FindItemAsync<TItem>(SqlCommand command, IMapper<TItem> mapper) where TItem : class
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) await command.Connection.OpenAsync();
				await using SqlDataReader reader = await command.ExecuteReaderAsync();

				TItem result = null;
				if (await reader.ReadAsync()) result = mapper.ReadIteam(reader);

				return result;
			}
			catch (Exception ex)
			{
				ExceptionManager.Instance.Publish(ex, command);
				throw;
			}
		}

		public async Task<TItem> GetItemAsync<TItem>(SqlCommand command, IMapper<TItem> mapper) where TItem : class
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) await command.Connection.OpenAsync();
				await using SqlDataReader reader = await command.ExecuteReaderAsync();

				if (await reader.ReadAsync()) return mapper.ReadIteam(reader);

				throw new Exception("Item not found.");
			}
			catch (Exception ex)
			{
				ExceptionManager.Instance.Publish(ex, command);
				throw;
			}
		}

		public async Task<List<TItem>> GetItemsAsync<TItem>(SqlCommand command, IMapper<TItem> mapper)
			where TItem : class
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) await command.Connection.OpenAsync();
				await using SqlDataReader reader = await command.ExecuteReaderAsync();

				var result = new List<TItem>();
				while (await reader.ReadAsync()) result.Add(mapper.ReadIteam(reader));

				return result;
			}
			catch (Exception ex)
			{
				ExceptionManager.Instance.Publish(ex, command);
				throw;
			}
		}

		public async Task<Guid> AddItemAsync(SqlCommand command)
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) await command.Connection.OpenAsync();
				if (await command.ExecuteScalarAsync() is Guid id) return id;

				throw new Exception("Item was not added.");
			}
			catch (Exception ex)
			{
				ExceptionManager.Instance.Publish(ex, command);
				throw;
			}
		}

		public async Task<Boolean> ExecuteAsync(SqlCommand command)
		{
			try
			{
				if (command.Connection.State != ConnectionState.Open) await command.Connection.OpenAsync();
				return await command.ExecuteNonQueryAsync() > 0;
			}
			catch (Exception ex)
			{
				ExceptionManager.Instance.Publish(ex, command);
				throw;
			}
		}

		/// <summary>
		/// Creates a command without attaching to any transaction.
		/// </summary>
		/// <param name="procedure">Name of procedure to be called.</param>
		/// <param name="connection">Current connection to database.</param>
		/// <param name="params">Set of parameters to the procedure.</param>
		public SqlCommand CreateProcedureCommand(String procedure, SqlConnection connection,
			params SqlParameter[] @params)
		{
			var command = new SqlCommand(procedure, connection);

			command.Parameters.AddRange(@params);
			command.CommandType = CommandType.StoredProcedure;

			return command;
		}

		/// <summary>
		/// Creates a command as a part of transaction.
		/// </summary>
		/// <param name="procedure">Name of procedure to be called.</param>
		/// <param name="transaction">Current transaction to attach the procedure.</param>
		/// <param name="params">Set of parameters to the procedure.</param>
		public SqlCommand CreateProcedureCommand(String procedure, SqlTransaction transaction,
			params SqlParameter[] @params)
		{
			var command = new SqlCommand(procedure, transaction.Connection, transaction);

			command.Parameters.AddRange(@params);
			command.CommandType = CommandType.StoredProcedure;

			return command;
		}
	}
}