using System;
using Derp.Sales.Application;
using Derp.Sales.Domain;
using Derp.Sales.MessageBuilders;
using Derp.Sales.Messages;
using Derp.Sales.Tests.Templates;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Specifications
{
    public static class TheCustomer
    {
        public static readonly Guid Id = Guid.NewGuid();
    }

    public static class AProduct
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly string ProductName = "X-75";
        public static readonly string ProductDescription = "Ninja Pro Gaming Headsets";
    }
    public class CustomerSalesForecastSpecifications
    {
        public Specification forecasting()
        {
            return new MessageSpecification
            {
                Before = () => SystemTime.Clock = () => new DateTime(2008, 12, 28),
                Bootstrap = bus => bus.Subscribe(new CustomerSalesForecastingHandler(bus)),
                When =
                    new ForecastCustomerSales(TheCustomer.Id, AProduct.Id, IsoWeek.FromDate(new DateTime(2009, 3, 1)), 10000),
                Expect =
                {
                    result => result.Does(
                        () => New.Forecasted().ForCustomer(TheCustomer.Id).ForProduct(AProduct.Id).InWeek(9, 2009).QuantityOf(10000))
                }
            };
        }

        public Specification forecasting_in_the_past()
        {
            return new MessageSpecification
            {
                Before = () => SystemTime.Clock = () => new DateTime(2008, 12, 28),
                Bootstrap = bus => bus.Subscribe(new CustomerSalesForecastingHandler(bus)),
                When =
                    new ForecastCustomerSales(TheCustomer.Id, AProduct.Id, IsoWeek.FromDate(new DateTime(2008, 11, 1)), 10000),
                Expect =
                {
                    result => result.DidNotChangeAnything(),
                    result => result.ThrewAnException,
                    result => result.ThrownException is InvalidOperationException,
                    result => result.ThrownException.Message.Equals("You tried to forecast for 2008-W44 but the current week is 2008-W52. Forecasting in the past is not allowed.")
                }
            };
        }
    }
}