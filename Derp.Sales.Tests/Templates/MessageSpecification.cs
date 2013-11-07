using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Derp.Sales.Messaging;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Templates
{
    public class MessageSpecification : TypedSpecification<MessageSpecification.Result>
    {
        public Action Before;
        public Action<IBus> Bootstrap = bus => { };


        public List<Expression<Func<Result, bool>>> Expect = new List<Expression<Func<Result, bool>>>();
        public Action Finally;

        public List<Event> Given = new List<Event>();
        public string Name;
        public Message When;

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
                messages =>
                {
                    var results = new List<Message>();
                    var bus = new Bus();
                    Bootstrap(bus);

                    bus.Subscribe(new AdHocHandler<Message>(message => results.Add(message)));

                    Exception thrownException = null;
                    try
                    {
                        bus.Publish(messages.Last());
                    }
                    catch (Exception ex)
                    {
                        thrownException = ex;
                    }
                    return new Result(results, thrownException);
                });
        }

        public IEnumerable<Expression<Func<Result, bool>>> GetAssertions()
        {
            return Expect;
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

        public class Result
        {
            private readonly IEnumerable<Message> actions;

            public Result(IEnumerable<Message> actions, Exception ex = null)
            {
                this.actions = actions;
                ThrownException = ex ?? new ExpectedExceptionDidNotOccurException(null);
            }

            public bool ThrewAnException
            {
                get { return false == ThrownException is ExpectedExceptionDidNotOccurException; }
            }

            public Exception ThrownException { get; private set; }

            public bool DoesNothing
            {
                get
                {
                    // i.e does nothing
                    return false == actions.Any();
                }
            }

            public bool Does(params Func<object>[] does)
            {
                return does.Select(f => f()).SequenceEqual(actions, SerializationEqualityComparer.Instance);
            }

            public override string ToString()
            {
                return ThrewAnException
                    ? ThrownException.Message
                    : actions.Aggregate(
                        new StringBuilder(),
                        (builder, action) => builder.Append(action).AppendLine(),
                        builder => builder.ToString());
            }
        }

        #endregion
    }
}
