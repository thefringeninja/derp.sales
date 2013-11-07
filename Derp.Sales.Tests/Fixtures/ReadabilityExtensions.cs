namespace Derp.Sales.Tests.Fixtures
{
    public static class ReadabilityExtensions
    {
        public static bool DoesNotEqual<T>(this T instance, T result)
        {
            return instance.Equals(result).Equals(false);
        }
    }
}
