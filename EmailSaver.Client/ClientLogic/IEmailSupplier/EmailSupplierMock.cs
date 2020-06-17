using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailSaver.Core;

namespace EmailSaver.Client.Logic
{
	internal class EmailSupplierMock : IEmailSupplier
	{
		#region NotImplemented
		
		public Task<Email> GetAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Email>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

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

		public Task<Guid> AddAsync(Email email)
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