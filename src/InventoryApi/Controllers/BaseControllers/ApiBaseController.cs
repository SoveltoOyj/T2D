using InventoryApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using T2D.Infra;
using T2D.InventoryBL.Metadata;

namespace InventoryApi.Controllers.BaseControllers
{
	[WebApiExceptionFilter]
	public class ApiBaseController : Controller
	{
		public ApiBaseController(EfContext dbc)
		{
			_dbc = dbc;
		}
		protected readonly EfContext _dbc;
		protected EnumBL _enumBL = new EnumBL();

		protected override void Dispose(bool disposing)
		{
			_dbc.Dispose();
			base.Dispose(disposing);
		}
	}
}
