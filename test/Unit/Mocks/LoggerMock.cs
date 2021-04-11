// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Unit.Mocks
{
    public class LoggerMock<T> : Mock<ILogger<T>>
    {
        public class LogMessageMock
        {
            public LogLevel LogLevel { get;set; }
            public EventId Event { get;set; }
            public string Message { get;set; }
        }

        public List<LogMessageMock> Messsages { get; } = new List<LogMessageMock>();

        public LoggerMock()
        {
            Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                )
            )
            .Callback(new InvocationAction(invocation =>
            {
                // https://stackoverflow.com/questions/52707702/how-do-you-mock-ilogger-loginformation
                // https://github.com/moq/moq4/issues/918
                var logLevel = (LogLevel)invocation.Arguments[0];
                var eventId = (EventId)invocation.Arguments[1];
                var state = invocation.Arguments[2];
                var exception = (Exception?)invocation.Arguments[3];
                var formatter = invocation.Arguments[4];

                var invokeMethod = formatter
                    .GetType()
                    .GetMethod("Invoke");

                var logMessage = (string?)invokeMethod?.Invoke(formatter, new[] { state, exception });
                Messsages.Add(new LogMessageMock {
                    Event = eventId,
                    LogLevel = logLevel,
                    Message = logMessage
                });
            }));
        }

        public LoggerMock<T> SetupLogLevel(LogLevel logLevel, bool enabled = true)
        {
            Setup(x => x.IsEnabled(It.Is<LogLevel>(p => p.Equals(logLevel))))
                .Returns(enabled);
            return this;
        }
    }
}