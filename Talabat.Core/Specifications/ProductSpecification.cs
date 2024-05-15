using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecification:BaseSpecification<Product>
    {//where(P => P.ProductBrandId == brandId && P.ProductTypeId == typeId)
     //where(P=>true && true)
     //where(P=>P.ProductBrandId==brandId && true)
     //where(P=> true && P.ProductTypeId == typeId)
        public ProductSpecification()////
        {
        }
        public ProductSpecification(ProductSpecParams productSpec)//get
            :base(P =>
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search)) &&
            (!productSpec.BrandId.HasValue || P.ProductBrandId == productSpec.BrandId) &&
            (!productSpec.TypeId.HasValue || P.ProductTypeId == productSpec.TypeId))
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
                //totalProduct=100;
                //pagesize=10;
                //pageIndex=3
                ApplyPagination(productSpec.PageSize*(productSpec.PageIndex-1),productSpec.PageSize);
            }
        
        public ProductSpecification(int id):base(P=>P.Id == id)//get by id
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }

    }
}
