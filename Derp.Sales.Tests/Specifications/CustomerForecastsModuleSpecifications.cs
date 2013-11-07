using System;
using System.Linq;
using Derp.Sales.Tests.Fixtures;
using Derp.Sales.Tests.Templates;
using Derp.Sales.Web.Features.CustomerForecasts;
using Nancy;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Specifications
{
    public class CustomerForecastsModuleSpecifications
    {
        public Specification viewing_list_of_customers()
        {
            return new ModuleSpecification<CustomerForecastsModule>
            {
                Bootstrap = c => c.Dependency<GetListOfCustomers>(() => new CustomerListViewModel(new []
                {
                    new CustomerViewModel(Guid.NewGuid(), "Customer A"), 
                    new CustomerViewModel(Guid.NewGuid(), "Customer B"), 
                    new CustomerViewModel(Guid.NewGuid(), "Customer C"), 
                })),
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
    }
}
