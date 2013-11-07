using System;
using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class MessageHandler<T> : Handles<Message>, IEquatable<Handles<Message>> where T : Message
    {
        private readonly Handles<T> handler;

        public MessageHandler(Handles<T> handler)
        {
            this.handler = handler;
        }

        public Task Handle(Message message)
        {
            if (message is T)
            {
                return handler.Handle((T) message);
            }

            return Task.FromResult(true);
        }

        public bool Equals(Handles<Message> other)
        {
            throw new NotImplementedException();
        }
    }
    public class OptionalHandler<T1, T2> : Handles<T1> where T1 : Message where T2 : Message
    {
        private readonly Handles<T1> handler;

        public OptionalHandler(Handles<T1> handler)
        {
            this.handler = handler;
        }

        public Task Handle(T1 message)
        {
            return message is T2
                ? handler.Handle(message)
                : Task.FromResult(true);
        }
    }
}