using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;


namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                      .ForMember(PD => PD.ProductBrand, O => O.MapFrom(P => P.ProductBrand.Name))
                      .ForMember(PD => PD.ProductType, O => O.MapFrom(P => P.ProductType.Name))
                      .ForMember(PD => PD.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto,Talabat.Core.Entities.Order_Aggregation. Address>();
        }
    }
}
