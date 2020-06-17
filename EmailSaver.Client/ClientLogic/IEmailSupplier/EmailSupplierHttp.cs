using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailSaver.Core;

namespace EmailSaver.Client.Logic
{
	public class EmailSupplierHttp : IEmailSupplier
	{
		private readonly HttpHelper _helper = HttpHelper.Instance;

		public Task<Email> GetAsync(Guid id)
		{
			return _helper.GetAsync<Email>($"/api/emails/id?value={id}");
		}

		public Task<List<Email>> GetAllAsync()
		{
			return _helper.GetAsync<List<Email>>("/api/emails");
		}

		public Task<Guid> AddAsync(Email email)
		{
			return _helper.PostAsync(email, "/api/emails");
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

		public Task<Boolean> UpdateAsync(Email email)
		{
			throw new NotImplementedException();
		}

		public Task<Boolean> DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}