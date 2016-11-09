using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using T2D.Entities;
using T2D.Helpers;
using T2D.Model;
using T2D.Model.Helpers;

namespace T2D.InventoryBL.Mappers
{
	/// <summary>
	/// Note: this mapper uses Model.ThingId also for Entity key!
	/// </summary>
	/// 

//	public class ThingMapper : IThingMapper<Entities.RegularThing, Model.Thing>
	public class ThingMapper : IThingMapper<Entities.BaseThing, Model.Thing>
	{
		static ThingMapper()
		{
			AutoMapper.Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<T2D.Entities.RegularThing, T2D.Model.Thing>()
					.ForMember(dest => dest.Id, opt => opt.MapFrom(src => ThingIdHelper.Create(src.Fqdn, src.US, true)))
					//.ForMember(dest => dest.Creator, opt => opt.MapFrom(src =>  src.Creator==null?"N/A":  ThingIdHelper.Create(src.Creator.CreatorFQDN, src.Creator.UniqueString,true)))
					//.ForMember(dest => dest.Parted, opt => opt.MapFrom(src => src.Parted==null?"N/A":  ThingIdHelper.Create(src.Parted.CreatorFQDN, src.Parted.UniqueString, true)))
				;

				cfg.CreateMap<T2D.Model.Thing, T2D.Entities.RegularThing>()
					.ForMember(dest => dest.Fqdn, opt => opt.MapFrom(src => src.Id != null? ThingIdHelper.GetFQDN(src.Id):null))
					.ForMember(dest => dest.US, opt => opt.MapFrom(src => src.Id != null ? ThingIdHelper.GetUniqueString(src.Id):null))
					//.ForMember(dest => dest.CreatorThingId_CreatorUri, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.CreatorUri:null))
					//.ForMember(dest => dest.CreatorThingId_UniqueString, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.UniqueString:null))
					//.ForMember(dest => dest.PartedThingId_CreatorUri, opt => opt.MapFrom(src => src.Parted != null ? src.Parted.CreatorUri:null))
					//.ForMember(dest => dest.PartedThingId_UniqueString, opt => opt.MapFrom(src => src.Parted != null ? src.Parted.UniqueString:null))
				;
			});
		}




		public string FromModelId(string id)
		{
			return id;
		}

		public string FromEntityId(string id)
		{
			return id;
		}

		public Model.Thing EntityToModel(Entities.BaseThing from)
		{
			Model.Thing ret = new Model.Thing();
			ret = AutoMapper.Mapper.Map<Model.Thing>(from);
			return ret;
		}
		public Entities.BaseThing ModelToEntity(Model.Thing from)
		{
			Entities.RegularThing ret = new Entities.RegularThing();
			ret = AutoMapper.Mapper.Map<Entities.RegularThing>(from);
			return ret;
		}

		/// <summary>
		/// Updates entity from model. All properties except id.
		/// </summary>
		/// <param name="to">Entity to update. All properties except Id.</param>
		/// <param name="from">Model where data is from.</param>
		public Entities.BaseThing UpdateEntityFromModel(Model.Thing from, Entities.BaseThing to)
		{
			string save1 = to.Fqdn;
			string save2 = to.US;

			to = ModelToEntity(from);
			to.Fqdn = save1;
			to.US = save2;
			return to;
		}

		public BaseThing UpdateEntityFromModel(Thing from, BaseThing to, bool updateAlsoId)
		{
			if (updateAlsoId)
			{
				to.Fqdn = ThingIdHelper.GetFQDN(from.Id);
				to.US = ThingIdHelper.GetUniqueString(from.Id);
			}
			UpdateEntityFromModel(from, to);
			return to;
		}

	}
}

