// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Kaylumah.AdventuresWithMock.Access.Article.Interface;

namespace Kaylumah.AdventuresWithMock.Access.Article.Service
{
    public class ArticleAccess : IArticleAccess
    {
        private readonly HttpClient _httpClient;
        
        public ArticleAccess(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateArticlesResponse> CreateArticles(CreateArticlesRequest createArticlesRequest)
        {
            // NOTE: not going to call them in a loop, just for demo purposes.
            var json = JsonSerializer.Serialize(createArticlesRequest.CreateArticleRequests.First());
            var response = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", new StringContent(json));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went horribly wrong!");
            }
            var responseText = await response.Content.ReadAsStringAsync();
            // Map it to response
            return new CreateArticlesResponse {};
        }

        public Task DeleteArticles(DeleteArticlesRequest deleteArticlesRequest)
        {
            throw new NotImplementedException();
        }

        public Task<FilterArticleResponse> FilterArticles(FilterArticleCriteria filterArticleCriteria = null)
        {
            throw new NotImplementedException();
        }
    }
}
