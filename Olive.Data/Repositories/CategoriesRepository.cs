using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Olive.Data.Repositories
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(DbContext dbContext)
            : base(dbContext)
        {

        }

        public IQueryable<Category> AllParentCategories()
        {
            return this.DbSet.Where(c => c.ParentCategoryID == null).AsQueryable();
        }

        public IQueryable<Category> AllChildrenCategories()
        {
            return this.DbSet.Where(c => c.ParentCategoryID != null).AsQueryable();
        }

        public IQueryable<Category> GetChildrenCategoriesByParentID(int id)
        {
            return this.DbSet.Where(c => c.ParentCategoryID == id).AsQueryable();
        }
    }
}
