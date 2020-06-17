using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Server
{
	public interface ITagSupplier
	{
		Task<List<String>> GetAllAsync();
		Task<Boolean> AddAsync(String name);
		Task<Boolean> RenameAsync(String oldName, String newName);
		Task<Boolean> DeleteAsync(String name);
	}
}