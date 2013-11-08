using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Derp.Sales.Tests.Fixtures;
using Nancy;
using Nancy.Responses;
using Nancy.Testing;
using Nancy.ViewEngines;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Templates
{
    public class ModuleSpecification<TModule> : TypedSpecification<SystemUnderTest>
        where TModule : class, INancyModule
    {
        public readonly List<Expression<Func<SystemUnderTest, bool>>> Expect =
            new List<Expression<Func<SystemUnderTest, bool>>>();

        static ModuleSpecification()
        {
        }

        public Action Before;
        public Action<ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator> Bootstrap = with => { };
        public string ContentType = "text/html";

        public Action Finally;
        public string Name;

        public Action<BrowserContext> OnContext = context => { };

        public Func<UserAgent> When;

        #region TypedSpecification<SystemUnderTest> Members

        public string GetName()
        {
            return Name;
        }

        public Action GetBefore()
        {
            return Before;
        }

        public Delegate GetOn()
        {
            return new Func<UserAgent>(
                () =>
                {
                    var userAgent = When();
                    userAgent.OnContext = context =>
                    {
                        context.Accept(ContentType);
                        OnContext(context);
                    };
                    return userAgent;
                });
        }

        public Delegate GetWhen()
        {
            return new Func<UserAgent, SystemUnderTest>(
                userAgent =>
                {
                    var browser = new Browser(
                        with => Bootstrap(
                            with.Module<TModule>()
                                .ViewFactory<TestViewFactory>()));

                    var browserResponse = userAgent.Execute(browser);
                    
                    return new SystemUnderTest(browserResponse);
                });
        }

        public IEnumerable<Expression<Func<SystemUnderTest, bool>>> GetAssertions()
        {
            return Expect;
        }

        public Action GetFinally()
        {
            return Finally;
        }

        #endregion

        #region Nested type: TestViewFactory

        public class TestViewFactory : IViewFactory
        {
            #region IViewFactory Members

            public Response RenderView(string viewName, dynamic model, ViewLocationContext viewLocationContext)
            {
                viewLocationContext.Context.Items[SystemUnderTest.ViewModelKey] = model;
                return new HtmlResponse();
            }

            #endregion
        }

        #endregion
    }
}
