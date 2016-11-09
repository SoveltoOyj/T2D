using System;
using T2D.Entities;
using T2D.Model;

namespace T2D.InventoryBL
{
	public interface IMapper<TEntity, TModel, TKeyEntity, TKeyModel>
		where TEntity : class, T2D.Entities.IEntity
		where TModel : class, T2D.Model.IBaseModel
	{
		TModel EntityToModel(TEntity from);
		TKeyModel FromEntityId(TKeyEntity id);
		TKeyEntity FromModelId(TKeyModel id);
		TEntity ModelToEntity(TModel from);
		TEntity UpdateEntityFromModel(TModel from, TEntity to);
	}

	/// <summary>
	/// ThingMapper uses TModel key also for Entity and UpdateEntityModel-method can update also Id
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TModel"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public interface IThingMapper<TEntity, TModel>:IMapper<TEntity, TModel, string, string>
		where TEntity : class, T2D.Entities.IThingEntity
		where TModel : class, T2D.Model.IBaseModel
	{
		TEntity UpdateEntityFromModel(TModel from, TEntity to, bool updateAlsoId);
	}

	/// <summary>
	/// Mapper from/to enum in Rest APi (using strings) and Entity, where enums are in the database
	/// </summary>
	/// <typeparam name="TEnum">Enum type</typeparam>
	/// <typeparam name="TEntity">Entity Type, must be EnumEntity type</typeparam>
	public interface IEnumMapper<TEnum, TEntity>
		where TEnum: struct, IConvertible
		where TEntity : class, T2D.Entities.IEnumEntity
	{
		TEnum EntityToEnum(TEntity from);
		TEnum FromEntityId(int id);
		TEntity EnumToEntity(TEnum from);
	}
}