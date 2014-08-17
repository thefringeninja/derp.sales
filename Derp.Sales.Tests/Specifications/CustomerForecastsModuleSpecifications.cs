using System;
using System.Collections.Generic;
using System.Linq;
using Derp.Sales.MessageBuilders;
using Derp.Sales.Tests.Fixtures;
using Derp.Sales.Tests.Templates;
using Derp.Sales.Web.Features.CustomerForecasts;
using Nancy;
using Nancy.Testing;
using Nancy.Validation;
using Nancy.Validation.FluentValidation;
using Simple.Testing.ClientFramework;

namespace Derp.Sales.Tests.Specifications
{
    public class CustomerForecastsModuleSpecifications
    {
        private static void Bootstrap(ConfigurableBootstrapper.ConfigurableBootstrapperConfigurator configure)
        {
            configure.Dependency<GetListOfCustomers>(
                () => new CustomerListViewModel(
                    new[]
                    {
                        new CustomerViewModel(TheCustomer.Id, "Customer A"),
                        new CustomerViewModel(Guid.NewGuid(), "Customer B"),
                        new CustomerViewModel(Guid.NewGuid(), "Customer C"),
                    }))
                     .Dependency<GetListOfProducts>(
                         customerId => new ProductListViewModel(
                             customerId,
                             new[]
                             {
                                 new ProductViewModel(AProduct.Id, AProduct.ProductName, AProduct.ProductDescription),
                             }))
                     .Dependency<IModelValidator>(typeof (FluentValidationValidator))
                     .Dependency<IModelValidatorFactory>(typeof (FluentValidationValidatorFactory))
                     .Dependency<IFluentAdapterFactory>(typeof (DefaultFluentAdapterFactory))
            .WithBus();
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
                When = () => UserAgent.Get("/customer-forecasts/" + TheCustomer.Id),
                Expect =
                {
                    sut => sut.Response.StatusCode.Is(HttpStatusCode.OK),
                    sut => sut.Response.ViewModel<ProductListViewModel>() != null,
                    sut => sut.Response.ViewModel<ProductListViewModel>().Count().Equals(1),
                    sut => sut.Response.ViewModel<ProductListViewModel>().CustomerId.Equals(TheCustomer.Id)
                }
            };
        }

        public Specification forecasting_sales()
        {
            return new ModuleSpecification<CustomerForecastsModule>
            {
                Bootstrap = Bootstrap,
                OnContext = context => context.Form(
                    new
                    {
                        customerId = TheCustomer.Id, productId = AProduct.Id,
                        week = "2013-W11",
                        quantity = 10000
                    }.ToApplicationFormUrlEncoded()),
                When = () => UserAgent.Post("/customer-forecasts/" + TheCustomer.Id + "/" + AProduct.Id),
                Expect =
                {
                    sut => sut.Response.StatusCode.Is(HttpStatusCode.SeeOther),
                    sut => sut.Response.Headers["Location"] == "/customer-forecasts/" + TheCustomer.Id + "/" + AProduct.Id,
                    sut => sut.Does(
                        () => New.Forecast().ForCustomer(TheCustomer.Id).ForProduct(AProduct.Id)
                                 .InWeek(11, 2013)
                                 .QuantityOf(10000))
                }
            };
        }

        public IEnumerable<Specification> invalid_input()
        {
            var rows = new Dictionary<string, object>
            {
                {
                    "blank customer id", new
                    {
                        CustomerId = Guid.Empty, productId = AProduct.Id,
                        week = "2013-W11",
                        quantity = 10000
                    }
                },
                {
                    "blank product id", new
                    {
                        customerId = TheCustomer.Id,
                        ProductId = Guid.Empty,
                        week = "2013-W11",
                        quantity = 10000
                    }
                },
                {
                    "blank week", new
                    {
                        customerId = TheCustomer.Id, productId = AProduct.Id,
                        week = String.Empty,
                        quantity = 10000
                    }
                },
                {
                    "quantity less than 1", new
                    {
                        customerId = TheCustomer.Id, productId = AProduct.Id,
                        week = String.Empty,
                        quantity = 0
                    }
                },
            };

            return from row in rows
                   let name = row.Key
                   let form = row.Value
                   select new ModuleSpecification<CustomerForecastsModule>
                   {
                       Name = "invalid inputs: " + name,
                       Bootstrap = Bootstrap,
                       OnContext = context => context.Form(form.ToApplicationFormUrlEncoded()),
                       When = () => UserAgent.Post("/customer-forecasts/" + TheCustomer.Id + "/" + AProduct.Id),
                       Expect =
                       {
                           sut => sut.Response.StatusCode.Is(HttpStatusCode.BadRequest),
                           sut => sut.DidNotChangeAnything()
                       }
                   };
        }
    }
}
