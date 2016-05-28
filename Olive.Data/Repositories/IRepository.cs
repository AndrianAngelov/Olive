namespace Olive.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T GetById(int id);

        T GetById(long id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int id);

        void Delete(long id);

        void Detach(T entity);

    }
}
