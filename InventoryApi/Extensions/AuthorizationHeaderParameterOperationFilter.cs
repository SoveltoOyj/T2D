using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace InventoryApi.Extensions
{
	//https://stackoverflow.com/questions/38784537/use-jwt-authorization-bearer-in-swagger-in-asp-net-core-1-0

	/// <summary>
	/// For Swagger Authorization Bearer JWT argument
	/// </summary>
	public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
	{
		public void Apply(Operation operation, OperationFilterContext context)
		{
			var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
			var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
			var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

			if (isAuthorized && !allowAnonymous)
			{
				if (operation.Parameters == null)
					operation.Parameters = new List<IParameter>();

				operation.Parameters.Add(new NonBodyParameter
				{
					Name = "Authorization",
					In = "header",
					Description = "Bearer JWT access token",
					Required = true,
					Type = "string",
					Default = "Bearer {add JWT token here}"
				});
			}
		}

	}
}
