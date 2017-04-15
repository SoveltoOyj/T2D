using InventoryApi.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace InventoryApi.Controllers.BaseControllers
{
	[WebApiExceptionFilter]
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
