using Nancy;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public class CustomerForecastsModule : NancyModule
    {
        public CustomerForecastsModule(GetListOfCustomers getListOfCustomers)
            : base("/customer-forecasts")
        {
            Get["/"] = _ => Negotiate.WithModel(getListOfCustomers());
        }
    }
}