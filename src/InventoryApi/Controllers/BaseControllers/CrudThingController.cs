using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Controllers.BaseControllers;
using T2D.InventoryBL.Mappers;
using Microsoft.AspNetCore.JsonPatch;
using T2D.Model;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using T2D.Helpers;
using System.Linq.Expressions;

namespace InventoryApi.Controllers.BaseControllers
{
	public class CrudThingController<TThingEntity,TThingModel> : ApiBaseController
		where TThingEntity: class,T2D.Entities.IThingEntity, new()
		where TThingModel: class, T2D.Model.IThingModel
	{
		protected T2D.InventoryBL.IThingMapper<TThingEntity, TThingModel> _mapper;
		public CrudThingController(T2D.InventoryBL.IThingMapper<TThingEntity, TThingModel> mapper):base()
		{
			_mapper = mapper;
		}

		[HttpGet()]
		public virtual IEnumerable<TThingModel> Get(int page = 0, int pageSize = 10)
		{
			List<TThingModel> ret = new List<TThingModel>();
			PaginationHeader ph = new PaginationHeader();
			//			IQueryable<TThingEntity> query = dbc.Set<TThingEntity>().OrderBy(e=>new { e.CreatorFQDN, e.UniqueString });
			IQueryable<TThingEntity> query = dbc.Set<TThingEntity>().OrderBy(e => e.Fqdn);

			ph.TotalCount = query.LongCount();
			ph.CurrentPage = page;
			ph.PageSize = pageSize;
			ph.MorePages = ((page + 1) * pageSize) < ph.TotalCount;

			foreach (var item in query.Skip(page*pageSize).Take(pageSize))
			{
				ret.Add(_mapper.EntityToModel(item));
			}

			this.Response.Headers.Add("X-Pagination", ph.ToString());
			return ret;
		}

		// GET api/test/{model}/id?cu=creatorUri&us=uniqueString
		// f.ex. http://localhost:27122/api/test/thing/id?cu=sovelto.fi/inventory&us=ThingNb2
		[HttpGet("id")]
		public virtual TThingModel Get(string cu, string us)
		{
			return  _mapper.EntityToModel(dbc.Set<TThingEntity>().FirstOrDefault(t => t.Fqdn==cu && t.US== us ));
		}

		// POST api/test/{model}
		// add a new Entity
		// f.ex:
		// {
		//	id: {
		//		creatorUri: "sovelto.fi/inventory",
		//		uniqueString: "ThingNb jokin muu"
		//	},
		//	Location_GPS: "124.5"
		//	}
		[HttpPost]
		public virtual TThingModel Post([FromBody]TThingModel value)
		{
			var newEntity = new TThingEntity();
			_mapper.UpdateEntityFromModel(value, newEntity,true);
			dbc.Set<TThingEntity>().Add(newEntity);
			dbc.SaveChanges();
			return _mapper.EntityToModel(newEntity);
		}

		// Patch api/test/{model}?cu=creatorUri&us=uniqueString
		//[
		//  {"op":"replace",
		//	  "path":"height",
		//	  "value": 9988
		//  },
		//	{"op":"replace",
		//	  "path":"width",
		//	  "value": 123 
		//	}
		//]
		[HttpPatch()]
		public virtual TThingModel Patch(string cu, string us, [FromBody]JsonPatchDocument<TThingModel> value)
		{

			TThingEntity current = dbc.Set<TThingEntity>().FirstOrDefault(t => t.Fqdn == cu && t.US == us);
			if (current == null) throw new Exception($"Thing not Found");

			var updatedModel = _mapper.EntityToModel(current);
			value.ApplyTo(updatedModel);

			_mapper.UpdateEntityFromModel(updatedModel, current, false);

			dbc.SaveChanges();
			return updatedModel;
		}


		// PUT api/test/{model}/?cu=creatorUri&us=uniqueString
		// update whole entity
		// f.ex:
		//http://localhost:27122/api/test/thing/?cu=sovelto.fi/inventory&us=ThingNb2
		// {
		//  height: 124.5,
		//  width: 43
		// }
	[HttpPut()]
		public virtual TThingModel Put(string cu, string us, [FromBody]TThingModel value)
		{
			TThingEntity current = dbc.Set<TThingEntity>().FirstOrDefault(t => t.Fqdn == cu && t.US == us);
			if (current == null) throw new Exception($"Thing not Found");

			_mapper.UpdateEntityFromModel(value, current, false);
			dbc.SaveChanges();
			return _mapper.EntityToModel(current);
		}

		// DELETE api/test/{model}/?cu=creatorUri&us=uniqueString
		[HttpDelete()]
		public virtual void Delete(string cu, string us)
		{
			TThingEntity t = new TThingEntity { Fqdn = cu, US = us };
			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}
	}
}
