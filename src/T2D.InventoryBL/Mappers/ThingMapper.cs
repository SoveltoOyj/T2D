using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using T2D.Entities;
using T2D.Helpers;
using T2D.Model;

namespace T2D.InventoryBL.Mappers
{
	/// <summary>
	/// Note: this mapper uses Model.ThingId also for Entity key!
	/// </summary>
	/// 

	public class ThingMapper : IThingMapper<Entities.RegularThing, Model.Thing, ThingId>
	{
		static ThingMapper()
		{
			AutoMapper.Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<T2D.Entities.RegularThing, T2D.Model.Thing>()
					.ForMember(dest => dest.Id, opt => opt.MapFrom(src => ThingId.Create(src.Id_CreatorUri, src.Id_UniqueString)))
					.ForMember(dest => dest.Creator, opt => opt.MapFrom(src => ThingId.Create(src.CreatorThingId_CreatorUri, src.CreatorThingId_UniqueString)))
					.ForMember(dest => dest.Parted, opt => opt.MapFrom(src => ThingId.Create(src.PartedThingId_CreatorUri, src.PartedThingId_UniqueString)))
				;

				cfg.CreateMap<T2D.Model.Thing, T2D.Entities.RegularThing>()
					.ForMember(dest => dest.Id_CreatorUri, opt => opt.MapFrom(src => src.Id != null? src.Id.CreatorUri:null))
					.ForMember(dest => dest.Id_UniqueString, opt => opt.MapFrom(src => src.Id != null ? src.Id.UniqueString:null))
					//.ForMember(dest => dest.CreatorThingId_CreatorUri, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.CreatorUri:null))
					//.ForMember(dest => dest.CreatorThingId_UniqueString, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.UniqueString:null))
					//.ForMember(dest => dest.PartedThingId_CreatorUri, opt => opt.MapFrom(src => src.Parted != null ? src.Parted.CreatorUri:null))
					//.ForMember(dest => dest.PartedThingId_UniqueString, opt => opt.MapFrom(src => src.Parted != null ? src.Parted.UniqueString:null))
				;
			});
		}




		public ThingId FromModelId(ThingId id)
		{
			return id;
		}

		public ThingId FromEntityId(ThingId id)
		{
			return id;
		}

		public Model.Thing EntityToModel(Entities.RegularThing from)
		{
			Model.Thing ret = new Model.Thing();
			ret = AutoMapper.Mapper.Map<Model.Thing>(from);
			return ret;
		}
		public Entities.RegularThing ModelToEntity(Model.Thing from)
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
		public Entities.RegularThing UpdateEntityFromModel(Model.Thing from, Entities.RegularThing to)
		{
			string save1 = to.Id_CreatorUri;
			string save2 = to.Id_UniqueString;

			to = ModelToEntity(from);
			to.Id_CreatorUri = save1;
			to.Id_UniqueString = save2;
			return to;
		}

		public RegularThing UpdateEntityFromModel(Thing from, RegularThing to, bool updateAlsoId)
		{
			if (updateAlsoId)
			{
				to.Id_CreatorUri = from.Id.CreatorUri.ToString();
				to.Id_UniqueString = from.Id.UniqueString;
			}
			UpdateEntityFromModel(from, to);
			return to;
		}

	}
}

