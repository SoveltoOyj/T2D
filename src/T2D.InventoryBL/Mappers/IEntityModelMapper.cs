using System;
using T2D.Entities;
using T2D.Model;

namespace T2D.InventoryBL
{
	public interface IEntityModelMapper<TEntity, TModel>
		where TEntity : class, T2D.Entities.IEntity
		where TModel : class, T2D.Model.IModel
	{
		TModel EntityToModel(TEntity from);
		TEntity ModelToEntity(TModel from);
		TEntity UpdateEntityFromModel(TModel from, TEntity to);
	}

}