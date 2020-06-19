using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using EmailSaver.Core;

namespace EmailSaver.Client.ViewModels
{
	internal class EmailListViewModel : BindableBase
	{
		private readonly IEmailSupplier _emailSupplier = EmailSupplierMock.Instance;

		public ObservableCollection<EmailObservable> Emails { get; }
		public EmailObservable SelectedEmail { get; set; }

		public event Action<Guid> OpenEmailClicked;

		public RelayCommand OpenEmailCommand { get; }

		public EmailListViewModel()
		{
			//_emailSupplier = new EmailSupplierHttp();
			OpenEmailClicked += emailId => { };

			var emails = _emailSupplier.GetAllAsync().Result.Select(e => new EmailObservable(e));

			Emails = new ObservableCollection<EmailObservable>(emails);

			OpenEmailCommand = new RelayCommand(OpenEmail);
		}

		public async void GetAllEmailsClickedHandler()
		{
			var emails = await _emailSupplier.GetAllAsync();

			Emails.Clear();
			foreach (Email email in emails) Emails.Add(new EmailObservable(email));
		}

		private void OpenEmail(Object parameter)
		{
			OpenEmailClicked?.Invoke(SelectedEmail.Id);
		}
	}
}