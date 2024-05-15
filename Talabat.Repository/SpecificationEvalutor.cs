using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public class SpecificationEvalutor<TEntity> where TEntity: BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query = inputQuery;// context.products

            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);// p=>p.Id ==id
            // context.products.where(p=>p.ProductBrandId==brandId);
            // context.products.where(p=>p.ProductTypeId==typeId);
            // context.products.where(p=>p.ProductBrandId==brandId && p.ProductTypeId==typeId);
            // context.products.where(p=>p.ProductBrandId==brandId && true)
            // context.products.where(p=>p.Name.ToLower().Contains("angular") && true && true);


            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            //context.products.OrderBy(P => P.Name)
            //context.products.where(p=>p.ProductBrandId==brandId && true).OrderBy(P => P.Name)

            if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);
            //context.products.OrderByDescending(P => P.Price)
            // context.products.where(p=>p.ProductBrandId==brandId && p.ProductTypeId==typeId).OrderByDescending;


            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);
            // context.products.OrderByDescending(p => p.Price);
            // context.products.where(p => p.ProductBrandId == brandId &&p.ProductTypeId==typeId).OrderByDescending(p => p.Price);
            // context.products.where(p=>p.Name.ToLower().Contains("angular") && true&&true).Skip(0).Take(5);

            query = spec.Includes.Aggregate(query,(currentQuery,IncludeExpression) => currentQuery.Include(IncludeExpression));
            // context.products.where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType);
            //context.products.OrderBy(P => P.Name).Include(p => p.ProductBrand).Include(p => p.ProductType);
            //context.products.OrderByDescending(P => P.Price).Include(p => p.ProductBrand).Include(p => p.ProductType);
            //context.products.where(p=>p.ProductBrandId==brandId && true).Include(p => p.ProductBrand).Include(p => p.ProductType);
            //context.products.where(p=>true && p.ProductTypeId==typeId ).Include(p => p.ProductBrand).Include(p => p.ProductType);
            //context.products.where(p=>p.ProductBrandId==brandId &&  p.ProductType==typeId).OrderByDescending(p=>p.price).Include(p => p.ProductBrand).Include(p => p.ProductType);
            //context.products.where(p=> true && true).Skip(0).Take(5).Include(p=>p.ProductBrand).Include(p=>p.ProductType)
            //context.products.where(p=> p.ProductBrandId==brandId &&p.ProductType==typeId).OrderByDescending(p=>p.price).Skip(6).Take(3).Incloude(p => p.ProductBrand).Incloude(p => p.ProductType);
            // context.products.where(p=>p.Name.ToLower().Contains("angular") && true&&true).Skip(0).Take(5).Incloude(p => p.ProductBrand).Incloude(p => p.ProductType);

            return query;
        }
    }
}
