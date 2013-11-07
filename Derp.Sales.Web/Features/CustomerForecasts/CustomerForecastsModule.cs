using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public class CustomerForecastsModule : NancyModule
    {
        public CustomerForecastsModule()
            : base("/customer-forecasts")
        {
            Get["/"] = _ => 200;
        }
    }
}