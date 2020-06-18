using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmailSaver.Data
{
	public interface ITagData
	{
		Task<List<String>> GetAllAsync();
		Task<Guid> AddAsync(String name);
		Task DeleteAsync(String name);
	}
}