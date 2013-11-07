using System.Collections;
using System.Collections.Generic;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public class CustomerListViewModel : IEnumerable<CustomerViewModel>
    {
        private readonly IEnumerable<CustomerViewModel> customers;

        public CustomerListViewModel(IEnumerable<CustomerViewModel> customers)
        {
            this.customers = customers;
        }

        public IEnumerator<CustomerViewModel> GetEnumerator()
        {
            return customers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}