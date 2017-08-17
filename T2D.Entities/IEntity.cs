using System;

namespace T2D.Entities
{
	public interface IEntity
	{
		Guid Id { get; set; }
	}
	public interface IThing : IEntity
	{
		string Fqdn { get; set; }
		string US { get; set; }
	}
	public interface IInventoryThing : IThing
	{
		string Title { get; set; }
	}

	public interface IEnumEntity
	{
		int Id { get; set; }
		string Name { get; set; }
	}

	//entities that have Modified and Created properties
	// these are automatically set by infra
	public interface IAuditableEntity
	{
		DateTime Created { get; set; }
		DateTime Modified { get; set; }
	}
}