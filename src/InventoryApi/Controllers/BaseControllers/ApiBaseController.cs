using InventoryApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using T2D.InventoryBL.Metadata;

namespace InventoryApi.Controllers.BaseControllers
{
	[WebApiExceptionFilter]
	public class ApiBaseController : Controller
	{

		protected T2D.Infra.EfContext _dbc = new T2D.Infra.EfContext();
		protected EnumBL _enumBL = new EnumBL();

		protected override void Dispose(bool disposing)
		{
			_dbc.Dispose();
			base.Dispose(disposing);
		}
	}
}
