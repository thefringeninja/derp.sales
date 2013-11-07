using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class Multiplexor<T> : Handles<T> where T : Message
    {
        private List<Handles<T>> handlers;

        public Multiplexor(params Handles<T>[] handlers)
        {
            this.handlers = handlers.ToList();
        }

        public int Count
        {
            get { return handlers.Count; }
        }

        #region Handles<T> Members

        public async Task Handle(T message)
        {
            await Task.WhenAll(handlers.Select(handler => handler.Handle(message)));
        }

        #endregion

        public void Add(Handles<T> handler)
        {
            var handlersLocal = new List<Handles<T>>(handlers) {handler};
            handlers = handlersLocal;
        }

        public void Remove(Handles<T> handler)
        {
            var handlersLocal = new List<Handles<T>>(handlers);
            handlersLocal.Remove(handler);
            handlers = handlersLocal;
        }
    }
}
