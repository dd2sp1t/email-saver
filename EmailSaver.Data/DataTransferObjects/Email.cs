using System;

namespace EmailSaver.Data
{
	public class Email
	{
		public Guid Id { get; }
		public DateTime Date { get; }
		public String Sender { get; }
		public String Recipient { get; }
		public String Subject { get; }
		public String Text { get; }
		/// <summary>
		/// Serialized list of strings.
		/// </summary>
		public String Tags { get; }

		/// <param name="id"></param>
		/// <param name="date"></param>
		/// <param name="sender"></param>
		/// <param name="recipient"></param>
		/// <param name="subject"></param>
		/// <param name="text"></param>
		/// <param name="tags">Serialized list of strings.</param>
		public Email(Guid id, DateTime date, String sender, String recipient, String subject, String text, String tags)
		{
			Id = id;
			Date = date;
			Sender = sender;
			Recipient = recipient;
			Subject = subject;
			Text = text;
			Tags = tags;
		}
	}
}