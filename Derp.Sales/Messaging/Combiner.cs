using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class Combiner<T> : Handles<T> where T : Message
    {
        private readonly Handles<T> handler;

        public Combiner(Handles<T> handler)
        {
            this.handler = handler;
        }

        #region Handles<T> Members

        public Task Handle(T message)
        {
            return handler.Handle(message);
        }

        #endregion
    }
}
