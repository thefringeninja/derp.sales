using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public class WideningHandler<TInput, TOutput> : Handles<TInput>
        where TInput : TOutput
        where TOutput : Message
    {
        private readonly Handles<TOutput> handler;

        public WideningHandler(Handles<TOutput> handler)
        {
            this.handler = handler;
        }

        public Task Handle(TInput message)
        {
            return handler.Handle(message);
        }
    }
}