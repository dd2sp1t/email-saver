using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailSaver.Data;

namespace EmailSaver.Server
{
	internal class TagSupplier : ITagSupplier
	{
		private readonly ITagDataPublic _tagData;

		public TagSupplier()
		{
			_tagData = new TagDataAdo();
		}

		public Task<List<String>> GetAllAsync()
		{
			return _tagData.GetAllAsync();
		}

		public Task<Boolean> AddAsync(String name)
		{
			return _tagData.AddAsync(name);
		}

		public Task<Boolean> RenameAsync(String oldName, String newName)
		{
			return _tagData.RenameAsync(oldName, newName);
		}

		public Task<Boolean> DeleteAsync(String name)
		{
			return _tagData.DeleteAsync(name);
		}
	}
}