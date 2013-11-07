using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public interface Handles<in T> where T : Message
    {
        Task Handle(T message);
    }
}
