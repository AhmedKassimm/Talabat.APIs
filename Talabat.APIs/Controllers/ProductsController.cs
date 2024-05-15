using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data.Configurations;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        //[Authorize]
        [CachedAttribute(300)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);
            var Products = await unitOfWork.Repository<Product>().GetAllWithSpaceAsync(spec);
            
            var data = mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSec = new ProductWithFilterationForCountSpecification(productSpec);
            var count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSec);
            return Ok(new Pagination<ProductToReturnDto>(productSpec.PageIndex,productSpec.PageSize,count,data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductSpecification();
            var Product = await unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (Product is null) return NotFound(new ApiErrorResponse(404));
            var MappedProduct = mapper.Map<Product, ProductToReturnDto>(Product);

            return Ok(MappedProduct);
        }
        [HttpGet("brands")]//api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var Brands =await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }

        [HttpGet("types")]//api/products/types
        public async Task<ActionResult<ProductType>> GetAllTypes()
        {
            var Types = await unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }

    }
}
