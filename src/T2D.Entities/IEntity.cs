using System;

namespace T2D.Entities
{
	public interface IEntity
	{
	}

	public interface IThingEntity:IEntity
	{
		Guid Id { get; set; }
		string Title { get; set; }
	}

	public interface IEnumEntity:IEntity
	{
		int Id { get; set; }
		string Name { get; set; }
	}
}