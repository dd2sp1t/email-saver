using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EmailSaver.Core
{
	[Serializable]
	public class Email
	{
		[JsonProperty("id")] public Guid Id { get; set; }
		[JsonProperty("date")] public DateTime Date { get; set; }
		[JsonProperty("sender")] public String Sender { get; set; }
		[JsonProperty("recipient")] public String Recipient { get; set; }
		[JsonProperty("subject")] public String Subject { get; set; }
		[JsonProperty("text")] public String Text { get; set; }
		[JsonProperty("tags")] public List<String> Tags { get; set; }

		public Email(Guid id, DateTime date, String sender, String recipient, String subject, String text,
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

		public Email()
		{
		}
	}
}