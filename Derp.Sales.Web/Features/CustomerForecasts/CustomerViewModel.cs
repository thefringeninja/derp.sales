using System;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public class CustomerViewModel
    {
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }

        public CustomerViewModel(Guid customerId, string customerName)
        {
            CustomerId = customerId;
            CustomerName = customerName;
        }
    }
}