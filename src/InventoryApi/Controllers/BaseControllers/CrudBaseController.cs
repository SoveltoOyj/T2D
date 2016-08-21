using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Controllers.BaseControllers;
using T2D.InventoryBL.Mappers;
using Microsoft.AspNetCore.JsonPatch;

namespace InventoryApi.Controllers.TestControllers
{
	public class CrudBaseController<TEntity,TModel> : ApiBaseController
		where TEntity: class,T2D.Entities.IEntity, new()
		where TModel: class, T2D.Model.IModel
	{
		protected T2D.InventoryBL.IMapper<TEntity, TModel, long, string> _mapper;
		public CrudBaseController(T2D.InventoryBL.IMapper<TEntity, TModel, long, string> mapper):base()
		{
			_mapper = mapper;
		}


		// GET api/test/{model}
		[HttpGet]
		public IEnumerable<TModel> Get()
		{
			List<TModel> ret = new List<TModel>();
			foreach (var item in dbc.Set<TEntity>())
			{
				ret.Add(_mapper.EntityToModel(item));
			}

			return ret;
		}

		// GET api/test/{model}/{id}
		[HttpGet("{id}")]
		public TModel Get(string id)
		{
			return  _mapper.EntityToModel(dbc.Set<TEntity>().FirstOrDefault(t => t.Id == _mapper.FromModelId(id)));
		}

		// POST api/test/{model}
		// add a new Entity
		[HttpPost]
		public TModel Post([FromBody]TModel value)
		{
			var newEntity = new TEntity();
			_mapper.UpdateEntityFromModel(value, newEntity);
			dbc.Set<TEntity>().Add(newEntity);
			dbc.SaveChanges();
			return _mapper.EntityToModel(newEntity);
		}

		// Patch api/test/{model}/{id}
		//[
		//	{"op":"replace", "path":"name", "value": "a new value"}
		//]
		[HttpPatch("{id}")]
		public TModel Patch(string id, [FromBody]JsonPatchDocument<TModel> value)
		{
			long localId = _mapper.FromModelId(id);
			TEntity current = dbc.Set<TEntity>().FirstOrDefault(t => t.Id == localId);
			if (current == null) throw new Exception($"Entity {id} not Found");

			var updatedModel = _mapper.EntityToModel(current);
			value.ApplyTo(updatedModel);

			_mapper.UpdateEntityFromModel(updatedModel, current);

			dbc.SaveChanges();
			return updatedModel;
		}


		// PUT api/test/{model}/{id}
		// update whole entity
		[HttpPut("{id}")]
		public TModel Put(string id, [FromBody]TModel value)
		{
			long localId = _mapper.FromModelId(id);
			TEntity current = dbc.Set<TEntity>().FirstOrDefault(t => t.Id == localId);
			if (current == null)
				throw new Exception($"Entity {id} not Found.");

			_mapper.UpdateEntityFromModel(value, current);
			dbc.SaveChanges();
			return _mapper.EntityToModel(current);
		}

		// DELETE api/test/{model}/{id}
		[HttpDelete("{id}")]
		public void Delete(string id)
		{
			long localId = _mapper.FromModelId(id);
			TEntity t = new TEntity{ Id = localId };
			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}
	}
}
