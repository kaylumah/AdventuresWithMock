// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Kaylumah.AdventuresWithMock.Access.Author.Interface;
using Moq;

namespace Test.Unit.Mocks
{
    public class AuthorAccessMock : Mock<IAuthorAccess>
    {
        public List<Author> Authors { get; }
        public AuthorAccessMock(List<Author> authors)
        {
            Authors = authors;

            Setup(x => x.FilterAuthors(It.IsAny<FilterAuthorCriteria>()))
                .ReturnsAsync((FilterAuthorCriteria criteria) => {

                    IQueryable<Author> result = Authors.AsQueryable();
                    if (criteria != null)
                    {
                        result = result.Where(x => criteria.AuthorIds.Contains(x.Id));
                    }

                    return new FilterAuthorResponse {
                        Authors = result.ToArray()
                    };
                });
        }
    }
}