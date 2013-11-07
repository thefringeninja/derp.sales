using System;
using Nancy;

namespace Derp.Sales.Tests
{
    public static class AssertionExtensions
    {
        public static bool Is(this HttpStatusCode actual, HttpStatusCode expected)
        {
            return actual == expected;
        }

        public static bool Equals<T>(this T item, Func<T> factory)
        {
            return item.Equals(factory());
        }
    }
}