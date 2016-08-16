using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Controllers.BaseControllers;
using T2D.Model.Mappers;
using Microsoft.AspNetCore.JsonPatch;

namespace InventoryApi.Controllers.TestControllers
{
	[Route("api/test/[controller]")]
	public class ThingController : ApiBaseController
	{
		// GET api/test/thing
		[HttpGet]
		public IEnumerable<T2D.Model.Thing> Get()
		{
			List<T2D.Model.Thing> ret = new List<T2D.Model.Thing>();
			foreach (var item in dbc.Things)
			{
				ret.Add(item.EntityToModel());
			}

			return ret;
		}

		// GET api/test/thing/5
		[HttpGet("{id}")]
		public T2D.Model.Thing Get(int id)
		{
			return dbc.Things.FirstOrDefault(t => t.Id == id).EntityToModel();
		}

		// POST api/test/thing
		// add a new Thing
		[HttpPost]
		public void Post([FromBody]T2D.Model.Thing value)
		{
			dbc.Things.Add(new T2D.Entities.Thing { Name = value.Name });
			dbc.SaveChanges();
		}

        // Patch api/test/thing/5
  //      [
		//{"op":"replace", "path":"name",
		//  "value": "A thing uusin taas paivitetty"

  //      }
		//]
		[HttpPatch("{id}")]
		public T2D.Model.Thing Patch(string id, [FromBody]JsonPatchDocument<T2D.Model.Thing> value)
		{
			long localId = ThingMapper.FromModelId(id);
			T2D.Entities.Thing current = dbc.Things.FirstOrDefault(t => t.Id == localId);
			if (current == null) throw new Exception("Thing not Found");

			var updatedModel = current.EntityToModel();
			value.ApplyTo(updatedModel);

			current.UpdateEntityFromModel(updatedModel);

			dbc.SaveChanges();
			return updatedModel;
		}


		// PUT api/test/thing/5
		// update whole entity
		[HttpPut("{id}")]
		public void Put(string id, [FromBody]T2D.Model.Thing value)
		{
			long localId = ThingMapper.FromModelId(id);
			T2D.Entities.Thing current = dbc.Things.FirstOrDefault(t => t.Id == localId);
			if (current == null)
				throw new Exception("Thing not Found.");

			current.UpdateEntityFromModel(value);
			dbc.SaveChanges();
		}

		// DELETE api/test/thing/5
		[HttpDelete("{id}")]
		public void Delete(string id)
		{
			long localId = ThingMapper.FromModelId(id);
			T2D.Entities.Thing t = new T2D.Entities.Thing { Id = localId };
			dbc.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			dbc.SaveChanges();
		}
	}
}
