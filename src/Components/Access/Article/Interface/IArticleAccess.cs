// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Kaylumah.AdventuresWithMock.Access.Article.Interface
{
    public interface IArticleAccess
    {
        Task<FilterArticleResponse> FilterArticles(FilterArticleCriteria filterArticleCriteria = null);
        Task<CreateArticlesResponse> CreateArticles(CreateArticlesRequest createArticlesRequest);
        Task DeleteArticles(DeleteArticlesRequest deleteArticlesRequest);
    }
}
