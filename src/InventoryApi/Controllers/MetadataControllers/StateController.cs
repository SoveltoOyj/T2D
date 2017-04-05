using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InventoryApi.Controllers.MetadataControllers
{
	[Route("api/metadata/[controller]")]
	public class StateController : ApiBaseController
	{

		// GET api/test/{model}
		[HttpGet]
		public virtual IEnumerable<T2D.Model.State> Get()
		{
			List<T2D.Model.State> ret = new List<T2D.Model.State>();
			foreach (var item in GetValues<T2D.Entities.ServiceAndActitivityStateEnum>())
			{
				ret.Add(new T2D.Model.State { Id = (int)item, Name = item.ToString() });
			}

			return ret;
		}

		// GET api/test/{model}/{id}
		[HttpGet("{id}")]
		public virtual T2D.Model.State Get(int id)
		{
			var item = (T2D.Entities.ServiceAndActitivityStateEnum)id;
			return new T2D.Model.State { Id = (int)item, Name = item.ToString() };
		}


		[NonAction]
		private  IEnumerable<T> GetValues<T>()
			{
				return Enum.GetValues(typeof(T)).Cast<T>();
			}

	}
}

