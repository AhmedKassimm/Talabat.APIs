using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        object GetAllWithSpaceAsync(EmployeeSpecifications spec);
        Task<IReadOnlyList<Product>> GetAllWithSpaceAsync(ProductSpecification spec);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IReadOnlyList<Order>> GetAllWithSpaceAsync(OrderSpecification spec);
    }
}
