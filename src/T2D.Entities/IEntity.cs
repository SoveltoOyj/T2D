using System;

namespace T2D.Entities
{
	public interface IBaseEntity
	{
	}

	// Can't use complex types
	//public struct ThingId
	//{
	//	Uri CreatorUri { get; set; }
	//	string UniqueString { get; set; }
	//}

	public interface IThingEntity:IBaseEntity
	{
		//string Id { get; set; }
		string Id_CreatorFQDN { get; set;}

		string Id_UniqueString { get; set; } 

		string Title { get; set; }
	}
	public interface IEntity : IBaseEntity
	{
		string Id { get; set; }
	}

	public interface IEnumEntity:IEntity
	{
		string Name { get; set; }
	}
}