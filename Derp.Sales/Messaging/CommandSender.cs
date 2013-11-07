using System.Threading.Tasks;

namespace Derp.Sales.Messaging
{
    public interface CommandSender
    {
        Task Send<T>(T message) where T : Command;
    }
}