using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Order_Spec;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext dbContext;

        public GenericRepository(StoreContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        #region Static Quires
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //if (typeof(T) == typeof(Product))//Specification pattern => product -> brand type => Employee => Department
            //return T await dbContext.Products.Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync();
            return await dbContext.Set<T>().FindAsync(id);
        }
        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), spec);
        }

        public object GetAllWithSpaceAsync(EmployeeSpecifications spec)//
        {
            throw new NotImplementedException();
        }
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public Task<IReadOnlyList<Product>> GetAllWithSpaceAsync(ProductSpecification spec)//
        {
            throw new NotImplementedException();
        }

        public async Task Add(T entity)
        
          => await dbContext.Set<T>().AddAsync(entity);
        

        public  void Update(T entity)
        =>  dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        
            =>dbContext.Set<T>().Remove(entity);

        public Task<IReadOnlyList<Order>> GetAllWithSpaceAsync(OrderSpecification spec)
        {
            throw new NotImplementedException();
        }
    }
}
