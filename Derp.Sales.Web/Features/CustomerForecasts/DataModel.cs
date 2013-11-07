using System;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public static class DataModel
    {
        public static GetListOfCustomers GetCustomerList()
        {
            return () => new CustomerListViewModel(
                new[]
                {
                    new CustomerViewModel(Guid.NewGuid(), "Customer A"),
                    new CustomerViewModel(Guid.NewGuid(), "Customer B"),
                    new CustomerViewModel(Guid.NewGuid(), "Customer C"),
                });
        }
    }
}