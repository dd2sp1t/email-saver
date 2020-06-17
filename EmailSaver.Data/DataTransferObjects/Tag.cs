using System;

namespace EmailSaver.Data
{
	internal class Tag
	{
		public Guid Id { get; }
		public String Name { get; }

		public Tag(Guid id, String name)
		{
			Id = id;
			Name = name;
		}
	}
}