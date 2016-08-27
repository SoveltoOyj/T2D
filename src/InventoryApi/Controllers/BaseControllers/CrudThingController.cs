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

namespace InventoryApi.Controllers.TestControllers
{
	public class CrudThingController<TThingEntity,TThingModel> : ApiBaseController
		where TThingEntity: class,T2D.Entities.IThingEntity, new()
		where TThingModel: class, T2D.Model.IThingModel
	{
		protected T2D.InventoryBL.IThingMapper<TThingEntity, TThingModel, T2D.Model.ThingId> _mapper;
		public CrudThingController(T2D.InventoryBL.IThingMapper<TThingEntity, TThingModel, T2D.Model.ThingId> mapper):base()
		{
			_mapper = mapper;
		}


		// GET api/test/{model}
		[HttpGet("search")]
		public virtual IEnumerable Search(int page = 0, int pageSize = 10, string orderBy = "Id", string select = "Id, Width", string where="")
		{
			List<TThingModel> ret = new List<TThingModel>();
			PaginationHeader ph = new PaginationHeader();

			string orderByStr = "";
			foreach (var item in orderBy.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				if (orderByStr.Length > 0)
					orderByStr += ", ";
				orderByStr += _mapper.ModelToEntityPropertyName(item);
			}
			var query = dbc.Set<TThingEntity>().AsQueryable();
			query = query.FromSql($"select * from Things ");

			ph.TotalCount = query.LongCount();
			ph.CurrentPage = page;
			ph.PageSize = pageSize;
			ph.MorePages = ((page + 1) * pageSize) < ph.TotalCount;

			query = dbc.Set<TThingEntity>().AsQueryable();
			query = dbc.Set<TThingEntity>()
				.AsQueryable()
				.FromSql($"select * from Things t WHERE [t].[Discriminator] IN (N'ArchivedThing', N'RegularThing') order by {orderByStr} offset {page * pageSize} rows fetch next {pageSize} rows only")
				;
			query.OrderBy(t=>t.Id_CreatorUri);
			foreach (var item in query)
			{
				ret.Add(_mapper.EntityToModel(item));
			}

			this.Response.Headers.Add("X-Pagination", ph.ToString());
				return ret;

		}


		[HttpGet()]
		public virtual IEnumerable<TThingModel> Get(int page = 0, int pageSize = 10)
		{
			List<TThingModel> ret = new List<TThingModel>();
			PaginationHeader ph = new PaginationHeader();
			IQueryable<TThingEntity> query = dbc.Set<TThingEntity>().OrderBy(e=>new { e.Id_CreatorUri, e.Id_UniqueString });

			ph.TotalCount = query.LongCount();
			ph.CurrentPage = page;
			ph.PageSize = pageSize;
			ph.MorePages = ((page + 1) * pageSize) < ph.TotalCount;

			foreach (var item in dbc.Set<TThingEntity>().Skip(page*pageSize).Take(pageSize))
			{
				ret.Add(_mapper.EntityToModel(item));
			}

			this.Response.Headers.Add("X-Pagination", ph.ToString());
			return ret;
		}

		// GET api/test/{model}/id?cu=creatorUri&us=uniqueString
		// f.ex. http://localhost:27122/api/test/thing/id?cu=sovelto.fi/inventory&us=ThingNb2
		[HttpGet("id")]
		public virtual TThingModel Get(Uri cu, string us)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);
			return  _mapper.EntityToModel(dbc.Set<TThingEntity>().FirstOrDefault(t => t.Id_CreatorUri==key.CreatorUri.ToString() && t.Id_UniqueString== key.UniqueString ));
		}

		// POST api/test/{model}
		// add a new Entity
		// f.ex:
		// {
		//	id: {
		//		creatorUri: "sovelto.fi/inventory",
		//		uniqueString: "ThingNb jokin muu"
		//	},
		//	height: 124.5,
		//	width: 43
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
		public virtual TThingModel Patch(Uri cu, string us, [FromBody]JsonPatchDocument<TThingModel> value)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);

			TThingEntity current = dbc.Set<TThingEntity>().FirstOrDefault(t => t.Id_CreatorUri == key.CreatorUri.ToString() && t.Id_UniqueString == key.UniqueString);
			if (current == null) throw new Exception($"Thing {key} not Found");

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
		public virtual TThingModel Put(Uri cu, string us, [FromBody]TThingModel value)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);
			TThingEntity current = dbc.Set<TThingEntity>().FirstOrDefault(t => t.Id_CreatorUri == key.CreatorUri.ToString() && t.Id_UniqueString == key.UniqueString);
			if (current == null) throw new Exception($"Thing {key} not Found");

			_mapper.UpdateEntityFromModel(value, current, false);
			dbc.SaveChanges();
			return _mapper.EntityToModel(current);
		}

		// DELETE api/test/{model}/?cu=creatorUri&us=uniqueString
		[HttpDelete()]
		public virtual void Delete(Uri cu, string us)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);
			TThingEntity t = new TThingEntity { Id_CreatorUri = key.CreatorUri.ToString(), Id_UniqueString = key.UniqueString };
			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}
	}
}
