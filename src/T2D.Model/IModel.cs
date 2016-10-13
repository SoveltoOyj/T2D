using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace T2D.Model
{


	public interface IBaseModel
	{
	}

	public interface IThingModel:IBaseModel
	{
		string Id { get; set; }
		string Title { get; set; }
	}

	public interface IModel : IBaseModel
	{
		long Id { get; set; }
	}

	public interface IEnumModel : IModel
	{
		string Name { get; set; }
	}


}