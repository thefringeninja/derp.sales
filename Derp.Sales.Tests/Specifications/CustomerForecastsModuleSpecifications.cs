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
                When = () => UserAgent.Get("/customer-forecasts"),
                Expect =
                {
                    sut => sut.Response.StatusCode.Is(HttpStatusCode.OK)
                }
            };
        }
    }
}
