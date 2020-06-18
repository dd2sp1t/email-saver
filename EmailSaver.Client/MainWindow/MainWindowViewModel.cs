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

		private void OnOpenEmailClicked(Guid obj)
		{
			AddEditEmailViewModel.EmailId = obj;
			CurrentViewModel = AddEditEmailViewModel;
		}

		private void Navigate(Object parameter)
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
					CurrentViewModel = AddEditEmailViewModel;
					break;
				default:
					CurrentViewModel = CurrentViewModel;
					break;
			}

			AddEditEmailViewModel.EmailId = Guid.Empty;
		}
	}
}