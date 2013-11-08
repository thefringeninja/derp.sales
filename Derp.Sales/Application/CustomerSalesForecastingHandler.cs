using System;
using System.Threading.Tasks;
using Derp.Sales.Domain;
using Derp.Sales.MessageBuilders;
using Derp.Sales.Messages;
using Derp.Sales.Messaging;

namespace Derp.Sales.Application
{
    public class CustomerSalesForecastingHandler : Handles<ForecastCustomerSales>
    {
        private readonly EventPublisher bus;

        public CustomerSalesForecastingHandler(EventPublisher bus)
        {
            this.bus = bus;
        }

        #region Handles<ForecastCustomerSales> Members

        public async Task Handle(ForecastCustomerSales message)
        {
            CustomerSalesForecasted @event = New.Forecasted()
                                                .ForCustomer(message.CustomerId)
                                                .ForProduct(message.ProductId)
                                                .QuantityOf(message.Quantity).InWeek(IsoWeek.FromString(message.Week));
            await bus.Publish(@event);
        }

        #endregion
    }
}
