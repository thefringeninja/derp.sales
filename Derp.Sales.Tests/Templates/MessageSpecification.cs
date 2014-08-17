using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Derp.Sales.Messaging;
using Derp.Sales.Tests.Fixtures;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Templates
{
    public class MessageSpecification : TypedSpecification<MessageSpecification.Result>, HasMessages
    {
        public Action Before;
        public Action<IBus> Bootstrap = bus => { };


        public List<Expression<Func<Result, bool>>> Assertions = new List<Expression<Func<Result, bool>>>();
        public Action Finally;

        public readonly List<Event> Given = new List<Event>();
        public string Name;
        public Message When;

        public readonly List<Event> Expect = new List<Event>();

        #region TypedSpecification<Result> Members

        public string GetName()
        {
            return Name;
        }

        public Action GetBefore()
        {
            return Before;
        }

        public Delegate GetOn()
        {
            var messages = new List<Message>(Given)
            {
                When
            };
            return new Func<Messages>(() => new Messages(messages));
        }

        public Delegate GetWhen()
        {
            return new Func<Messages, Result>(
                input =>
                {
                    var output = new List<Message>();
                    var bus = new Bus();
                    Bootstrap(bus);

                    bus.Subscribe(new AdHocHandler<Message>(message => output.Add(message)));

                    Exception thrownException = null;
                    try
                    {
                        foreach (var message in input)
                        {
                            bus.Publish(message).Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        thrownException = ex;
                    }
                    return new Result(output.Except(input), thrownException);
                });
        }

        public IEnumerable<Expression<Func<Result, bool>>> GetAssertions()
        {
            return new List<Expression<Func<Result, bool>>>(Assertions)
            {
                result => result.EventsMatched(this)
            };
        }

        public Action GetFinally()
        {
            return Finally;
        }

        #endregion

        #region Nested type: Messages

        private class Messages : IEnumerable<Message>
        {
            private readonly IList<Message> list;

            public Messages(IEnumerable<Message> source)
            {
                list = new List<Message>(source);
            }

            #region IEnumerable<Message> Members

            public IEnumerator<Message> GetEnumerator()
            {
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            public override string ToString()
            {
                return this.Aggregate(
                    new StringBuilder(),
                    (builder, x) => builder.AppendLine(x.ToString()),
                    builder => builder.ToString());
            }
        }

        #endregion

        #region Nested type: NoExceptionWasThrownException

        public class NoExceptionWasThrownException : Exception
        {
        }

        #endregion

        #region Nested type: Result

        public class Result : HasMessages
        {
            public readonly IEnumerable<Message> Actions;

            public Result(IEnumerable<Message> actions, Exception ex = null)
            {
                this.Actions = actions;
                if (ex is AggregateException)
                {
                    ex = ex.GetBaseException();
                }
                ThrownException = ex ?? new ExpectedExceptionDidNotOccurException(null);
            }

            public bool ThrewAnException
            {
                get { return false == ThrownException is ExpectedExceptionDidNotOccurException; }
            }

            public Exception ThrownException { get; private set; }

            public bool DidNotChangeAnything()
            {
                return false == Actions.Any();
            }

            public override string ToString()
            {
                return ThrewAnException
                    ? ThrownException.Message
                    : Actions.Aggregate(
                        new StringBuilder(),
                        (builder, action) => builder.Append(action).AppendLine(),
                        builder => builder.ToString());
            }

            IEnumerable<Message> HasMessages.Messages
            {
                get { return Actions; }
            }
        }

        #endregion

        IEnumerable<Message> HasMessages.Messages
        {
            get { return Expect; }
        }
    }
}