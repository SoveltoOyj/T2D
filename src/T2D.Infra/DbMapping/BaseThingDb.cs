using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public static class BaseThingDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BaseThing>()
					.HasKey(t => new { t.Id_CreatorUri, t.Id_UniqueString })
					;

		}
	}
}
