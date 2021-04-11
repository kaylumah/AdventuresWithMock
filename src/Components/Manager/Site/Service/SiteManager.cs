// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;
using Kaylumah.AdventuresWithMock.Access.Article.Interface;
using Kaylumah.AdventuresWithMock.Access.Author.Interface;
using Kaylumah.AdventuresWithMock.Manager.Site.Interface;
using Microsoft.Extensions.Logging;

namespace Kaylumah.AdventuresWithMock.Manager.Site.Service
{
    public class SiteManager : ISiteManager
    {

        private readonly IArticleAccess _articleAccess;
        private readonly IAuthorAccess _authorAccess;
        private readonly ILogger _logger;

        public SiteManager(IArticleAccess articleAccess, IAuthorAccess authorAccess, ILogger<SiteManager> logger)
        {
            _articleAccess = articleAccess;
            _authorAccess = authorAccess;
            _logger = logger;
        }

        public async Task CreateArticle(Interface.CreateArticleRequest createArticleRequest)
        {
            // Hardcoded for now, would probably come from JWT user claim.
            var authorId = 666;

            var authorsResponse = await _authorAccess.FilterAuthors(new FilterAuthorCriteria {
                AuthorIds = new int[] { authorId }
            });

            var author = authorsResponse.Authors.SingleOrDefault(x => x.Id.Equals(authorId));

            if (author == null)
            {
                _logger.LogWarning($"No author found for {authorId}");
                return;
            }

            if (!author.Verfied)
            {
                _logger.LogAuthorNotVerfied(authorId);
                return;
            }

            var article = new Access.Article.Interface.CreateArticleRequest
            { 
                AuthorId = authorId,
                Title = createArticleRequest.Title,
                Description = createArticleRequest.Content
            };

            var response = await _articleAccess.CreateArticles(new CreateArticlesRequest {
                CreateArticleRequests = new Access.Article.Interface.CreateArticleRequest[] {
                    article
                }
            });
        }
    }
}
