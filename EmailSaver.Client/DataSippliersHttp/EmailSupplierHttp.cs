using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailSaver.Core;

namespace EmailSaver.Client
{
	internal class EmailSupplierHttp : IEmailSupplier
	{
		public Task<Email> GetAsync(Guid id)
		{
			return HttpHelper.GetAsync<Email>($"/api/emails/id?value={id}");
		}

		public Task<List<Email>> GetAllAsync()
		{
			return HttpHelper.GetAsync<List<Email>>("/api/emails");
		}

		public Task<Guid> AddAsync(Email email)
		{
			return HttpHelper.PostAsync(email, "/api/emails");
		}
		
		public Task UpdateAsync(Email email)
		{
			return HttpHelper.PutAsync(email, "/api/emails");
		}
		
		#region NotImplemented

		public Task<List<Email>> GetByTagAsync(String tag)
		{
			throw new NotImplementedException();
		}

		public Task<List<Email>> GetBySenderAsync(String sender)
		{
			throw new NotImplementedException();
		}

		public Task<List<Email>> GetByRecipientAsync(String recipient)
		{
			throw new NotImplementedException();
		}

		public Task<List<Email>> GetForPeriodAsync(DateTime start, DateTime end)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
