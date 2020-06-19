using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EmailSaver.Core
{
	[Serializable]
	public class Email
	{
		[JsonProperty("id")] public Guid Id { get;}
		[JsonProperty("date")] public DateTime Date { get; }
		[JsonProperty("sender")] public String Sender { get;}
		[JsonProperty("recipient")] public String Recipient { get;}
		[JsonProperty("subject")] public String Subject { get; }
		[JsonProperty("text")] public String Text { get; }
		[JsonProperty("tags")] public List<String> Tags { get; }

		[JsonConstructor]
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

		public override String ToString()
		{
			return "email:\n" +
			       $"	id : {Id}\n" +
			       $"	date : {Date}\n" +
			       $"	sender : {Sender}\n" +
			       $"	recipient : {Recipient}\n" +
			       $"	subject : {Subject}";
		}
	}
}