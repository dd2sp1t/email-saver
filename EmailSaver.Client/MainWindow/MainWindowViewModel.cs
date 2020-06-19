using System;

namespace EmailSaver.Client.ViewModels
{
	internal class MainWindowViewModel : BindableBase
	{
		public BindableBase CurrentViewModel { get; private set; }

		public EmailListViewModel EmailListViewModel { get; }
		public AddEditEmailViewModel AddEditEmailViewModel { get; }

		public RelayCommand NavigationCommand { get; }

		private event Action GetAllEmailsClicked;

		public MainWindowViewModel()
		{
			EmailListViewModel = new EmailListViewModel();
			AddEditEmailViewModel = new AddEditEmailViewModel();

			NavigationCommand = new RelayCommand(Navigate);

			GetAllEmailsClicked += EmailListViewModel.GetAllEmailsClickedHandler;
			EmailListViewModel.OpenEmailClicked += OnOpenEmailClicked;
		}

		private async void OnOpenEmailClicked(Guid id)
		{
			await AddEditEmailViewModel.FillEmailData(id);
			CurrentViewModel = AddEditEmailViewModel;
		}

		private async void Navigate(Object parameter)
		{
			if (!(parameter is String target)) throw new ArgumentException();

			switch (target)
			{
				case "emails":
				{
					GetAllEmailsClicked?.Invoke();
					CurrentViewModel = EmailListViewModel;
				}
					break;
				case "add-edit":
				{
					await AddEditEmailViewModel.FillEmailData(Guid.Empty);
					CurrentViewModel = AddEditEmailViewModel;
				}
					break;
				default:
					CurrentViewModel = CurrentViewModel;
					break;
			}
		}
	}
}