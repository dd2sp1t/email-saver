using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Core
{
	public interface ITagSupplier
	{
		Task<List<String>> GetAllAsync();
		Task AddAsync(String name);
		Task DeleteAsync(String name);
	}
}