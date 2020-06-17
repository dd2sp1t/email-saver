using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using EmailSaver.Data;

namespace EmailSaver.Server
{
	internal class EmailSupplier : IEmailSupplier
	{
		private readonly IEmailData _emailData;

		public EmailSupplier()
		{
			_emailData = new EmailDataAdo();
		}

		public async Task<Core.Email> GetAsync(Guid id)
		{
			return ConvertToCoreEmail(await _emailData.GetAsync(id));
		}

		public async Task<List<Core.Email>> GetAllAsync()
		{
			return (await _emailData.GetAllAsync()).Select(ConvertToCoreEmail).ToList();
		}

		public async Task<List<Core.Email>> GetByTagAsync(String tag)
		{
			return (await _emailData.GetByTagAsync(tag)).Select(ConvertToCoreEmail).ToList();
		}

		public async Task<List<Core.Email>> GetBySenderAsync(String sender)
		{
			return (await _emailData.GetBySenderAsync(sender)).Select(ConvertToCoreEmail).ToList();
		}

		public async Task<List<Core.Email>> GetByRecipientAsync(String recipient)
		{
			return (await _emailData.GetByRecipientAsync(recipient)).Select(ConvertToCoreEmail).ToList();
		}

		public async Task<List<Core.Email>> GetForPeriodAsync(DateTime start_date, DateTime end_date)
		{
			return (await _emailData.GetForPeriodAsync(start_date, end_date)).Select(ConvertToCoreEmail).ToList();
		}

		public async Task<Guid> AddAsync(Core.Email email)
		{
			var dto = new Data.Email(Guid.Empty, email.Date, email.Sender.ToLower(), email.Recipient.ToLower(),
				email.Subject, email.Text, null);

			return await _emailData.AddAsync(dto, email.Tags);
		}

		public async Task<Boolean> UpdateAsync(Core.Email email)
		{
			var dto = new Data.Email(email.Id, email.Date, email.Sender.ToLower(), email.Recipient.ToLower(),
				email.Subject, email.Text, null);

			return await _emailData.UpdateAsync(dto, email.Tags);
		}

		public async Task<Boolean> DeleteAsync(Guid id)
		{
			return await _emailData.DeleteAsync(id);
		}

		private static Core.Email ConvertToCoreEmail(Email dto)
		{
			var tags = JsonConvert.DeserializeObject<List<String>>(dto.Tags);
			return new Core.Email(dto.Id, dto.Date, dto.Sender, dto.Recipient, dto.Subject, dto.Text, tags);
		}
	}
}