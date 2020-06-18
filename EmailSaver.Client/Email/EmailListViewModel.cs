using System;
using System.Collections.ObjectModel;
using EmailSaver.Client.Logic;
using EmailSaver.Core;

namespace EmailSaver.Client.ViewModels
{
	internal class EmailListViewModel : BindableBase
	{
		private readonly IEmailSupplier _emailSupplier;


		public String Text { get; set; }

		public ObservableCollection<EmailObservable> Emails { get; }
		public EmailObservable SelectedEmail { get; set; }
		
		public RelayCommand OpenEmailCommand { get; }

		public EmailListViewModel()
		{
			_emailSupplier = new EmailSupplierHttp();

			Text = "EmailList";

			Emails = new ObservableCollection<EmailObservable>();
			OpenEmailCommand = new RelayCommand(OpenEmail);
		}

		private async void GetAllEmails(Object parameter)
		{
			var emails = await _emailSupplier.GetAllAsync();

			Emails.Clear();
			foreach (Email email in emails) Emails.Add(new EmailObservable(email));
		}

		private void OpenEmail(Object parameter)
		{

		}
	}
}