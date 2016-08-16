using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Extensions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers.BaseControllers
{
	[WebApiExceptionFilterAttribute]
	public class ApiBaseController : Controller
	{

		protected T2D.Infra.EfContext dbc = new T2D.Infra.EfContext();

		protected override void Dispose(bool disposing)
		{
			dbc.Dispose();
			base.Dispose(disposing);
		}
	}
}
