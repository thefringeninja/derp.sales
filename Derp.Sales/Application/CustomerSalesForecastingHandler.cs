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
            var forecastWeek = IsoWeek.FromString(message.Week);
            var currentWeek = IsoWeek.FromDate(SystemTime.Now);

            if (forecastWeek < currentWeek) throw new InvalidOperationException(String.Format("You tried to forecast for {0} but the current week is {1}. Forecasting in the past is not allowed.",
                forecastWeek, currentWeek));

            CustomerSalesForecasted @event = New.Forecasted()
                                                .ForCustomer(message.CustomerId)
                                                .ForProduct(message.ProductId)
                                                .QuantityOf(message.Quantity).InWeek(forecastWeek);
            await bus.Publish(@event);
        }

        #endregion
    }
}
