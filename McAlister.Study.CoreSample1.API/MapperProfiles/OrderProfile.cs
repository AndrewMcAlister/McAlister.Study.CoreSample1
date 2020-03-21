using AutoMapper;
using df = McAlister.Study.CoreSample1.Definitions;

namespace McAlister.Study.CoreSample1.MapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<df.Entities.Orders, df.Models.Order>();
            CreateMap<df.Models.Order, df.Entities.Orders>();
        }
    }
}
