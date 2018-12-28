using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CourseApp.API.IRepositories;

namespace CourseApp.API.Data
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DataContext _context;

        public RepositoryBase(DataContext context)
        {
            _context = context;

        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }





        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}