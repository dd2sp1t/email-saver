using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Server
{
	internal class TagSupplierMock : ITagSupplier
	{
		#region NotImplemented

		public Task<List<String>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Boolean> AddAsync(String name)
		{
			throw new NotImplementedException();
		}

		public Task<Boolean> RenameAsync(String oldName, String newName)
		{
			throw new NotImplementedException();
		}

		public Task<Boolean> DeleteAsync(String name)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}