// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kaylumah.AdventuresWithMock.Access.Article.Interface;
using Test.Unit.Mocks;
using Xunit;

namespace Test.Unit
{
    public class ArticleAccessMockTests
    {
        [Fact]
        public async Task Test_ArticleAccessMock_StatefullDemo1()
        {
            // Arange
            var articleAccessMock = new ArticleAccessMock()
                .SetupFilterArticles(new List<Article> {});
            var sut = articleAccessMock.Object;

            // Act
            var initialResponse = await sut.FilterArticles();
            var createResponse = await sut.CreateArticles(new CreateArticlesRequest {
                CreateArticleRequests = new CreateArticleRequest[] {
                    new CreateArticleRequest {
                        AuthorId = 666,
                        Description = "1",
                        Title = "1"
                    },
                    new CreateArticleRequest {
                        AuthorId = 666,
                        Description = "2",
                        Title = "2"
                    }
                }
            });

            var afterAddResponse = await sut.FilterArticles();

            await sut.DeleteArticles(new DeleteArticlesRequest {
                DeleteArticleRequests = new DeleteArticleRequest[] {
                    new DeleteArticleRequest {
                        ArticleId = createResponse.Articles.First().Id
                    }
                }
            });

            var afterRemoveResponse = await sut.FilterArticles();


            // Assert
            initialResponse.Should().NotBeNull();
            initialResponse.Articles.Count().Should().Be(0, "No articles initially");

            afterAddResponse.Should().NotBeNull();
            afterAddResponse.Articles.Count().Should().Be(2, "We created two articles");

            afterRemoveResponse.Should().NotBeNull();
            afterRemoveResponse.Articles.Count().Should().Be(1, "There is only one article left");

            // Verify result with predicate logic instead if Mock.Verify()
            articleAccessMock.VerifyArticles(articles => articles.Count(x => x.Removed) == 1).Should().BeTrue();
        }
    }
}