// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Language;
using Moq.Protected;

namespace Test.Unit.Mocks
{
    public class HttpClientMock : Mock<HttpMessageHandler>
    {
        private readonly List<Tuple<HttpStatusCode, HttpContent>> _responses;
        public HttpClientMock(List<Tuple<HttpStatusCode, HttpContent>> responses) : base(MockBehavior.Strict)
        {
            _responses = responses;
            SetupResponses();
        }

        private void SetupResponses()
        {
            var handlerPart = this.Protected().SetupSequence<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           );

            foreach (var item in _responses)
            {
                handlerPart = AdddReturnPart(handlerPart, item.Item1, item.Item2);
            }
        }

        private ISetupSequentialResult<Task<HttpResponseMessage>> AdddReturnPart(ISetupSequentialResult<Task<HttpResponseMessage>> handlerPart,
        HttpStatusCode statusCode, HttpContent content)
        {
            return handlerPart.ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = content
            });
        }

        public static implicit operator HttpClient (HttpClientMock mock)
        {
            // Since neither HttpClient or HttpClientMock is an interface we can use implicit operator to convert.
            // Safes us a call to mock.Object in the test code.
            return new HttpClient(mock.Object) {};
        }
    }
}