using System.ComponentModel;

namespace EmailSaver.Client
{
	internal class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
	}
}