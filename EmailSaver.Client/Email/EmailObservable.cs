using System;
using System.Collections.Generic;
using EmailSaver.Core;

namespace EmailSaver.Client
{
	internal class EmailObservable : BindableBase
	{
		public Guid Id { get; set; }
		public DateTime Date { get; set; }
		public String Sender { get; set; }
		public String Recipient { get; set; }
		public String Subject { get; set; }
		public String Text { get; set; }
		public List<String> Tags { get; set; }

		public EmailObservable(Guid id, DateTime date, String sender, String recipient, String subject, String text,
			List<String> tags)
		{
			Id = id;
			Date = date;
			Sender = sender;
			Recipient = recipient;
			Subject = subject;
			Text = text;
			Tags = tags;
		}

		public EmailObservable(Email email)
			: this(email.Id, email.Date, email.Sender, email.Recipient, email.Subject, email.Text, email.Tags)
		{
		}
	}
}