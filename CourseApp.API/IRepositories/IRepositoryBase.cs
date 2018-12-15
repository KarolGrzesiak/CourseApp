using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseApp.API.IRepositories
{
    public interface IRepositoryBase<T>
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task<bool> SaveAllAsync();
    }
}