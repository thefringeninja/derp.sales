using Derp.Sales.Web.ViewModels;
using Nancy;

namespace Derp.Sales.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => Negotiate.WithModel(new HomeViewModel());
        }
    }
}