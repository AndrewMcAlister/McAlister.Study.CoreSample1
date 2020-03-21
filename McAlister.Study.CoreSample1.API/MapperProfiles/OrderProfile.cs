using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using df=McAlister.Study.CoreSample1.Definitions;
using AutoMapper;

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
