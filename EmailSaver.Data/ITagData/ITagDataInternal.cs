using System;
using System.Threading.Tasks;

namespace EmailSaver.Data
{
	internal interface ITagDataInternal
	{
		Task<Guid> AddAsync(String name);
		Task<Guid> GetIdAsync(String name);
	}
}