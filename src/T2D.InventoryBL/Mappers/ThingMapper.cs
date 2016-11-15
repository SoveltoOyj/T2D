using AutoMapper;
using T2D.Model.Helpers;

namespace T2D.InventoryBL.Mappers
{

	public class ThingProfile :Profile
	{
		public ThingProfile()
		{
			CreateMap<Entities.IThing, Model.IThing>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => ThingIdHelper.Create(src.Fqdn, src.US, true)))
				;
			CreateMap<Model.IThing, Entities.IThing>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Fqdn, opt => opt.MapFrom(src => src.Id != null ? ThingIdHelper.GetFQDN(src.Id) : null))
				.ForMember(dest => dest.US, opt => opt.MapFrom(src => src.Id != null ? ThingIdHelper.GetUniqueString(src.Id) : null))
				;

			CreateMap<Model.BaseThing, Entities.BaseThing>()
				.IncludeBase<Model.IThing, Entities.IThing>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				;

			CreateMap<Entities.BaseThing, Model.BaseThing>()
				.IncludeBase<Entities.IThing, Model.IThing>()
				;
		}
	}


	public class ThingMapper<TEntity, TModel>
		where TEntity : class, T2D.Entities.IThing, new()
		where TModel : class, T2D.Model.IThing, new()
	{ 
		private IMapper _mapper;
		public ThingMapper()
		{
			var config = new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<ThingProfile>();
				}
			);
			_mapper = config.CreateMapper();
		}

		public TModel EntityToModel(TEntity from)
		{
			var ret =  _mapper.Map<TModel>(from);
			return ret;
		}
		public TEntity ModelToEntity(TModel from)
		{
			var ret = _mapper.Map<TEntity>(from);
			return ret;
		}

		public void UpdateEntityFromModel(TModel from, ref TEntity to, bool updateThingId=false)
		{
			string fqdn=null, us=null;
			if (!updateThingId)
			{
				fqdn = to.Fqdn;
				us = to.US;
			}
			to = ModelToEntity(from);
			if (!updateThingId)
			{
				to.Fqdn = fqdn;
				to.US = us;
			}
		}
	}
}

