﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmailSaver.Data;

namespace EmailSaver.Server
{
	internal class TagSupplier : ITagSupplier
	{
		private readonly ITagData _tagData;

		public TagSupplier()
		{
			_tagData = new TagDataAdo();
		}

		public Task<List<String>> GetAllAsync()
		{
			return _tagData.GetAllAsync();
		}

		public Task AddAsync(String name)
		{
			return _tagData.AddAsync(name);
		}

		public Task DeleteAsync(String name)
		{
			return _tagData.DeleteAsync(name);
		}
	}
}