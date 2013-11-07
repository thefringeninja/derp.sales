using Nancy.Conventions;

namespace Derp.Sales.Web
{
    public static class OrganizationByFeature
    {
        public static void Enable(NancyConventions nancyConventions)
        {
            nancyConventions.ViewLocationConventions.Clear();

            nancyConventions.ViewLocationConventions.Add(
                (viewName, model, viewLocationContext) =>
                    "features" + viewLocationContext.ModulePath.Underscore().Pascalize() + "/views/" + viewName);

            nancyConventions.ViewLocationConventions.Add(
                (viewName, model, viewLocationContext) =>
                    "features/home/views/" + viewName);

            nancyConventions.ViewLocationConventions.Add(
                (viewName, model, viewLocationContext) =>
                    "features/" + viewName);
        }
    }
}