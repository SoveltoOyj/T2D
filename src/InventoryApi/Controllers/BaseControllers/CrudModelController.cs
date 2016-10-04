using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Controllers.BaseControllers;
using T2D.InventoryBL.Mappers;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Http;
using System.Net;

namespace InventoryApi.Controllers.BaseControllers
{
	/// <summary>
	/// This crud-controller requires Model with string ID and Entity with long Id
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TModel"></typeparam>
	public class CrudBaseController<TEntity,TModel> : ApiBaseController
		where TEntity: class,T2D.Entities.IEntity, new()
		where TModel: class, T2D.Model.IModel
	{

		protected T2D.InventoryBL.IMapper<TEntity, TModel, long, long> _mapper;
		protected bool _onlyGet;

		public CrudBaseController( T2D.InventoryBL.IMapper<TEntity, TModel, long, long> mapper, bool onlyGet = true) :base()
		{
			_mapper = mapper;
			_onlyGet = onlyGet;
		}


		// GET api/test/{model}
		[HttpGet]
		public virtual IEnumerable<TModel> Get()
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
		public virtual TModel Get(long id)
		{
			return  _mapper.EntityToModel(dbc.Set<TEntity>().FirstOrDefault(t => t.Id == _mapper.FromModelId(id)));
		}

		// POST api/test/{model}
		// add a new Entity
		[HttpPost]
		public virtual TModel Post([FromBody]TModel value)
		{
			if (_onlyGet) throw new HttpRequestException("Post is not allowed.");

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
		public virtual TModel Patch(long id, [FromBody]JsonPatchDocument<TModel> value)
		{
			if (_onlyGet) throw new HttpRequestException("Patch is not allowed.");

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
		public virtual TModel Put(long id, [FromBody]TModel value)
		{
			if (_onlyGet) throw new HttpRequestException("Put is not allowed.");

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
		public virtual void Delete(long id)
		{
			if (_onlyGet) throw new HttpRequestException("Delete is not allowed.");

			long localId = _mapper.FromModelId(id);
			TEntity t = new TEntity{ Id = localId };
			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}
	}
}
