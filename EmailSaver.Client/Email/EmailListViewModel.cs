using System;
using System.Collections.ObjectModel;
using EmailSaver.Client.Logic;
using EmailSaver.Core;

namespace EmailSaver.Client.ViewModels
{
	internal class EmailListViewModel
	{
		private readonly IEmailSupplier _emailSupplier;

		public ObservableCollection<EmailObservable> Emails { get; }
		public RelayCommand GetAllEmailsCommand { get; }
		public RelayCommand AddEmailCommand { get; }
		public RelayCommand OpenEmailCommand { get; }

		public EmailListViewModel()
		{
			Emails = new ObservableCollection<EmailObservable>();
			_emailSupplier = new EmailSupplierHttp();

			GetAllEmailsCommand = new RelayCommand(GetAllEmails);
			AddEmailCommand = new RelayCommand(AddEmail);
		}

		private async void GetAllEmails(Object parameter)
		{
			var emails = await _emailSupplier.GetAllAsync();
			
			Emails.Clear();
			foreach (Email email in emails) Emails.Add(new EmailObservable(email));
		}

		private async void AddEmail(Object parameter)
		{
			// stub
			var email = new Email(Guid.Empty, DateTime.UtcNow, "Test", "Test", "Test", "Test", null);

			await _emailSupplier.AddAsync(email);

			Emails.Add(new EmailObservable(email));
		}
	}
}