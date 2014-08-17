using System;
using System.Runtime.Serialization;
using Derp.Sales.Messaging;

namespace Derp.Sales.Messages
{
    public class WtfDto
    {
        public WtfDto()
        {
            
        }
    }
    [DataContract] public class CustomerSalesForecasted : Event
    {
        [DataMember(Order = 1)] public readonly Guid CustomerId;
        [DataMember(Order = 2)] public readonly Guid ProductId;
        [DataMember(Order = 4)] public readonly int Quantity;
        [DataMember(Order = 3)] public readonly string Week;
        //public readonly WtfDto Wtf;

        public CustomerSalesForecasted(Guid customerId, Guid productId, string week, int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Week = week;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return string.Format("Sales forecasted of {0} for {1}", Quantity, Week);
        }
    }
}
