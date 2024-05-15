using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly StoreContext context;

        private Hashtable _Repositories;
        public UnitOfWork(StoreContext Context)
        {
            context = Context;
           _Repositories = new Hashtable();
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var types= typeof(TEntity).Name;
            if(!_Repositories.ContainsKey(types))
            {
                var repository = new GenericRepository<TEntity>(context);
                _Repositories.Add(types, repository);
            }
            return _Repositories[types] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
        => await context.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
        => await context.DisposeAsync();

        
    }
}
