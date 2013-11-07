using System;
using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class NarrowingHandler<TInput, TOutput> : Handles<TInput>, IEquatable<NarrowingHandler<TInput, TOutput>>
        where TInput : Message
        where TOutput : TInput
    {
        private readonly Handles<TOutput> handler;

        public NarrowingHandler(Handles<TOutput> handler)
        {
            this.handler = handler;
        }

        #region Handles<TOutput> Members

        public Task Handle(TInput message)
        {
            return handler.Handle((TOutput)message);
        }

        #endregion

        #region IEquatable<NarrowingHandler<TInput,TOutput>> Members

        public bool Equals(NarrowingHandler<TInput, TOutput> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(handler, other.handler);
        }

        #endregion

        public override int GetHashCode()
        {
            return (handler != null
                ? handler.GetHashCode()
                : 0);
        }

        public override bool Equals(object obj)
        {
            return handler.Equals(obj);
        }
    }
}
