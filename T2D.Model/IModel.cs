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

	public class ModelEnum
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}


}