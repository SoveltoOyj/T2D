using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Controllers.BaseControllers;
using T2D.InventoryBL.Mappers;
using Microsoft.AspNetCore.JsonPatch;
using T2D.Model;

namespace InventoryApi.Controllers.TestControllers
{
	[Route("api/test/[controller]")]
	public class ThingController : CrudThingController<T2D.Entities.RegularThing, T2D.Model.Thing>
	{
		public ThingController():base(new ThingMapper())
		{
		}

		//example of overrided get
//		public override IEnumerable<Thing> Get()
//		{
//			List<T2D.Model.Thing> ret = new List<T2D.Model.Thing>();
//			//			foreach (var item in dbc.Set<T2D.Entities.RegularThing>())
//			foreach (var item in dbc.Set<T2D.Entities.BaseThing>().OfType<T2D.Entities.ArchetypeThing>())
//				{
//					ret.Add(new T2D.Model.Thing { Id = ThingId.Create(item.Id_CreatorUri, item.Id_UniqueString) });
////				ret.Add(_mapper.EntityToModel(item));
//			}

//			return ret;

//		}
	}
}
