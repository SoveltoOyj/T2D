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
	[Route("api/test/[controller]")]
	public class ThingController : CrudThingController<T2D.Entities.RegularThing, T2D.Model.Thing>
	{
		public ThingController():base(new ThingMapper())
		{
		}
	}
}
