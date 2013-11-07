using System.Collections.Generic;
using Newtonsoft.Json;

namespace Derp.Sales.Tests.Templates
{
    public class SerializationEqualityComparer : IEqualityComparer<object>
    {
        public static readonly IEqualityComparer<object>  Instance = new SerializationEqualityComparer();

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
        };

        public bool Equals(object x, object y)
        {
            // if both are null, they are equal
            if (x == null && y == null)
            {
                return true;
            }
            // if one of them are null, they are not equal
            if (x == null || y == null)
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            var a = JsonConvert.SerializeObject(x, SerializerSettings);
            var b = JsonConvert.SerializeObject(y, SerializerSettings);
            return string.Equals(a, b);
        }

        public int GetHashCode(object obj)
        {
            // always perform this check
            return 0;
        }
    }
}
