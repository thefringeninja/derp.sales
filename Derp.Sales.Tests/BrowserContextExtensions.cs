using Nancy.Testing;
using Newtonsoft.Json;

namespace Derp.Sales.Tests
{
    public static class BrowserContextExtensions
    {
        public static BrowserContext Form(this BrowserContext browserContext, string form)
        {
            var contextValues =
                (IBrowserContextValues) browserContext;

            contextValues.FormValues = form;
            contextValues.Headers["Content-Type"] = new[] {"application/x-www-form-urlencoded"};

            return browserContext;
        }

        public static BrowserContext Json<T>(this BrowserContext browserContext, T content)
        {
            var contextValues =
                (IBrowserContextValues)browserContext;

            contextValues.BodyString = JsonConvert.SerializeObject(content, Formatting.Indented);
            contextValues.Headers["Content-Type"] = new[] { "application/json" };

            return browserContext;
        }

        public static BrowserContext AddQuery(this BrowserContext browserContext, string key, string value)
        {
            browserContext.Query(key, value);
            return browserContext;
        }

    }
}