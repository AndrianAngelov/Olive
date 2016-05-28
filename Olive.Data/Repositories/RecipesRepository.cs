using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using CloudinaryDotNet;
using System.Configuration;
using CloudinaryDotNet.Actions;
using System.IO;

namespace Olive.Data.Repositories
{
    public class RecipesRepository : GenericRepository<Recipe>, IRecipesRepository
    {
        public RecipesRepository(DbContext dbContext)
            : base(dbContext)
        {

        }

        public IQueryable<Recipe> GetRecipesByRosiRecommendation()
        {
            return this.DbSet.Where(recipe => recipe.Recommended == true).OrderByDescending(x => x.Date);
        }

        public IQueryable<Recipe> GetTopRecipesByCreationDate(int nubmersOfRecipes)
        {
            return this.DbSet.OrderBy(x => x.Date).Take(nubmersOfRecipes);
        }

        public IQueryable<Recipe> GetTopRecipesByRating(int nubmersOfRecipes)
        {
            return this.DbSet.OrderByDescending(x => x.Rating).Take(nubmersOfRecipes);
        }

        public IQueryable<Recipe> GetTopRecipesByNubmerOfHits(int nubmersOfRecipes)
        {
            return this.DbSet.OrderByDescending(x => x.NumberOfHits).Take(nubmersOfRecipes);
        }

        public IQueryable<Recipe> AllRecipesByCategoryID(long categoryID)
        {
            return this.DbSet.Where(p => p.Categories.Any(c => c.CategoryID == categoryID || c.Category1.CategoryID == categoryID));
        }

        public IQueryable<Recipe> AllRecipesByCategoryID(int categoryID)
        {
            return this.DbSet.Where(p => p.Categories.Any(c => c.CategoryID == categoryID || c.Category1.CategoryID == categoryID)); 
        }

        
        static string cloudinaryAccount = ConfigurationManager.AppSettings["CLOUDINARY_URL"];
        static string cloudinaryStoreURL = ConfigurationManager.AppSettings["AccountsStoreURL"];
        static string cloudinaryAccountsFolder = ConfigurationManager.AppSettings["AccountsFolderName"];
        
        public string UploadCloudinaryRecipeImage(string imageURL, string imageName)
        {
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(cloudinaryAccount); 
            string recipeImageVersion = null;
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(String.Format(@"{0}", imageURL)),
                PublicId = Path.GetFileNameWithoutExtension(imageName),
                Invalidate = true
            };

            var uploadResult = cloudinary.Upload(uploadParams);
            recipeImageVersion = "v" + uploadResult.Version;
            return recipeImageVersion;
        }

        public void DeleteCloudinaryRecipeImage(string imageName)
        {
            CloudinaryDotNet.Cloudinary cloudinaryDelete = new CloudinaryDotNet.Cloudinary(cloudinaryAccount);

            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { Path.GetFileNameWithoutExtension(imageName) },
                Invalidate = true
            };
            DelResResult result = cloudinaryDelete.DeleteResources(delParams);
        }

    }
}
