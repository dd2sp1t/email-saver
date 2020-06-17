using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailSaver.Core;

namespace EmailSaver.Server
{
	public interface IEmailSupplier
	{
		Task<Email> GetAsync(Guid id);
		Task<List<Email>> GetAllAsync();
		Task<List<Email>> GetByTagAsync(String tag);
		Task<List<Email>> GetBySenderAsync(String sender);
		Task<List<Email>> GetByRecipientAsync(String recipient);
		Task<List<Email>> GetForPeriodAsync(DateTime start, DateTime end);

		Task<Guid> AddAsync(Email email);
		Task<Boolean> UpdateAsync(Email email);
		Task<Boolean> DeleteAsync(Guid id);
	}
}