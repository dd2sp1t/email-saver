using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EmailSaver.Core;
using Validar;

namespace EmailSaver.Client.ViewModels
{
	[InjectValidation]
	internal class AddEditEmailViewModel : BindableBase
	{
		private readonly StringBuilder _builder;
		private readonly IEmailSupplier _emailSupplier;
		private readonly ValidationTemplate _validation;

		public Guid Id { get; private set; }
		[Required] public DateTime Date { get; set; }
		[Required] [MaxLength(100)] public String Sender { get; set; }
		[Required] [MaxLength(100)] public String Recipient { get; set; }
		[Required] [MaxLength(100)] public String Subject { get; set; }
		[Required] [MaxLength(2000)] public String Text { get; set; }
		[MaxLength(2000)] public String Tags { get; set; }

		public RelayCommand SubmitCommand { get; }
		public Boolean IsSubmitEnabled { get; private set; }

		public event Action EmailSubmited;

		public AddEditEmailViewModel(IEmailSupplier emailSupplier)
		{
			_builder = new StringBuilder();
			_emailSupplier = emailSupplier;
			_validation = new ValidationTemplate(this);
			_validation.ErrorsChanged += (sender, args) => IsSubmitEnabled = !_validation.HasErrors;

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

			foreach (String tag in email.Tags) _builder.Append(tag + " ");

			Tags = _builder.ToString();

			_builder.Clear();
		}

		private void Clear()
		{
			Id = default;
			Date = default;
			Sender = default;
			Recipient = default;
			Subject = default;
			Text = default;
			Tags = default;
		}

		private async void Submit(Object obj)
		{
			var email = new Email(Id, Date, Sender, Recipient, Subject, Text,
				Tags.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());

			if (Id == Guid.Empty)
				Id = await _emailSupplier.AddAsync(email);
			else
				await _emailSupplier.UpdateAsync(email);

			EmailSubmited?.Invoke();
		}
	}
}