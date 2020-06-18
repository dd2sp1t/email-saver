using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailSaver.Core;

namespace EmailSaver.Server
{
	internal class EmailSupplierMock : IEmailSupplier
	{
		private readonly List<Email> _emails;

		public EmailSupplierMock()
		{
			var ids = new List<Guid>()
			{
				new Guid("00000000-0000-0000-0000-000000000000"),
				new Guid("00000000-0000-0000-0000-000000000001"),
				new Guid("00000000-0000-0000-0000-000000000002"),
				new Guid("00000000-0000-0000-0000-000000000003"),
				new Guid("00000000-0000-0000-0000-000000000004"),
				new Guid("00000000-0000-0000-0000-000000000005"),
			};

			var tags = new List<String>();

			_emails = new List<Email>
			{
				new Email(ids[0], DateTime.UtcNow, "Hogwarts", "HP", "Hello!", "You're a wizard, Harry!", tags),
				new Email(ids[1], DateTime.UtcNow.AddDays(1), "HP", "Hogwarts", "RE: Hello", "Cool!", tags),
				new Email(ids[2], DateTime.UtcNow.AddDays(1), "HP", "AK", "LOL", "I'm wizard!", tags),
				new Email(ids[3], DateTime.UtcNow.AddDays(1), "AK", "HP", "RE: LOL", "Wow! Can't wait anymore. I'll ask them right now!", tags),
				new Email(ids[4], DateTime.UtcNow.AddDays(1), "AK", "Hogwarts", "Very Important Question", "Hello! Am I wizard too?", tags),
				new Email(ids[5], DateTime.UtcNow.AddDays(2), "Hogwarts", "AK", "RE: Very Important Question", "Sorry, you're not.", tags)
			};
		}

		public Task<Email> GetAsync(Guid id)
		{
			return Task.FromResult(_emails.Single(m => m.Id == id));
		}

		public Task<List<Email>> GetAllAsync()
		{
			return Task.FromResult(_emails);
		}

		public Task<List<Email>> GetBySenderAsync(String sender)
		{
			return Task.FromResult(_emails.Where(m => m.Sender == sender).ToList());
		}

		public Task<List<Email>> GetByRecipientAsync(String recipient)
		{
			return Task.FromResult(_emails.Where(m => m.Recipient == recipient).ToList());
		}

		#region NotImplemented

		public Task<List<Email>> GetByTagAsync(String tag)
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