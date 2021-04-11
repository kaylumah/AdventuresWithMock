// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Kaylumah.AdventuresWithMock.Access.Author.Interface;

namespace Kaylumah.AdventuresWithMock.Access.Author.Service
{
    public class AuthorAccess : IAuthorAccess
    {
        public Task<FilterAuthorResponse> FilterAuthors(FilterAuthorCriteria filterAuthorCriteria = null)
        {
            throw new NotImplementedException();
        }
    }
}
