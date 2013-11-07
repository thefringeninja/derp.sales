using System;
using System.Linq;
using Derp.Sales.Tests.Fixtures;
using Derp.Sales.Tests.Templates;
using Derp.Sales.Web.Features.CustomerForecasts;
using Nancy;
using Nancy.Testing;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Specifications
{
    public class CustomerForecastsModuleSpecifications
    {
        private static readonly Guid CustomerId = Guid.NewGuid();
        private static readonly Guid ProductId = Guid.NewGuid();
        private static readonly string ProductName = "X-75";
        private static readonly string ProductDescription = "Ninja Pro Gaming Headsets";

        private static void Bootstrap(ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator configure)
        {
            configure.Dependency<GetListOfCustomers>(
                () => new CustomerListViewModel(
                    new[]
                    {
                        new CustomerViewModel(CustomerId, "Customer A"),
                        new CustomerViewModel(Guid.NewGuid(), "Customer B"),
                        new CustomerViewModel(Guid.NewGuid(), "Customer C"),
                    }))
                    .Dependency<GetListOfProducts>(
                        customerId => new ProductListViewModel(customerId, new []
                        {
                            new ProductViewModel(ProductId, ProductName, ProductDescription), 
                        }));
        }

        public Specification viewing_list_of_customers()
        {
            return new ModuleSpecification<CustomerForecastsModule>
            {
                Bootstrap = Bootstrap,
                When = () => UserAgent.Get("/customer-forecasts"),
                Expect =
                {
                    sut => sut.Response.StatusCode.Is(HttpStatusCode.OK),
                    sut => sut.Response.ViewModel<CustomerListViewModel>() != null,
                    sut => sut.Response.ViewModel<CustomerListViewModel>()
                        .Count().Equals(3)
                }
            };
        }

        public Specification viewing_list_of_forecastable_products_by_customer()
        {
            return new ModuleSpecification<CustomerForecastsModule>
            {
                Bootstrap = Bootstrap,
                When = () => UserAgent.Get("/customer-forecasts/" + CustomerId),
                Expect =
                {
                    sut => sut.Response.StatusCode.Is(HttpStatusCode.OK),
                    sut => sut.Response.ViewModel<ProductListViewModel>() != null,
                    sut => sut.Response.ViewModel<ProductListViewModel>().Count().Equals(1),
                    sut => sut.Response.ViewModel<ProductListViewModel>().CustomerId.Equals(CustomerId)
                }
            };
        }
    }
}
