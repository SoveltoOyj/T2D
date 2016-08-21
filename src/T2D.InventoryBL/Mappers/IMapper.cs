using T2D.Entities;
using T2D.Model;

namespace T2D.InventoryBL
{
	public interface IMapper<TEntity, TModel, TKeyEntity, TKeyModel>
		where TEntity : class, T2D.Entities.IEntity
		where TModel : class, T2D.Model.IModel
	{
		TModel EntityToModel(TEntity from);
		TKeyModel FromEntityId(TKeyEntity id);
		TKeyEntity FromModelId(TKeyModel id);
		TEntity ModelToEntity(TModel from);
		TEntity UpdateEntityFromModel(TModel from, TEntity to);
	}
}