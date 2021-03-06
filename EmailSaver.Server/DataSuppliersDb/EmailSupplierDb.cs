﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using EmailSaver.Core;
using EmailSaver.Data;


namespace EmailSaver.Server
{
	internal class EmailSupplierDb : IEmailSupplier
	{
		private readonly IEmailData _emailData;

		public EmailSupplierDb()
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

		public async Task<List<Core.Email>> GetForPeriodAsync(DateTime start, DateTime end)
		{
			return (await _emailData.GetForPeriodAsync(start, end)).Select(ConvertToCoreEmail).ToList();
		}

		public Task<Guid> AddAsync(Core.Email email)
		{
			String serialized = JsonConvert.SerializeObject(email.Tags);

			var dto = new Data.Email(Guid.Empty, email.Date, email.Sender.ToLower(), email.Recipient.ToLower(),
				email.Subject, email.Text, serialized);

			return _emailData.AddAsync(dto, email.Tags);
		}

		public Task UpdateAsync(Core.Email email)
		{
			String serialized = JsonConvert.SerializeObject(email.Tags);

			var dto = new Data.Email(email.Id, email.Date, email.Sender.ToLower(), email.Recipient.ToLower(),
				email.Subject, email.Text, serialized);

			return _emailData.UpdateAsync(dto, email.Tags);
		}

		public Task DeleteAsync(Guid id)
		{
			return _emailData.DeleteAsync(id);
		}

		private static Core.Email ConvertToCoreEmail(Data.Email dto)
		{
			var tags = JsonConvert.DeserializeObject<List<String>>(dto.Tags);
			return new Core.Email(dto.Id, dto.Date, dto.Sender, dto.Recipient, dto.Subject, dto.Text, tags);
		}
	}
}
