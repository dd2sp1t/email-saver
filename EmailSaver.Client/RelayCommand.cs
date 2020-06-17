using System;
using System.Windows.Input;

namespace EmailSaver.Client
{
	internal class RelayCommand : ICommand
	{
		private readonly Action<Object> _execute;


		public RelayCommand(Action<Object> execute)
		{
			_execute = execute;
		}

		public Boolean CanExecute(Object parameter)
		{
			return _execute != null;
		}

		public void Execute(Object parameter)
		{
			_execute(parameter);
		}

		public event EventHandler CanExecuteChanged;
	}
}