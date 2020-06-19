using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Core
{
	public class EmailSupplierMock : IEmailSupplier
	{
		private readonly List<Email> _emails;
		private readonly Random _random;

		public EmailSupplierMock()
		{
			var ids = new List<Guid>()
			{
				new Guid("29af5cd1-8f7e-4df6-9add-64946c2795f0"),
				new Guid("edea4e92-9682-4b45-8301-8f407403357b"),
				new Guid("4541bfd0-060e-453c-b46a-b987f63f0fd0"),
				new Guid("0dee2177-ab72-41a5-8bad-7ec97d705372"),
				new Guid("a0ab3ed8-cb36-4480-8520-a13d9906cad6"),
				new Guid("b8167db3-c458-4cb4-8423-c8d76b64d070"),
			};

			var tags = new List<String> {"inbox"};

			_emails = new List<Email>
			{
				new Email(ids[0], DateTime.UtcNow, "Hogwarts", "HP", "Hello!", "You're a wizard, Harry!", tags),

				new Email(ids[1], DateTime.UtcNow.AddDays(1), "HP", "Hogwarts", "RE: Hello", "Cool!", tags),

				new Email(ids[2], DateTime.UtcNow.AddDays(1), "HP", "AK", "LOL", "I'm wizard!", tags),

				new Email(ids[3], DateTime.UtcNow.AddDays(1),
					"AK", "HP", "RE: LOL", "Wow! Can't wait anymore. I'll ask them right now!", tags),

				new Email(ids[4], DateTime.UtcNow.AddDays(1),
					"AK", "Hogwarts", "Very Important Question", "Hello! Am I wizard too?", tags),

				new Email(ids[5], DateTime.UtcNow.AddDays(2),
					"Hogwarts", "AK", "RE: Very Important Question", "Sorry, you're not.", tags)
			};

			_random = new Random();
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

		public Task<Guid> AddAsync(Email email)
		{
			var bytes = new Byte[16];
			_random.NextBytes(bytes);

			var id = new Guid(bytes);
			_emails.Add(new Email(id, email.Date, email.Sender, email.Recipient, email.Subject, email.Text,
				email.Tags));

			return Task.FromResult(id);
		}

		public Task UpdateAsync(Email email)
		{
			Email old = _emails.SingleOrDefault(e => e.Id == email.Id);

			if (old == null) throw new Exception($"Email with id = {email.Id} not found.");

			_emails.Remove(old);
			_emails.Add(email);

			// todo: update tag mock set

			return Task.CompletedTask;
		}

		public Task DeleteAsync(Guid id)
		{
			Email old = _emails.SingleOrDefault(e => e.Id == id);

			if (old == null) throw new Exception($"Email with id = {id} not found.");

			_emails.Remove(old);

			return Task.CompletedTask;
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

		#endregion
	}
}