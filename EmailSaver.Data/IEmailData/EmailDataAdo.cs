using System;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EmailSaver.Data
{
	public class EmailDataAdo : IEmailData
	{
		private readonly EmailMapper _mapper = new EmailMapper();
		private readonly AdoHelper _helper = AdoHelper.Instance;

		public async Task<Email> GetAsync(Guid id)
		{
			var @params = new SqlParameter[] {new SqlParameter("@id", id)};
			await using var connection = new SqlConnection(_helper.ConnectionString);
			await using SqlCommand command = _helper.CreateProcedureCommand("sp_get_email", connection, @params);

			return await _helper.GetItemAsync(command, _mapper);
		}

		public async Task<List<Email>> GetAllAsync()
		{
			var @params = new SqlParameter[0];
			await using var connection = new SqlConnection(_helper.ConnectionString);
			await using SqlCommand command = _helper.CreateProcedureCommand("sp_get_emails", connection, @params);

			return await _helper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetByTagAsync(String tag)
		{
			var @params = new SqlParameter[] {new SqlParameter("@tag", tag)};
			await using var connection = new SqlConnection(_helper.ConnectionString);
			await using SqlCommand command =
				_helper.CreateProcedureCommand("sp_get_emails_by_tag", connection, @params);

			return await _helper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetBySenderAsync(String sender)
		{
			var @params = new SqlParameter[] {new SqlParameter("@sender", sender)};
			await using var connection = new SqlConnection(_helper.ConnectionString);
			await using SqlCommand command =
				_helper.CreateProcedureCommand("sp_get_emails_by_sender", connection, @params);

			return await _helper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetByRecipientAsync(String recipient)
		{
			var @params = new SqlParameter[] {new SqlParameter("@recipient", recipient)};
			await using var connection = new SqlConnection(_helper.ConnectionString);
			await using SqlCommand command =
				_helper.CreateProcedureCommand("sp_get_emails_by_recipient", connection, @params);

			return await _helper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetForPeriodAsync(DateTime start, DateTime end)
		{
			if (start > end) throw new Exception("Invalid dates order.");

			var @params = new SqlParameter[]
			{
				new SqlParameter("@start_date", start.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@end_date", end.ToString("yyyy-MM-dd HH:mm:ss"))
			};

			await using var connection = new SqlConnection(_helper.ConnectionString);
			await using SqlCommand command =
				_helper.CreateProcedureCommand("sp_get_emails_for_period", connection, @params);

			return await _helper.GetItemsAsync(command, _mapper);
		}

		public async Task<Guid> AddAsync(Email email, List<String> tags)
		{
			await using var connection = new SqlConnection(_helper.ConnectionString);

			await connection.OpenAsync();

			await using SqlTransaction transaction = connection.BeginTransaction();

			try
			{
				var dtos = await GetValidTags(tags, connection, transaction);

				String serialized = JsonConvert.SerializeObject(dtos.Select(t => t.Name).ToList());

				Guid id = await AddEmail(email, serialized, connection, transaction);

				await AddEmailTags(id, dtos, connection, transaction);

				await transaction.CommitAsync();

				return id;
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw new Exception("Errors was occured. Transaction was declined.", ex);
			}
		}

		public Task<Boolean> UpdateAsync(Email email, List<String> tags)
		{
			throw new NotImplementedException();
		}

		public async Task<Boolean> DeleteAsync(Guid id)
		{
			var @params = new SqlParameter[] {new SqlParameter("@id", id)};
			await using var connection = new SqlConnection(_helper.ConnectionString);
			await using SqlCommand command = _helper.CreateProcedureCommand("sp_delete_email", connection, @params);

			return await _helper.ExecuteAsync(command);
		}

		/// <summary>
		/// Adds new tags to database and returns valid GUIDs for all tag set.
		/// </summary>
		/// <param name="tags">Email tags collection.</param>
		/// <param name="connection">Current connection to database.</param>
		/// <param name="transaction">Current transaction to attach possible insert action.</param>
		private async Task<List<Tag>> GetValidTags(IEnumerable<String> tags, SqlConnection connection,
			SqlTransaction transaction)
		{
			if (tags == null) return new List<Tag>();

			var temps = tags.Select(name => new
				{Name = name, Task = GetTagId(name, connection, transaction)}).ToList();

			await Task.WhenAll(temps.Select(_ => _.Task));

			return temps.Select(_ => new Tag(_.Task.Result, _.Name)).ToList();
		}

		/// <summary>
		/// For existing tag just returns its GUID.
		/// For new tag adds the one to database as a part of transaction and returns its GUID.
		/// </summary>
		/// <param name="name">Tag name.</param>
		/// <param name="connection">Current connection to database.</param>
		/// <param name="transaction">Current transaction to attach possible insert action.</param>
		private async Task<Guid> GetTagId(String name, SqlConnection connection, SqlTransaction transaction)
		{
			// todo: change procedure name in .sql file
			var procedure = "sp_get_tag_id_or_add";
			var param = new SqlParameter("@name", name.ToLower());
			await using SqlCommand command = _helper.CreateProcedureCommand(procedure, connection, transaction, param);

			return await _helper.AddItemAsync(command);
		}

		/// <summary>
		/// Adds email entity to database as a part of transaction and returns its GUID.
		/// </summary>
		/// <param name="email">Email data transfer object.</param>
		/// <param name="tags">Serialized string list of valid tags names.</param>
		/// <param name="connection">Current connection to database.</param>
		/// <param name="transaction">Current transaction to attach insert action.</param>
		private async Task<Guid> AddEmail(Email email, String tags, SqlConnection connection,
			SqlTransaction transaction)
		{
			var procedure = "sp_add_email";
			var @params = new SqlParameter[]
			{
				new SqlParameter("@registration_date", email.Date.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sender", email.Sender.ToLower()),
				new SqlParameter("@recipient", email.Recipient.ToLower()),
				new SqlParameter("@subject", email.Subject),
				new SqlParameter("@text", email.Text),
				new SqlParameter("@tags", tags)
			};

			await using SqlCommand command =
				_helper.CreateProcedureCommand(procedure, connection, transaction, @params);

			return await _helper.AddItemAsync(command);
		}

		private Task AddEmailTags(Guid emailId, IEnumerable<Tag> tags, SqlConnection cnn, SqlTransaction trn)
		{
			return Task.WhenAll(tags.Select(t => AddEmailTag(emailId, t.Id, cnn, trn)));
		}

		private async Task<Guid> AddEmailTag(Guid emailId, Guid tagId, SqlConnection connection,
			SqlTransaction transaction)
		{
			var procedure = "sp_add_email_tag";
			var @params = new SqlParameter[]
				{new SqlParameter("@email_id", emailId), new SqlParameter("@tag_id", tagId)};

			await using SqlCommand command =
				_helper.CreateProcedureCommand(procedure, connection, transaction, @params);

			return await _helper.AddItemAsync(command);
		}
	}
}