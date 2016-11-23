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
	public class AttributeController : CrudEnumController<T2D.Entities.Attribute, T2D.Model.Attribute>
	{
		public AttributeController() : base(onlyGet:true)
		{
		}

	}
}
