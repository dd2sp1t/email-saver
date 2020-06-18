using System;

namespace EmailSaver.Client.ViewModels
{
	internal class AddEditEmailViewModel : BindableBase
	{
		public Guid EmailId { get; set; }

		public AddEditEmailViewModel()
		{
		}
	}
}