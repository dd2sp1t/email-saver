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

		public async Task<Email> GetAsync(Guid id)
		{
			var @params = new SqlParameter[] {new SqlParameter("@id", id)};
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await using SqlCommand command = AdoHelper.CreateProcedureCommand("sp_get_email", connection, @params);

			return await AdoHelper.GetItemAsync(command, _mapper);
		}

		public async Task<List<Email>> GetAllAsync()
		{
			var @params = new SqlParameter[0];
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await using SqlCommand command = AdoHelper.CreateProcedureCommand("sp_get_emails", connection, @params);

			return await AdoHelper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetByTagAsync(String tag)
		{
			var @params = new SqlParameter[] {new SqlParameter("@tag", tag)};
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await using SqlCommand command =
				AdoHelper.CreateProcedureCommand("sp_get_emails_by_tag", connection, @params);

			return await AdoHelper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetBySenderAsync(String sender)
		{
			var @params = new SqlParameter[] {new SqlParameter("@sender", sender)};
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await using SqlCommand command =
				AdoHelper.CreateProcedureCommand("sp_get_emails_by_sender", connection, @params);

			return await AdoHelper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetByRecipientAsync(String recipient)
		{
			var @params = new SqlParameter[] {new SqlParameter("@recipient", recipient)};
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await using SqlCommand command =
				AdoHelper.CreateProcedureCommand("sp_get_emails_by_recipient", connection, @params);

			return await AdoHelper.GetItemsAsync(command, _mapper);
		}

		public async Task<List<Email>> GetForPeriodAsync(DateTime start, DateTime end)
		{
			if (start > end) throw new Exception("Invalid dates order.");

			var @params = new SqlParameter[]
			{
				new SqlParameter("@start_date", start.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@end_date", end.ToString("yyyy-MM-dd HH:mm:ss"))
			};

			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await using SqlCommand command =
				AdoHelper.CreateProcedureCommand("sp_get_emails_for_period", connection, @params);

			return await AdoHelper.GetItemsAsync(command, _mapper);
		}

		public async Task<Guid> AddAsync(Email email, List<String> tags)
		{
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await connection.OpenAsync();
			await using SqlTransaction transaction = connection.BeginTransaction();

			try
			{
				var tagIds = await GetTagIds(tags, transaction);

				Guid emailId = await AddEmail(email, transaction);

				await AddEmailTags(emailId, tagIds, transaction);

				await transaction.CommitAsync();

				return emailId;
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw new Exception("Errors was occured. Transaction declined.", ex);
			}
		}

		public async Task UpdateAsync(Email email, List<String> tags)
		{
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await connection.OpenAsync();
			await using SqlTransaction transaction = connection.BeginTransaction();

			try
			{
				Email old = await GetAsync(email.Id);

				await ProcessTags(email.Id, tags, JsonConvert.DeserializeObject<List<String>>(old.Tags), transaction);

				await UpdateEmailData(email, transaction);

				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw new Exception("Errors was occured. Transaction declined.", ex);
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			var @params = new SqlParameter[] {new SqlParameter("@id", id)};
			await using var connection = new SqlConnection(AdoHelper.ConnectionString);
			await using SqlCommand command = AdoHelper.CreateProcedureCommand("sp_delete_email", connection, @params);

			await AdoHelper.ExecuteAsync(command);
		}

		/// <summary>
		/// Returns tag GUIDs for a specific email as a part of insert/update transaction.
		/// </summary>
		/// <param name="tags">Email tag names collection.</param>
		/// <param name="transaction">Transaction to attach possible insert action.</param>
		private static async Task<Guid[]> GetTagIds(IReadOnlyCollection<String> tags, SqlTransaction transaction)
		{
			if (tags == null || tags.Count == 0) return new Guid[0];
			return await Task.WhenAll(tags.Select(t => GetTagId(t, transaction)));
		}

		/// <summary>
		/// For existing tag just returns its GUID.
		/// For new tag adds the one to database as a part of transaction and returns its GUID.
		/// </summary>
		/// <param name="name">Tag name.</param>
		/// <param name="transaction">Transaction to attach possible insert action.</param>
		private static async Task<Guid> GetTagId(String name, SqlTransaction transaction)
		{
			var procedure = "sp_get_tag_id_or_add";
			var param = new SqlParameter("@name", name.ToLower());
			await using SqlCommand command = AdoHelper.CreateProcedureCommand(procedure, transaction, param);

			return await AdoHelper.AddItemAsync(command);
		}

		/// <summary>
		/// Adds email entity to database as a part of transaction and returns its GUID.
		/// </summary>
		/// <param name="email">Email data transfer object.</param>
		/// <param name="transaction">Transaction to attach insert action.</param>
		private static async Task<Guid> AddEmail(Email email, SqlTransaction transaction)
		{
			var procedure = "sp_add_email";
			var @params = new SqlParameter[]
			{
				new SqlParameter("@registration_date", email.Date.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sender", email.Sender.ToLower()),
				new SqlParameter("@recipient", email.Recipient.ToLower()),
				new SqlParameter("@subject", email.Subject),
				new SqlParameter("@text", email.Text),
				new SqlParameter("@tags", email.Tags)
			};

			await using SqlCommand command = AdoHelper.CreateProcedureCommand(procedure, transaction, @params);

			return await AdoHelper.AddItemAsync(command);
		}

		/// <summary>
		/// Updates email data in database as a part of transaction.
		/// </summary>
		/// <param name="email">Email data transfer object.</param>
		/// <param name="transaction">Transaction to attach update action.</param>
		private static async Task UpdateEmailData(Email email, SqlTransaction transaction)
		{
			var procedure = "sp_update_email";
			var @params = new SqlParameter[]
			{
				new SqlParameter("@id", email.Id),
				new SqlParameter("@registration_date", email.Date.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sender", email.Sender.ToLower()),
				new SqlParameter("@recipient", email.Recipient.ToLower()),
				new SqlParameter("@subject", email.Subject),
				new SqlParameter("@text", email.Text),
				new SqlParameter("@tags", email.Tags)
			};

			await using SqlCommand command = AdoHelper.CreateProcedureCommand(procedure, transaction, @params);

			await AdoHelper.ExecuteAsync(command);
		}

		/// <summary>
		/// Adds email-tag relation for each email tag as a part of transaction.
		/// </summary>
		private static Task AddEmailTags(Guid emailId, IEnumerable<Guid> tagIds, SqlTransaction transaction)
		{
			return Task.WhenAll(tagIds.Select(id => AddEmailTag(emailId, id, transaction)));
		}

		/// <summary>
		/// Deletes email-tag relation for each email tag as a part of transaction.
		/// Deletes tag entity if there is no more email-tag relations for a specific tag id.
		/// </summary>
		private static Task DeleteEmailTags(Guid emailId, IEnumerable<Guid> tagIds, SqlTransaction transaction)
		{
			return Task.WhenAll(tagIds.Select(id => DeleteEmailTag(emailId, id, transaction)));
		}

		private static async Task<Guid> AddEmailTag(Guid emailId, Guid tagId, SqlTransaction transaction)
		{
			var procedure = "sp_add_email_tag";
			var @params = new SqlParameter[]
				{new SqlParameter("@email_id", emailId), new SqlParameter("@tag_id", tagId)};

			await using SqlCommand command = AdoHelper.CreateProcedureCommand(procedure, transaction, @params);

			return await AdoHelper.AddItemAsync(command);
		}

		private static async Task DeleteEmailTag(Guid emailId, Guid tagId, SqlTransaction transaction)
		{
			var procedure = "sp_delete_email_tag";
			var @params = new SqlParameter[]
				{new SqlParameter("@email_id", emailId), new SqlParameter("@tag_id", tagId)};

			await using SqlCommand command = AdoHelper.CreateProcedureCommand(procedure, transaction, @params);

			await AdoHelper.ExecuteAsync(command);
		}

		private static async Task ProcessTags(Guid emailId, IReadOnlyCollection<String> newTags,
			IReadOnlyCollection<String> oldTags, SqlTransaction transaction)
		{
			var oldTagSet = new HashSet<String>();
			var newTagSet = new HashSet<String>();

			foreach (String tag in oldTags) oldTagSet.Add(tag);
			foreach (String tag in newTags) newTagSet.Add(tag);

			var tagsToAdd = new List<String>(newTags.Where(t => !oldTagSet.Contains(t)));
			var tagsToDelete = new List<String>(oldTags.Where(t => !newTagSet.Contains(t)));

			var tagsToAddIds = await GetTagIds(tagsToAdd, transaction);
			var tagsToDeleteIds = await GetTagIds(tagsToDelete, transaction);

			await AddEmailTags(emailId, tagsToAddIds, transaction);
			await DeleteEmailTags(emailId, tagsToDeleteIds, transaction);
		}

		private async Task<Email> GetAsync(Guid id, SqlTransaction transaction)
		{
			var @params = new SqlParameter[] { new SqlParameter("@id", id) };
			await using SqlCommand command = AdoHelper.CreateProcedureCommand("sp_get_email", transaction, @params);

			return await AdoHelper.GetItemAsync(command, _mapper);
		}
	}
}