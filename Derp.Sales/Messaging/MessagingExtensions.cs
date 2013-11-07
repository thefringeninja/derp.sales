using System;
using System.Collections.Generic;
using System.Linq;

namespace Derp.Sales.Messaging
{
    public static class MessagingExtensions
    {
        public static Handles<TInput> NarrowTo<TInput, TOutput>(this Handles<TOutput> handler)
            where TInput : Message
            where TOutput : TInput

        {
            return new NarrowingHandler<TInput, TOutput>(handler);
        }

        public static Handles<TInput> OptionallyNarrowTo<TInput, TOutput>(this Handles<TOutput> handler)
            where TInput : Message
            where TOutput : TInput
        {
            return new OptionalHandler<TInput, TOutput>(handler.NarrowTo<TInput, TOutput>());
        }

        public static Handles<TInput> WidenFrom<TInput, TOutput>(this Handles<TOutput> handler)
            where TInput : TOutput
            where TOutput : Message

        {
            return new WideningHandler<TInput, TOutput>(handler);
        }

        public static IEnumerable<Type> GetMessagingTypeHeirarchy(this Type messageType)
        {
            if (messageType == null)
                throw new ArgumentNullException("messageType");

            if (false == typeof (Message).IsAssignableFrom(messageType))
                throw new ArgumentException(String.Format("{0} does not implement {1}", messageType, typeof (Message)));

            var type = messageType;

            ICollection<Type> types = new HashSet<Type> {type};

            do
            {
                type.GetInterfaces().Concat(new[] {type.BaseType}).ForEach(types.Add);
                type = type.BaseType;
            } while (type != typeof (object) && type != null);

            return types.Where(x => x != typeof (object) && x != null).ToList().AsReadOnly();
        }
    }
}
