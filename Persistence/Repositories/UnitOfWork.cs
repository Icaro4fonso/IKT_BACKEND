using IKT_BACKEND.Domain.Repositories;
using IKT_BACKEND.Persistence.Context;
using System;

namespace IKT_BACKEND.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext Context;

        public UnitOfWork(AppDbContext context)
        {
            Context = context;
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
