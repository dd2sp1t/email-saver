using System;

namespace EmailSaver.Client.ViewModels
{
	internal class MainWindowViewModel : BindableBase
	{
		private readonly EmailListViewModel _emailListViewModel = new EmailListViewModel();
		private readonly AddEditEmailViewModel _addEditEmailViewModel = new AddEditEmailViewModel();

		public BindableBase CurrentViewModel { get; private set; }
		public RelayCommand NavigationCommand { get; }

		public MainWindowViewModel()
		{
			NavigationCommand = new RelayCommand(Navigate);
		}

		private void Navigate(Object parameter)
		{
			if (!(parameter is String target)) throw new ArgumentException();

			CurrentViewModel = target switch
			{
				"emails" => _emailListViewModel,
				"add-edit" => _addEditEmailViewModel,
				_ => CurrentViewModel
			};
		}
	}
}