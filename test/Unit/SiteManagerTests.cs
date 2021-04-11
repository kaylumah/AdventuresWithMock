// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.
using System;
using System.Threading.Tasks;
using Kaylumah.AdventuresWithMock.Access.Article.Interface;
using Kaylumah.AdventuresWithMock.Access.Author.Interface;
using Kaylumah.AdventuresWithMock.Manager.Site.Interface;
using Kaylumah.AdventuresWithMock.Manager.Site.Service;
using Moq;
using Xunit;
using System.Linq;
using Test.Unit.Mocks;
using System.Collections.Generic;

namespace Test.Unit
{
    public class SiteManagerTests
    {
        [Fact]
        public async Task Test_SiteManager_CreateArticle_Traditionally()
        {
            // Arange
            var loggerMock = new LoggerMock<SiteManager>();
            var authorAccessMock = new Mock<IAuthorAccess>();
            authorAccessMock.Setup(x => x.FilterAuthors(It.Is<FilterAuthorCriteria>(p => p.AuthorIds.Contains(666)))).ReturnsAsync(new FilterAuthorResponse {
                Authors = new Author[] {
                    new Author {
                        Id = 666,
                        DisplayName = "Max",
                        Verfied = true
                    }
                }
            });
            var articleAccessMock = new Mock<IArticleAccess>();
            articleAccessMock.Setup(x => x.CreateArticles(It.IsAny<CreateArticlesRequest>())).ReturnsAsync(new CreateArticlesResponse {
                Articles = new Article[] {
                    new Article {
                        Id = 1,
                        AuthorId = 666,
                        Title = "...",
                        Description = "..."
                    }
                }
            });
            ISiteManager sut = new SiteManager(articleAccessMock.Object, authorAccessMock.Object, loggerMock.Object);

            // Act
            var request = new Kaylumah.AdventuresWithMock.Manager.Site.Interface.CreateArticleRequest { 
                Title = "Pretty Title",
                Content = "# AdventuresWithMock ..."
            };
            await sut.CreateArticle(request);

            // Assert
            authorAccessMock.Verify(x => x.FilterAuthors(It.IsAny<FilterAuthorCriteria>()), Times.Once);
            articleAccessMock.Verify(x => x.CreateArticles(It.IsAny<CreateArticlesRequest>()), Times.Once);
        }

        [Fact]
        public async Task Test_SiteManager_CreateArticle_RepoMocksDemo1()
        {
            // Arange
            var loggerMock = new LoggerMock<SiteManager>();
            var authorAccessMock = new AuthorAccessMock(new List<Author> {
                new Author { Id = 666, DisplayName = "Max", Verfied = false }
            });
            var articleAccessMock = new ArticleAccessMock();
            ISiteManager sut = new SiteManager(articleAccessMock.Object, authorAccessMock.Object, loggerMock.Object);

            // Act
            var request = new Kaylumah.AdventuresWithMock.Manager.Site.Interface.CreateArticleRequest
            {
                Title = "Pretty Title",
                Content = "# AdventuresWithMock ..."
            };
            await sut.CreateArticle(request);

            // Assert
            authorAccessMock.Verify(x => x.FilterAuthors(It.IsAny<FilterAuthorCriteria>()), Times.Once);
            articleAccessMock.Verify(x => x.CreateArticles(It.IsAny<CreateArticlesRequest>()), Times.Never);
        }

        [Fact]
        public async Task Test_SiteManager_CreateArticle_TestLogging()
        {
            // Arange
            var loggerMock = new LoggerMock<SiteManager>();
            var authorAccessMock = new AuthorAccessMock(new List<Author> {});
            var articleAccessMock = new ArticleAccessMock();
            ISiteManager sut = new SiteManager(articleAccessMock.Object, authorAccessMock.Object, loggerMock.Object);

            // Act
            var request = new Kaylumah.AdventuresWithMock.Manager.Site.Interface.CreateArticleRequest
            {
                Title = "Pretty Title",
                Content = "# AdventuresWithMock ..."
            };
            await sut.CreateArticle(request);

            // Assert
            authorAccessMock.Verify(x => x.FilterAuthors(It.IsAny<FilterAuthorCriteria>()), Times.Once);
            articleAccessMock.Verify(x => x.CreateArticles(It.IsAny<CreateArticlesRequest>()), Times.Never);
            loggerMock.VerifyLogging("No author found for 666", Microsoft.Extensions.Logging.LogLevel.Warning);
        }

        [Fact]
        public async Task Test_SiteManager_CreateArticle_TestLoggingExtensionMethod()
        {
            // Arange
            var loggerMock = new LoggerMock<SiteManager>()
                .SetupLogLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            var authorAccessMock = new AuthorAccessMock(new List<Author> {
                new Author { Id = 666, DisplayName = "Max", Verfied = false }
            });
            var articleAccessMock = new ArticleAccessMock();
            ISiteManager sut = new SiteManager(articleAccessMock.Object, authorAccessMock.Object, loggerMock.Object);

            // Act
            var request = new Kaylumah.AdventuresWithMock.Manager.Site.Interface.CreateArticleRequest
            {
                Title = "Pretty Title",
                Content = "# AdventuresWithMock ..."
            };
            await sut.CreateArticle(request);

            // Assert
            authorAccessMock.Verify(x => x.FilterAuthors(It.IsAny<FilterAuthorCriteria>()), Times.Once);
            articleAccessMock.Verify(x => x.CreateArticles(It.IsAny<CreateArticlesRequest>()), Times.Never);
            loggerMock.VerifyLogging("Author with Id 666 is not verfied!", Microsoft.Extensions.Logging.LogLevel.Information);
            loggerMock.VerifyEventIdWasCalled(new Microsoft.Extensions.Logging.EventId(100, "AuthorNotVerfied"));
        }
    }
}
