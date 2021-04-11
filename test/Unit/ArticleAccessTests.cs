// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kaylumah.AdventuresWithMock.Access.Article.Interface;
using Kaylumah.AdventuresWithMock.Access.Article.Service;
using Test.Unit.Mocks;
using Xunit;

namespace Test.Unit
{
    public class ArticleAccessTests
    {
        // [Fact]
        // public async Task Test1()
        // {
        //     var httpClient = new HttpClient();
        //     var articleAccess = new ArticleAccess(httpClient);
        //     await articleAccess.CreateArticles(new CreateArticlesRequest{
        //         CreateArticleRequests = new CreateArticleRequest[] {
        //             new CreateArticleRequest {
        //                 AuthorId = 666,
        //                 Description = "...",
        //                 Title = "Demo"
        //             }
        //         }
        //     });
        // }

        [Fact]
        public async Task Test_ArticleAccess_Returns200OK()
        {
            var createArticleResponse = new StringContent("{ 'id':'anId' }", Encoding.UTF8, "application/json");
            var httpClient = new HttpClientMock(new List<Tuple<HttpStatusCode, HttpContent>> {
                new Tuple<HttpStatusCode, HttpContent>(HttpStatusCode.OK, createArticleResponse),
            });
            var articleAccess = new ArticleAccess(httpClient);
            await articleAccess.CreateArticles(new CreateArticlesRequest{
                CreateArticleRequests = new CreateArticleRequest[] {
                    new CreateArticleRequest {
                        AuthorId = 666,
                        Description = "...",
                        Title = "Demo"
                    }
                }
            });
        }
    }
}