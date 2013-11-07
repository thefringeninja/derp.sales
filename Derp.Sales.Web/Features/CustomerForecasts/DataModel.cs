using System;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public static class DataModel
    {
        private static readonly Guid SingleCustomerId = Guid.Parse("F9DA5B07-52B3-4B10-B599-8834B699C4A1");

        static readonly ProductViewModel OnlyProduct = new ProductViewModel(Guid.Parse("F6376295-B9A8-4320-997A-C3F4F41F7FC5"), "X-75", "Ninja Pro Gaming Headsets");

        public static GetListOfCustomers GetCustomerList()
        {
            return () => new CustomerListViewModel(
                new[]
                {
                    new CustomerViewModel(SingleCustomerId, "Our Only Customer")
                });
        }

        public static GetListOfProducts GetProductList()
        {
            return customerId => new ProductListViewModel(customerId, new []{ OnlyProduct });
        }
    }
}