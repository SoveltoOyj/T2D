using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Mappers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers.TestControllers
{
	[Route("api/test/[controller]")]
	public class RoleController : CrudBaseController<T2D.Entities.Role, T2D.Model.Role>
	{
		public RoleController() : base(new EnumMapper<T2D.Entities.Role, T2D.Model.Role>())
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public override void Delete(string id)
		{
			throw new Exception("Can't delete a role");
		}
	}
}
