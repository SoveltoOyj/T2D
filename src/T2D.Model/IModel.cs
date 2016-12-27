using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace T2D.Model
{

	public interface IThing
	{
		string Id { get; set; }
		string Title { get; set; }
	}

	public interface IModel 
	{
		Guid Id { get; set; }
	}

	public interface IEnumModel
	{
		int Id { get; set; }
		string Name { get; set; }
	}


}