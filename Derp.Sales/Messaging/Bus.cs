using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class Bus : IBus
    {
        private readonly IDictionary<Type, Multiplexor<Message>> byTypePublisher =
            new Dictionary<Type, Multiplexor<Message>>();

        #region Handles<Message> Members

        public Task Handle(Message message)
        {
            return Publish(message);
        }

        #endregion

        #region IBus Members

        public Task Send<T>(T message) where T : Command
        {
            Multiplexor<Message> _;
            if (false == byTypePublisher.TryGetValue(typeof (T), out _) || _.Count != 1)
            {
                throw new ArgumentException(string.Format("Send requries exactly one handler of type {0}", typeof(T)), "message");
            }
            return PublishByType(message, typeof(T));
        }

        public Task Publish<T>(T message) where T : Message
        {
            return DispatchByType(message);
        }

        public void Subscribe<T>(Handles<T> handler) where T : Message
        {
            Subscribe(typeof (T), handler);
        }

        public void Unsubscribe<T>(Handles<T> handler) where T : Message
        {
            Unsubscribe(typeof (T), handler);
        }

        #endregion

        private Task DispatchByType(Message message)
        {
            var messagingTypeHeirarchy = message.GetType().GetMessagingTypeHeirarchy();
            return Task.WhenAll(messagingTypeHeirarchy.Select(topic => PublishByType(message, topic)));
        }

        private async Task PublishByType(Message message, Type topic)
        {
            Multiplexor<Message> handler;
            if (byTypePublisher.TryGetValue(topic, out handler))
            {
                await handler.Handle(message);
            }
        }

        public void Subscribe<T>(Type topic, Handles<T> handler) where T : Message
        {
            Multiplexor<Message> multiplexor;

            if (false == byTypePublisher.TryGetValue(topic, out multiplexor))
            {
                multiplexor = new Multiplexor<Message>();
                byTypePublisher.Add(topic, multiplexor);
            }

            multiplexor.Add(handler.OptionallyNarrowTo<Message, T>());
        }

        public void Unsubscribe<T>(Type topic, Handles<T> handler) where T : Message
        {
            throw new NotSupportedException();
        }
    }
}
