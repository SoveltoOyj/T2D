using System;

namespace T2D.Entities
{
	public interface IEntity
	{
		Guid Id { get; set; }
	}
		public interface IThing:IEntity
	{
		string Fqdn { get; set; }
		string US { get; set; }
		string Title { get; set; }
	}

	public interface IEnumEntity
	{
		int Id { get; set; }
		string Name { get; set; }
	}
}