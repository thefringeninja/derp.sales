using System;
using System.Runtime.Serialization;
using Derp.Sales.Messaging;

namespace Derp.Sales.Messages
{
    [DataContract] public class ForecastCustomerSales
        : Command, IEquatable<ForecastCustomerSales>
    {
        public bool Equals(ForecastCustomerSales other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return CustomerId.Equals(other.CustomerId) && ProductId.Equals(other.ProductId) && Quantity == other.Quantity && string.Equals(Week, other.Week);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ForecastCustomerSales) obj);
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

        [DataMember] public readonly Guid CustomerId;
        [DataMember] public readonly Guid ProductId;
        [DataMember] public readonly int Quantity;
        [DataMember] public readonly string Week;

        public ForecastCustomerSales(Guid customerId, Guid productId, string week, int quantity)
        {
            CustomerId = customerId;
            ProductId = productId;
            Week = week;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return string.Format("Forecasting sales of {0} for {1}", Quantity, Week);
        }
    }
}
