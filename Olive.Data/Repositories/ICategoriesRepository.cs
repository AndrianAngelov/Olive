using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Olive.Data.Repositories
{
    public interface ICategoriesRepository:IRepository<Category>
    {
        IQueryable<Category> AllParentCategories();

        IQueryable<Category> AllChildrenCategories();

        IQueryable<Category> GetChildrenCategoriesByParentID(int id);
    }
}
