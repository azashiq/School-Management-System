using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> Query();

        Task<T> AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(T entity);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<int> CountAsync();

        Task<int> SaveChangesAsync();
    }
}
