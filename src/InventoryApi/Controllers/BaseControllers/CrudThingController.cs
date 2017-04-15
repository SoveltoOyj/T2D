using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;
using Microsoft.AspNetCore.JsonPatch;
using T2D.Model.Helpers;

namespace InventoryApi.Controllers.BaseControllers
{
	public class CrudThingController<TThingEntity,TThingModel> : ApiBaseControllerOld
		where TThingEntity: class, T2D.Entities.IThing, new()
		where TThingModel: class, T2D.Model.IThing, new()
	{
		protected ThingMapper<TThingEntity, TThingModel> _mapper;
		public CrudThingController():base()
		{
			_mapper = new ThingMapper<TThingEntity, TThingModel>();
		}

		[HttpGet()]
		public virtual IEnumerable<TThingModel> Get(int page = 0, int pageSize = 10)
		{
			List<TThingModel> ret = new List<TThingModel>();
			T2D.Model.PaginationHeader ph = new T2D.Model.PaginationHeader();
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

		// GET api/test/{model}/id?c=creatorUri&u=uniqueString
		// f.ex. http://localhost:27122/api/test/thing/id?c=sovelto.fi/inventory&u=ThingNb2
		[HttpGet("id")]
		public virtual TThingModel Get([FromQuery] string c, [FromQuery]string u)
		{
			return  _mapper.EntityToModel(Find<TThingEntity>(c,u));
		}

		// POST api/test/{model}
		// add a new Entity
		// f.ex:
		// {
		//	id: "inventory1.sovelto.fi/ThingNb jokin muu"
		//	Location_GPS: "124.5"
		//	}
		[HttpPost]
		public virtual TThingModel Post([FromBody]TThingModel value)
		{
			var newEntity = new TThingEntity();
			_mapper.UpdateEntityFromModel(value, ref newEntity,true);
			dbc.Set<TThingEntity>().Add(newEntity);
			dbc.SaveChanges();
			return _mapper.EntityToModel(newEntity);
		}

		// Patch api/test/{model}?c=creatorUri&u=uniqueString
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
		public virtual TThingModel Patch([FromQuery] string c, [FromQuery]string u, [FromBody]JsonPatchDocument<TThingModel> value)
		{

			TThingEntity current = Find<TThingEntity>(c, u);
			if (current == null) throw new Exception($"Thing not Found");

			var updatedModel = _mapper.EntityToModel(current);
			value.ApplyTo(updatedModel);

			_mapper.UpdateEntityFromModel(updatedModel, ref current, false);

			dbc.SaveChanges();
			return updatedModel;
		}


		// PUT api/test/{model}
		// update the whole entity
		// f.ex:
		//http://localhost:27122/api/test/thing
		// {
		//  id:"inv1.sovelto.fi/uusi6"
		//  height: 124.5,
		//  width: 43
		// }
		[HttpPut()]
		public virtual TThingModel Put([FromBody]TThingModel value)
		{
			TThingEntity current = Find<TThingEntity, TThingModel>(value);
			if (current == null) throw new Exception($"Thing not Found");

			_mapper.UpdateEntityFromModel(value, ref current, false);
			dbc.SaveChanges();
			return _mapper.EntityToModel(current);
		}

		// DELETE api/test/{model}/?c=creatorUri&u=uniqueString
		[HttpDelete()]
		public virtual void Delete([FromQuery] string c, [FromQuery]string u)
		{
			var t = Find<TThingEntity>(c,u);
			if (t==null) throw new Exception($"Thing not Found");

			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}


		//private TThingEntity Find(TThingModel value)
		//{
		//	if (value == null || value.Id==null) throw new ArgumentNullException("value", "Thing or Thing Id is null.");
		//	return Find(ThingIdHelper.GetFQDN(value.Id), ThingIdHelper.GetUniqueString(value.Id));
		//}
		//private TThingEntity Find(string c, string u)
		//{
		//	return dbc.Set<TThingEntity>().FirstOrDefault(t => t.Fqdn == c && t.US == u);
		//}
	}
}
