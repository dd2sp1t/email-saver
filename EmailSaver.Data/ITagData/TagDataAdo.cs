using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Data
{
	public class TagDataAdo : ITagDataPublic, ITagDataInternal
	{
		#region NotImplemented

		#region ITagDataPublic

		Task<List<String>> ITagDataPublic.GetAllAsync()
		{
			throw new NotImplementedException();
		}

		Task<Boolean> ITagDataPublic.AddAsync(String name)
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

		#region ITagDataInternal

		Task<Guid> ITagDataInternal.GetIdAsync(String name)
		{
			throw new NotImplementedException();
		}

		Task<Guid> ITagDataInternal.AddAsync(String name)
		{
			throw new NotImplementedException();
		}

		#endregion

		#endregion
	}
}