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
using T2D.Model.Helpers;

namespace InventoryApi.Controllers.BaseControllers
{
	public class CrudThingController<TThingEntity,TThingModel> : ApiBaseController
		where TThingEntity: class,T2D.Entities.IThingEntity, new()
		where TThingModel: class, T2D.Model.IThingModel
	{
		protected T2D.InventoryBL.IThingMapper<T2D.Entities.IThingEntity, TThingModel> _mapper;
		public CrudThingController(T2D.InventoryBL.IThingMapper<T2D.Entities.IThingEntity, TThingModel> mapper):base()
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

		// GET api/test/{model}/id?c=creatorUri&u=uniqueString
		// f.ex. http://localhost:27122/api/test/thing/id?c=sovelto.fi/inventory&u=ThingNb2
		[HttpGet("id")]
		public virtual TThingModel Get([FromQuery] string c, [FromQuery]string u)
		{
			return  _mapper.EntityToModel(Find(c,u));
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
			newEntity= (TThingEntity) _mapper.UpdateEntityFromModel(value, newEntity,true);
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

			TThingEntity current = Find(c, u);
			if (current == null) throw new Exception($"Thing not Found");

			var updatedModel = _mapper.EntityToModel(current);
			value.ApplyTo(updatedModel);

			current = (TThingEntity) _mapper.UpdateEntityFromModel(updatedModel, current, false);

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
			TThingEntity current = Find(value);
			if (current == null) throw new Exception($"Thing not Found");

			current= (TThingEntity) _mapper.UpdateEntityFromModel(value, current, false);
			dbc.SaveChanges();
			return _mapper.EntityToModel(current);
		}

		// DELETE api/test/{model}/?c=creatorUri&u=uniqueString
		[HttpDelete()]
		public virtual void Delete([FromQuery] string c, [FromQuery]string u)
		{
			var t = Find(c,u);
			if (t==null) throw new Exception($"Thing not Found");

			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}


		private TThingEntity Find(TThingModel value)
		{
			if (value == null || value.Id==null) throw new ArgumentNullException("value", "Thing or Thing Id is null.");
			return Find(ThingIdHelper.GetFQDN(value.Id), ThingIdHelper.GetUniqueString(value.Id));
		}
		private TThingEntity Find(string c, string u)
		{
			return dbc.Set<TThingEntity>().FirstOrDefault(t => t.Fqdn == c && t.US == u);
		}
	}
}
