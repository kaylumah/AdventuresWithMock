// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Unit.Mocks
{
    public static class LoggerExtensions
    {
        public static Mock<ILogger<T>> VerifyLogging<T>(this Mock<ILogger<T>> logger, string expectedMessage, LogLevel expectedLogLevel = LogLevel.Debug, Times? times = null)
        {
            times ??= Times.Once();

            Func<object, Type, bool> state = (v, t) => v.ToString().CompareTo(expectedMessage) == 0;

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == expectedLogLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), (Times)times);

            return logger;
        }

        public static Mock<ILogger<T>> VerifyEventIdWasCalled<T>(this Mock<ILogger<T>> logger, EventId eventId, Times? times = null)
        {
            times ??= Times.Once();

            logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    eventId,
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), times.Value);

            return logger;
        }
    }
}