using System.Collections.Generic;
using Nancy.Bootstrapper;

namespace Derp.Sales.Web.Features.CustomerForecasts
{
    public class Registrations : IApplicationRegistrations
    {
        public IEnumerable<TypeRegistration> TypeRegistrations { get{return null;} }
        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations { get { return null;  } }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get
            {
                yield return new InstanceRegistration(
                    typeof(GetListOfCustomers), 
                    DataModel.GetCustomerList());
                yield return new InstanceRegistration(
                    typeof(GetListOfProducts),
                    DataModel.GetProductList());
            }
        }

    }
}