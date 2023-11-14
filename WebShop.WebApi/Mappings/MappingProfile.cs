﻿using AutoMapper;
using WebShop.DTO;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<UserDTO, User>();

            CreateMap<Currency, CurrencyDTO>();

            CreateMap<SubscriptionPlan, SubscriptionPlanDTO>();
        }
    }
}
