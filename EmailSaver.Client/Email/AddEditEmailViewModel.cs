using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using EmailSaver.Core;

namespace EmailSaver.Client.ViewModels
{
	internal class AddEditEmailViewModel : BindableBase
	{
		private readonly IEmailSupplier _emailSupplier = EmailSupplierMock.Instance;

		public Guid Id { get; private set; }
		public DateTime Date { get; set; }
		public String Sender { get; set; }
		public String Recipient { get; set; }
		public String Subject { get; set; }
		public String Text { get; set; }
		public ObservableCollection<String> Tags { get; }

		public RelayCommand SubmitCommand { get; }

		public AddEditEmailViewModel()
		{
			Tags = new ObservableCollection<String>();
			SubmitCommand = new RelayCommand(Submit);
		}

		public async Task FillEmailData(Guid id)
		{
			Clear();

			if (id == Guid.Empty) return;

			Email email = await _emailSupplier.GetAsync(id);

			Id = id;
			Date = email.Date;
			Sender = email.Sender;
			Recipient = email.Recipient;
			Subject = email.Subject;
			Text = email.Text;

			foreach (String tag in email.Tags) Tags.Add(tag);
		}

		private void Clear()
		{
			Id = default;
			Date = default;
			Sender = "";
			Recipient = "";
			Subject = "";
			Text = "";
			Tags.Clear();
		}

		private async void Submit(Object obj)
		{
			var email = new Email(Id, Date, Sender, Recipient, Subject, Text, Tags.ToList());

			if (Id == Guid.Empty)
			{
				Id = await _emailSupplier.AddAsync(email);
				return;
			}

			await _emailSupplier.UpdateAsync(email);
		}
	}
}