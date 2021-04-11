// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Kaylumah.AdventuresWithMock.Access.Author.Interface
{
    public interface IAuthorAccess
    {
        Task<FilterAuthorResponse> FilterAuthors(FilterAuthorCriteria filterAuthorCriteria = null);

    }
}
