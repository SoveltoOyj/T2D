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
//		ThingId Id { get; set; }
		string Id_CreatorUri { get; set;}

		string Id_UniqueString { get; set; } 
	}
	public interface IEntity : IBaseEntity
	{
		long Id { get; set; }
	}

	public interface IEnumEntity:IEntity
	{
		string Name { get; set; }
	}
}