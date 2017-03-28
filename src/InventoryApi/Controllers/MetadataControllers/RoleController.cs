using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;
using InventoryApi.Controllers.BaseControllers;


namespace InventoryApi.Controllers.MetadataControllers
{
	[Route("api/metadata/[controller]")]
	public class RoleController : CrudEnumController<T2D.Entities.Role, T2D.Model.Role>
	{
		public RoleController() : base(onlyGet:true)
		{
		}

	}
}
