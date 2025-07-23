using System;

namespace Terret_Billing.Tests
{
    public static class UnitTestHelper
    {
        public static void AssertEqual<T>(T expected, T actual, string message = null)
        {
            if (!Equals(expected, actual))
                throw new Exception($"Assertion failed: {message ?? "Values not equal"} | Expected: {expected}, Actual: {actual}");
        }
    }
}
