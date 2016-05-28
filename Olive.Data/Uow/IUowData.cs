namespace Olive.Data.Uow
{
    using System;
    using Olive.Data.Repositories;

    public interface IUowData : IDisposable
    {
        IRecipesRepository Recipes { get; }

        IRepository<RecipeItem> RecipeItems { get; }

        ICategoriesRepository Categories { get; }

        IRepository<Ingredient> Ingredients { get; }

        IRepository<RecipeItems_Ingredients> RecipeItems_Ingredients { get; }

        IRepository<Source> Sources { get; }

        IRepository<Step> Steps { get; }

        IRepository<Unit> Units { get; }

        IRepository<OliveAppSetting> OliveAppSettings { get; }

        int SaveChanges();
    }
}
