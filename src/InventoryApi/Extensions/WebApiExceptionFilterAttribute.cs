using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryApi.Extensions
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			var exception = context.Exception;
			context.Result = new JsonResult(exception.Message);
			context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		}
	}
}
