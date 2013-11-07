using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public interface EventPublisher
    {
        Task Publish<T>(T message) where T : Message;
    }
}
