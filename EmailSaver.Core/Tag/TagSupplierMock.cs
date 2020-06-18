using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Core
{
	public class TagSupplierMock : ITagSupplier
	{
		#region NotImplemented

		public Task<List<String>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task AddAsync(String name)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(String name)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}