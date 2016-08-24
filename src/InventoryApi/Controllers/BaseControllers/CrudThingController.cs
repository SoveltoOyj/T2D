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
		[HttpGet()]
		public IEnumerable<TThingModel> Get()
		{
			List<TThingModel> ret = new List<TThingModel>();
			foreach (var item in dbc.Set<TThingEntity>())
			{
				ret.Add(_mapper.EntityToModel(item));
			}

			return ret;
		}

		// GET api/test/{model}?cu=creatorUri&us=uniqueString
		// f.ex. http://localhost:27122/api/test/thing/id?cu=sovelto.fi/inventory&us=ThingNb2
		[HttpGet("id")]
		public TThingModel Get(string cu, string us)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);
			return  _mapper.EntityToModel(dbc.Set<TThingEntity>().FirstOrDefault(t => t.Id_CreatorUri==key.CreatorUri && t.Id_UniqueString== key.UniqueString ));
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
		public TThingModel Post([FromBody]TThingModel value)
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
		public TThingModel Patch(string cu, string us, [FromBody]JsonPatchDocument<TThingModel> value)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);

			TThingEntity current = dbc.Set<TThingEntity>().FirstOrDefault(t => t.Id_CreatorUri == key.CreatorUri && t.Id_UniqueString == key.UniqueString);
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
		public TThingModel Put(string cu, string us, [FromBody]TThingModel value)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);
			TThingEntity current = dbc.Set<TThingEntity>().FirstOrDefault(t => t.Id_CreatorUri == key.CreatorUri && t.Id_UniqueString == key.UniqueString);
			if (current == null) throw new Exception($"Thing {key} not Found");

			_mapper.UpdateEntityFromModel(value, current, false);
			dbc.SaveChanges();
			return _mapper.EntityToModel(current);
		}

		// DELETE api/test/{model}/?cu=creatorUri&us=uniqueString
		[HttpDelete()]
		public void Delete(string cu, string us)
		{
			T2D.Model.ThingId key = T2D.Model.ThingId.Create(cu, us);
			TThingEntity t = new TThingEntity { Id_CreatorUri = key.CreatorUri, Id_UniqueString = key.UniqueString };
			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}
	}
}
