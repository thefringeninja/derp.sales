using System;
using Derp.Sales.MessageBuilders;
using FluentValidation;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public class ForecastCustomerSalesBuilderValidator : AbstractValidator<New.ForecastCustomerSalesBuilder>
    {
        public ForecastCustomerSalesBuilderValidator()
        {
            RuleFor(model => model.CustomerId).NotEqual(Guid.Empty);
            RuleFor(model => model.ProductId).NotEqual(Guid.Empty);
            RuleFor(model => model.Quantity).GreaterThan(0);
            RuleFor(model => model.Week).NotEmpty().NotNull();
        }
    }
}