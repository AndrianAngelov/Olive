 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Olive.Data.Repositories
{
    public interface IRecipesRepository : IRepository<Recipe>
    {
        IQueryable<Recipe> AllRecipesByCategoryID(long categoryID);

        IQueryable<Recipe> GetRecipesByRosiRecommendation();

        IQueryable<Recipe> GetTopRecipesByCreationDate(int nubmersOfRecipes);

        IQueryable<Recipe> GetTopRecipesByRating(int nubmersOfRecipes);

        IQueryable<Recipe> GetTopRecipesByNubmerOfHits(int nubmersOfRecipes);

        string UploadCloudinaryRecipeImage(string imageURL, string imageName);

        void DeleteCloudinaryRecipeImage(string imageName);
    }
}
