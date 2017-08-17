using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryApi.Controllers.BaseControllers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Reflection;
using T2D.InventoryBL.Metadata;
using T2D.Model;
using System.ComponentModel.DataAnnotations;
using T2D.Infra;
using Hangfire;
using System.Text;
using T2D.Entities;
using T2D.Infra.TestData;
using T2D.InventoryBL.Thing;
using T2D.Helpers;
using System.Runtime.Serialization.Json;
using T2D.InventoryBL;

namespace InventoryApi.Controllers.MetadataControllers
{
	[Route("api/[controller]/[Action]")]
	public class TestdataController : ApiBaseController
	{

		private readonly IHostingEnvironment _env;

		public TestdataController(IHostingEnvironment env, EfContext dbc): base(dbc)
		{
			_env = env;
		}

		/// <summary>
		/// Creates a new empty db. All existing data will be deleted.
		/// </summary>
		/// <response code="200">Returns version number.</response>
		[HttpGet(), ActionName("CreateNewDb")]
		[Produces(typeof(string))]
		public IActionResult CreateNewDb()
		{
			_dbc.Database.EnsureDeleted();
			_dbc.Database.EnsureCreated();

			new T2D.Infra.TestData.BasicData(_dbc).DoIt();

			return Ok("database created and base data inserted.");
		}

		/// <summary>
		/// Add some Service Definitions. This should run only once.
		/// </summary>
		/// <response code="200">Returns version number.</response>
		[HttpGet(), ActionName("AddServiceRequestData")]
		[Produces(typeof(string))]
		public IActionResult AddServiceRequestData()
		{
			new T2D.Infra.TestData.Service_Action_TestData(_dbc).DoIt();

			return Ok("Service definition inserted.");
		}


		/// <summary>
		/// Add Fiskars test data (should run only once).
		/// </summary>
		/// <response code="200">Returns version number.</response>
		[HttpGet(), ActionName("AddFiskarsData")]
		[Produces(typeof(string))]
		public IActionResult AddFiskarsData()
		{
			string webRootPath = _env.WebRootPath;
			StringBuilder msg= new StringBuilder();
			List<Fiskars> products = new List<Fiskars>();

			//find the last GUID
			var lastGuid =
				_dbc.Things
				.Select (t=>new { t.Id, str=t.Id.ToString()})
				.ToList()
				.Where(t=>t.str.StartsWith("00000000-0000-0000-0000"))
				.OrderByDescending(tt=>tt.str)
				.First()
				;


			var file = Path.Combine(webRootPath, "Functional Product Data for T2D demo.csv");
			using (var streamReader = System.IO.File.OpenText(file))
			{
				// read header
				streamReader.ReadLine();
				System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("FI-fi");

				while (!streamReader.EndOfStream)
				{
					var line = streamReader.ReadLine();
					var data = line.Split(new[] { ';' });
					if (data.Length != 11)
					{
						msg.Append($"Skipped {line}, wrong csv count {data.Length}\n");
						continue;
					}
					products.Add(
						new Fiskars
						{
							Manufacturer =data[0],
							ProductDescription = data[1],
							DateOfManufacture =  DateTime.Parse(data[2],ci).Date,
							EanCode = long.Parse(data[3]),
							BusinessCategory = data[4],
							Activity = data[5],
							MarketArea = data[6],
							ProductLine = data[7],
							ProductType = data[8],
							CountryOfOrigin = data[9],
							ProductPage = new Uri(data[10], UriKind.Absolute)
						}

						);
				}
			}

			T2D.Infra.TestData.CommonTestData.SetNextGuid(T2D.Infra.TestData.CommonTestData.ConvertGuidToLong(lastGuid.Id));
			ThingBL thingBl = ThingBL.CreateThingBL(_dbc, null);
			int count = 0;
			foreach (var item in products)
			{
				string us = item.ProductDescription.Replace(" ", string.Empty);
				string orginalus = us;
				//if it exist, add number
				for (int i = 0; i < 100; i++)
				{
					var t = _dbc.Things.SingleOrDefault(tt => tt.US == us);
					if (t == null) break;
					us = orginalus + "_" + (i + 1).ToString("000");
				}

				var newThing = new ArchetypeThing
				{
					Id = CommonTestData.Next(),
					Fqdn = CommonTestData.Fqdn,
					US = us,
					Title = item.ProductDescription,
					Published = item.DateOfManufacture,
				};
				try
				{

					_dbc.ArchetypeThings.Add(newThing);
					_dbc.SaveChanges();
				}
				catch(Exception ex)
				{
					msg.Append($"Could not add {item.ProductDescription} because: {ex.Message}\n");
					continue;
				}
				thingBl.SetExtensionValue(newThing, "inv1.sovelto.fi/Fiskars", item.ToString());
				count++;
			}


			msg.Append($"added {count} products of  {products.Count} available.");
			return Ok(msg.ToString() );
		}


		/// <summary>
		/// Returns all Things in DB for test purposes.
		/// </summary>
		/// <response code="200">All things.</response>
		[HttpGet(), ActionName("GetTestData")]
		[Produces(typeof(string))]
		public IActionResult GetTestData()
		{
			var data = _dbc.Things
				.Select(t => new { ID = _dbc.GetThingStrId(t), Type= t.GetType().Name })
				.ToList()
				;

			return Ok(data);
		}


		public class Fiskars
		{
			public string Manufacturer { get; set; }
			public string ProductDescription { get; set; }
			public DateTime DateOfManufacture { get; set; }
			public long EanCode { get; set; }
			public string BusinessCategory { get; set; }
			public string Activity { get; set; }
			public string MarketArea { get; set; }
			public string ProductLine { get; set; }
			public string ProductType { get; set; }
			public string CountryOfOrigin { get; set; }
			public Uri ProductPage{ get; set; }

			public override string ToString()
			{
				var json = new DataContractJsonSerializer(this.GetType());
				using (var ms = new MemoryStream())
				{
					json.WriteObject(ms, this);
					ms.Position = 0;
					return Encoding.UTF8.GetString(ms.ToArray());
				};
			}
		}
	}
}
