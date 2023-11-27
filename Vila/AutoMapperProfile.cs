using AutoMapper;
using System.Linq;
using Vila.Data.DTO;
using Vila.Data.Entities;

namespace Vila
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UsersEO, UsersDTO>();
            CreateMap<UsersDTO, UsersEO>();

            CreateMap<VillaDetailsEO, VillaDetailsDTO>();
            CreateMap<VillaDetailsDTO, VillaDetailsEO>();

            //   CreateMap<(VillaDetailsEO, VillaDescriptionEO), VillaDetailsAndDescriptionDTO>();
            //    CreateMap<VillaDetailsAndDescriptionDTO, (VillaDetailsEO, VillaDescriptionEO)>();
            // המרה מפורשת בשש שורות למטה כי לפעמים ההמרה בשתי שורות למעלה לא עובדת
            CreateMap<(VillaDetailsEO, VillaDescriptionEO, CheckListVilaEO), VillaDetailsAndDescriptionDTO>()
                .ForMember(dest => dest.VillaDetails, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.VillaDescription, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.CheckListVila, opt => opt.MapFrom(src => src.Item3));

            CreateMap<VillaDetailsAndDescriptionDTO, (VillaDetailsEO, VillaDescriptionEO, CheckListVilaEO)>()
                .ForMember(dest => dest.Item1, opt => opt.MapFrom(src => src.VillaDetails))
                .ForMember(dest => dest.Item2, opt => opt.MapFrom(src => src.VillaDescription))
                .ForMember(dest => dest.Item3, opt => opt.MapFrom(src => src.CheckListVila));





        }
    }
}
