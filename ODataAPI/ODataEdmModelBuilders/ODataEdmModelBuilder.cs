using BusinessObjects;
using FUDTOs;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataAPI.ODataEdmModelBuilders
{
    public static class ODataEdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var newsArticles = builder.EntitySet<NewsArticleDTO>("newsArticles");
            newsArticles.EntityType.HasKey(x => x.NewsArticleId);
            var systemAccount = builder.EntitySet<SystemAccountDTO>("systemAccount");
            systemAccount.EntityType.HasKey(x => x.AccountId);
            var category = builder.EntitySet<CategoryDTO>("category");
            category.EntityType.HasKey(x => x.CategoryId);
            return builder.GetEdmModel();
        }
    }
}
