using System;
using System.Collections;
using System.Collections.Generic;
using Derp.Sales.MessageBuilders;
using Derp.Sales.Messages;
using Nancy;
using Nancy.ModelBinding;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public delegate ProductListViewModel GetListOfProducts(Guid customerId);

    public class CustomerForecastsModule : NancyModule
    {
        public CustomerForecastsModule(GetListOfCustomers getListOfCustomers, GetListOfProducts getListOfProducts)
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