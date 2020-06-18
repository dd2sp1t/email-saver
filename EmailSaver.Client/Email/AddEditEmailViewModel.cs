using System;

namespace EmailSaver.Client.ViewModels
{
	internal class AddEditEmailViewModel : BindableBase
	{
		public String Text { get; set; }

		public AddEditEmailViewModel()
		{
			Text = "AddEditEmail";
		}
	}
}