using System;
using System.Runtime.Serialization;
using Derp.Sales.Messaging;

namespace Derp.Sales.Messages
{
    [DataContract] public class CustomerSalesForecasted : Event, IEquatable<CustomerSalesForecasted>
    {
        [DataMember(Order = 1)] public readonly Guid CustomerId;
        [DataMember(Order = 2)] public readonly Guid ProductId;
        [DataMember(Order = 4)] public readonly int Quantity;
        [DataMember(Order = 3)] public readonly string Week;

        public CustomerSalesForecasted(Guid customerId, Guid productId, string week, int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Week = week;
            Quantity = quantity;
        }

        #region IEquatable<CustomerSalesForecasted> Members

        public bool Equals(CustomerSalesForecasted other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return CustomerId.Equals(other.CustomerId) && ProductId.Equals(other.ProductId)
                   && Quantity == other.Quantity && string.Equals(Week, other.Week);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((CustomerSalesForecasted) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CustomerId.GetHashCode();
                hashCode = (hashCode*397) ^ ProductId.GetHashCode();
                hashCode = (hashCode*397) ^ Quantity;
                hashCode = (hashCode*397) ^ (Week != null
                    ? Week.GetHashCode()
                    : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("Sales forecasted of {0} for {1}", Quantity, Week);
        }
    }
}
