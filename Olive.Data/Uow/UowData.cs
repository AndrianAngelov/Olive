namespace Olive.Data.Uow
{
    using System;
    //using System.Data.Entity.Core;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Olive.Data;
    using Olive.Data.Repositories;
    using System.Data.Entity.Infrastructure;

    public class UowData : IUowData
    {
        private readonly DbContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UowData()
            : this(new OliveDatabaseEntities())
        {

        }

        public UowData(DbContext context)
        {
            this.context = context;
        }

        public IRecipesRepository Recipes
        {
            get { return (IRecipesRepository)this.GetRepository<Recipe>(); }
        }

        public IRepository<RecipeItem> RecipeItems
        {
            get { return this.GetRepository<RecipeItem>(); }
        }

        public ICategoriesRepository Categories
        {
            get { return (ICategoriesRepository)this.GetRepository<Category>(); }
        }

        public IRepository<Ingredient> Ingredients
        {
            get { return this.GetRepository<Ingredient>(); }
        }

        public IRepository<RecipeItems_Ingredients> RecipeItems_Ingredients
        {
            get { return this.GetRepository<RecipeItems_Ingredients>(); }
        }

        public IRepository<Source> Sources
        {
            get { return this.GetRepository<Source>(); }
        }

        public IRepository<Step> Steps
        {
            get { return this.GetRepository<Step>(); }
        }

        public IRepository<Unit> Units
        {
            get { return this.GetRepository<Unit>(); }
        }

        public IRepository<OliveAppSetting> OliveAppSettings
        {
            get { return this.GetRepository<OliveAppSetting>(); }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                if (typeof(T) == typeof(Category))
                {
                    type = typeof(CategoriesRepository);
                }
                if (typeof(T) == typeof(Recipe))
                {
                    type = typeof(RecipesRepository);
                }

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

    }
}
