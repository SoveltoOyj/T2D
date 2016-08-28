using T2D.Entities;
using T2D.Model;

namespace T2D.InventoryBL
{
	public interface IMapper<TEntity, TModel, TKeyEntity, TKeyModel>
		where TEntity : class, T2D.Entities.IBaseEntity
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
	public interface IThingMapper<TEntity, TModel, TKey>:IMapper<TEntity, TModel, TKey, TKey>
		where TEntity : class, T2D.Entities.IBaseEntity
		where TModel : class, T2D.Model.IBaseModel
	{
		TEntity UpdateEntityFromModel(TModel from, TEntity to, bool updateAlsoId);
	}
}