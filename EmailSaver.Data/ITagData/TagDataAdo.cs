using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Data
{
	public class TagDataAdo : ITagData
	{
		#region NotImplemented

		public Task<List<String>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Guid> AddAsync(String name)
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