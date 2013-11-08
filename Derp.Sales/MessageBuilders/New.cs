using System;
using Derp.Sales.Domain;
using Derp.Sales.Messages;
using Derp.Sales.Messaging;

namespace Derp.Sales.MessageBuilders
{
    public static class New
    {
        public static ForecastCustomerSalesBuilder Forecast()
        {
            return new ForecastCustomerSalesBuilder();
        }

        #region Nested type: ForecastCustomerSalesBuilder

        public class ForecastCustomerSalesBuilder
        {
            public Guid CustomerId { get; set; }
            public Guid ProductId { get; set; }
            public string Week { get; set; }
            public int Quantity { get; set; }

            public ForecastCustomerSalesBuilder ForCustomer(Guid id)
            {
                CustomerId = id;
                return this;
            }

            public ForecastCustomerSalesBuilder ForProduct(Guid id)
            {
                ProductId = id;
                return this;
            }

            public ForecastCustomerSalesBuilder InWeek(int week, int year)
            {
                Week = year + "-W" + week;
                return this;
            }

            public ForecastCustomerSalesBuilder QuantityOf(int quantity)
            {
                Quantity = quantity;
                return this;
            }

            public static implicit operator ForecastCustomerSales(ForecastCustomerSalesBuilder builder)
            {
                return new ForecastCustomerSales(builder.CustomerId, builder.ProductId, builder.Week, builder.Quantity);
            }
        }

        #endregion

        public static CustomerSalesForecastedBuilder Forecasted()
        {
            return new CustomerSalesForecastedBuilder();
        }

        #region Nested type: CustomerSalesForecastedBuilder

        public class CustomerSalesForecastedBuilder
        {
            public Guid CustomerId { get; set; }
            public Guid ProductId { get; set; }
            public string Week { get; set; }
            public int Quantity { get; set; }

            public CustomerSalesForecastedBuilder ForCustomer(Guid id)
            {
                CustomerId = id;
                return this;
            }

            public CustomerSalesForecastedBuilder ForProduct(Guid id)
            {
                ProductId = id;
                return this;
            }

            public CustomerSalesForecastedBuilder InWeek(int week, int year)
            {
                Week = year + "-W" + week.ToString("D2");
                return this;
            }


            public CustomerSalesForecastedBuilder InWeek(IsoWeek week)
            {
                Week = week.ToString();
                return this;
            }
            
            public CustomerSalesForecastedBuilder QuantityOf(int quantity)
            {
                Quantity = quantity;
                return this;
            }

            public static implicit operator CustomerSalesForecasted(CustomerSalesForecastedBuilder builder)
            {
                return new CustomerSalesForecasted(builder.CustomerId, builder.ProductId, builder.Week, builder.Quantity);
            }
        }

        #endregion
    }
}
