using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EmailSaver.Core;
using Validar;

namespace EmailSaver.Client.ViewModels
{
	[InjectValidation]
	internal class AddEditEmailViewModel : BindableBase
	{
		private readonly IEmailSupplier _emailSupplier = EmailSupplierMock.Instance;
		private readonly ValidationTemplate _validation;

		public Guid Id { get; private set; }
		[Required] public DateTime Date { get; set; }
		[Required] public String Sender { get; set; }
		[Required] public String Recipient { get; set; }
		[Required] public String Subject { get; set; }
		[Required] public String Text { get; set; }
		public ObservableCollection<String> Tags { get; }

		public RelayCommand SubmitCommand { get; }
		public Boolean IsSubmitEnabled { get; private set; }

		public event Action EmailSubmited; 

		public AddEditEmailViewModel()
		{
			Tags = new ObservableCollection<String>();
			SubmitCommand = new RelayCommand(Submit);

			_validation = new ValidationTemplate(this);

			_validation.ErrorsChanged += (sender, args) => IsSubmitEnabled = !_validation.HasErrors;
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
			Sender = default;
			Recipient = default;
			Subject = default;
			Text = default;
			Tags.Clear();
		}

		private async void Submit(Object obj)
		{
			var email = new Email(Id, Date, Sender, Recipient, Subject, Text, Tags.ToList());

			if (Id == Guid.Empty)
			{
				Id = await _emailSupplier.AddAsync(email);
			}
			else
			{
				await _emailSupplier.UpdateAsync(email);
			}

			EmailSubmited?.Invoke();
		}
	}
}