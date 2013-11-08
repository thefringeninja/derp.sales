using System;
using System.Collections;
using System.Collections.Generic;
using Derp.Sales.MessageBuilders;
using Derp.Sales.Messages;
using Derp.Sales.Messaging;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public delegate ProductListViewModel GetListOfProducts(Guid customerId);

    public class CustomerForecastsModule : NancyModule
    {
        public CustomerForecastsModule(
            CommandSender bus,
            GetListOfCustomers getListOfCustomers, 
            GetListOfProducts getListOfProducts)
            : base("/customer-forecasts")
        {
            Get["/"] = _ => Negotiate.WithModel(getListOfCustomers());

            Get["/{customerId}"] = p =>
            {
                Guid customerId = p.customerId;
                return Negotiate.WithModel(getListOfProducts(customerId));
            };
            Get["/{customerId}/{productId}"] = p =>
            {
                ForecastCustomerSales viewModel = this.Bind<New.ForecastCustomerSalesBuilder>();

                return Negotiate.WithModel(viewModel);
            };
            Post["/{customerId}/{productId}", runAsync: true] = async (_, ctx) =>
            {
                ForecastCustomerSales command = this.Bind<New.ForecastCustomerSalesBuilder>();

                await bus.Send(command);

                return Context.GetRedirect("/customer-forecasts/" + command.CustomerId + "/" + command.ProductId)
                              .WithStatusCode(HttpStatusCode.SeeOther);

            };
        }
    }
    public class ProductViewModel
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }

        public ProductViewModel(Guid productId, string productName, string productDescription)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
        }
    }
    public class ProductListViewModel : IEnumerable<ProductViewModel>
    {
        public Guid CustomerId { get; private set; }
        private readonly IEnumerable<ProductViewModel> products;

        public ProductListViewModel(Guid customerId, IEnumerable<ProductViewModel> products)
        {
            CustomerId = customerId;
            this.products = products;
        }

        public IEnumerator<ProductViewModel> GetEnumerator()
        {
            return products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}