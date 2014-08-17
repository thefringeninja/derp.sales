using System.Threading.Tasks;
using Derp.Sales.Messaging;

namespace Derp.Sales.Tests.Specifications
{
    public class SystemTimeHandler : Handles<SystemTimeSetTo>
    {
        public Task Handle(SystemTimeSetTo message)
        {
            SystemTime.Clock = () => message.Date;

            return Task.FromResult(true);
        }
    }
}