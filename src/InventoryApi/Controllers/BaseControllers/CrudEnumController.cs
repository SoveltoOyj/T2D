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
	/// 
	/// </summary>
	/// <typeparam name="TEnumEntity"></typeparam>
	/// <typeparam name="TEnumModel"></typeparam>
	public class CrudEnumController<TEnumEntity,TEnumModel> : ApiBaseController
		where TEnumEntity: class,T2D.Entities.IEnumEntity, new()
		where TEnumModel: class, T2D.Model.IEnumModel, new()
	{

		protected MetadataEnumMapper<TEnumEntity, TEnumModel> _mapper;
		protected bool _onlyGet;

		public CrudEnumController(bool onlyGet = true) :base()
		{
			_mapper = new MetadataEnumMapper<TEnumEntity, TEnumModel>();
			_onlyGet = onlyGet;
		}


		// GET api/test/{model}
		[HttpGet]
		public virtual IEnumerable<TEnumModel> Get()
		{
			List<TEnumModel> ret = new List<TEnumModel>();
			foreach (var item in dbc.Set<TEnumEntity>())
			{
				ret.Add(_mapper.EntityToModel(item));
			}

			return ret;
		}

		// GET api/test/{model}/{id}
		[HttpGet("{id}")]
		public virtual TEnumModel Get(int id)
		{
			return  _mapper.EntityToModel(dbc.Set<TEnumEntity>().FirstOrDefault(t => t.Id == _mapper.FromModelId(id)));
		}

		// POST api/test/{model}
		// add a new Entity
		[HttpPost]
		public virtual TEnumModel Post([FromBody]TEnumModel value)
		{
			if (_onlyGet) throw new HttpRequestException("Post is not allowed.");

			var newEntity = new TEnumEntity();
			_mapper.UpdateEntityFromModel(value, newEntity);
			dbc.Set<TEnumEntity>().Add(newEntity);
			dbc.SaveChanges();
			return _mapper.EntityToModel(newEntity);
		}

		// Patch api/test/{model}/{id}
		//[
		//	{"op":"replace", "path":"name", "value": "a new value"}
		//]
		[HttpPatch("{id}")]
		public virtual TEnumModel Patch(int id, [FromBody]JsonPatchDocument<TEnumModel> value)
		{
			if (_onlyGet) throw new HttpRequestException("Patch is not allowed.");

			int localId = _mapper.FromModelId(id);
			TEnumEntity current = dbc.Set<TEnumEntity>().FirstOrDefault(t => t.Id == localId);
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
		public virtual TEnumModel Put(int id, [FromBody]TEnumModel value)
		{
			if (_onlyGet) throw new HttpRequestException("Put is not allowed.");

			int localId = _mapper.FromModelId(id);
			TEnumEntity current = dbc.Set<TEnumEntity>().FirstOrDefault(t => t.Id == localId);
			if (current == null)
				throw new Exception($"Entity {id} not Found.");

			_mapper.UpdateEntityFromModel(value, current);
			dbc.SaveChanges();
			return _mapper.EntityToModel(current);
		}

		// DELETE api/test/{model}/{id}
		[HttpDelete("{id}")]
		public virtual void Delete(int id)
		{
			if (_onlyGet) throw new HttpRequestException("Delete is not allowed.");

			int localId = _mapper.FromModelId(id);
			TEnumEntity t = new TEnumEntity { Id = localId };
			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}
	}
}
