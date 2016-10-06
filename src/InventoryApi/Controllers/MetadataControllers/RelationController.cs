using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;
using InventoryApi.Controllers.BaseControllers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers.MetadataControllers
{
	[Route("api/metadata/[controller]")]
	public class RelationController : CrudBaseController<T2D.Entities.Relation, T2D.Model.Relation>
	{
		public RelationController() : base(onlyGet:true, mapper: new MetadataEnumMapper<T2D.Entities.Relation, T2D.Model.Relation>())
		{
		}

	}
}
