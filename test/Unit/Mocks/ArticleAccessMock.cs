// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Kaylumah.AdventuresWithMock.Access.Article.Interface;
using Moq;

namespace Test.Unit.Mocks
{
    public class ArticleAccessMock : Mock<IArticleAccess>
    {
        public class ArticleMock
        {
            public int Id { get;set; }
            public int AuthorId { get;set; }
            public string Title { get;set; }
            public string Content { get;set; }
            public bool Removed { get;set; }
        }

        public List<CreateArticlesRequest> CreateArticlesRequests { get; } = new List<CreateArticlesRequest>();
        public List<DeleteArticlesRequest> DeleteArticlesRequests { get; } = new List<DeleteArticlesRequest>();

        private List<ArticleMock> _articleState = new List<ArticleMock>();
        private int _numberOfArticlesBeforeCreate = 0;

        public ArticleAccessMock()
        {
            Setup(access => access.CreateArticles(It.IsAny<CreateArticlesRequest>()))
                .Callback<CreateArticlesRequest>(request => {
                    CreateArticlesRequests.Add(request);
                    _numberOfArticlesBeforeCreate = _articleState.Count;
                    var nextId = _numberOfArticlesBeforeCreate + 1;
                    foreach(var createArticleRequest in request.CreateArticleRequests)
                    {
                        _articleState.Add(new ArticleMock {
                            Id = nextId,
                            AuthorId = createArticleRequest.AuthorId,
                            Content = createArticleRequest.Description,
                            Title = createArticleRequest.Title,
                            Removed = false
                        });
                        nextId++;
                    }
                })
                .ReturnsAsync(() => new CreateArticlesResponse {
                    Articles = _articleState
                    .Skip(_numberOfArticlesBeforeCreate)
                    .Select(x => new Article
                    {
                        Id = x.Id,
                        AuthorId = x.AuthorId,
                        Description = x.Content,
                        Title = x.Title
                    })
                    .ToArray()
                });
            
            Setup(access => access.DeleteArticles(It.IsAny<DeleteArticlesRequest>()))
                .Callback<DeleteArticlesRequest>(deleteArticlesRequest => {
                    DeleteArticlesRequests.Add(deleteArticlesRequest);
                    foreach(var deleteArticleRequests in deleteArticlesRequest.DeleteArticleRequests)
                    {
                        var existing = _articleState.SingleOrDefault(article => deleteArticleRequests.ArticleId == article.Id);
                        if (existing != null)
                        {
                            existing.Removed = true;
                        }
                    }
                });
        }

        // public ArticleAccessMock MakeStatefull(List<Article> articles)
        // {
        //     return this
        //         .SetupFilterArticles(articles)
        //         .SetupDeleteArticles()
        //         .SetupCreateArticles();
        // }

        // public ArticleAccessMock SetupDeleteArticles() { /* ... */ }
        // public ArticleAccessMock SetupCreateArticles() { /* ... */ }

        public ArticleAccessMock SetupFilterArticles(List<Article> articles)
        {
            _articleState = articles.Select(x => new ArticleMock {
                Id = x.Id,
                AuthorId = x.AuthorId,
                Content = x.Description,
                Title = x.Title,
                Removed = false
            }).ToList();

            Setup(x => x.FilterArticles(It.IsAny<FilterArticleCriteria>()))
                .ReturnsAsync((FilterArticleCriteria criteria) => {
                    IQueryable<ArticleMock> result = _articleState.AsQueryable();
                    if (criteria != null)
                    {
                        result = result.Where(x => criteria.ArticleIds.Contains(x.Id));
                    }
                    return new FilterArticleResponse {
                        Articles = result
                            .Where(x => !x.Removed)
                            .Select(x => new Article {
                                Id = x.Id,
                                AuthorId = x.AuthorId,
                                Description = x.Content,
                                Title = x.Title
                            })
                            .ToArray()
                    };
                });

            return this;
        }

        public bool VerifyArticles(Func<List<ArticleMock>, bool> predicate)
        {
           return predicate(_articleState);
        }
    }
}