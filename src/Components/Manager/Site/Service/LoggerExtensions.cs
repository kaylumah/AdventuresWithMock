// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace Kaylumah.AdventuresWithMock.Manager.Site.Service
{
    public static class LoggerExtensions
    {
        private static readonly Action<ILogger, int, Exception> _authorNotVerfied =
            LoggerMessage.Define<int>(
                LogLevel.Information,
                EventIds.AuthorNotVerfied,
                "Author with Id {AuthorId} is not verfied!"
            );

        public static void LogAuthorNotVerfied(this ILogger logger, int authorId)
        {
            _authorNotVerfied(logger, authorId, null);
        }

        private static class EventIds
        {
            public static readonly EventId AuthorNotVerfied = new(100, nameof(AuthorNotVerfied));
        }
    }
}