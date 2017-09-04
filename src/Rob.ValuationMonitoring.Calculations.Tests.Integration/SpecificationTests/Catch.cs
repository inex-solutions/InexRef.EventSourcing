using System;

namespace Rob.ValuationMonitoring.Calculations.Tests.Integration.SpecificationTests
{
    public static class Catch
    {
        public static Exception Exception(Action action)
        {
            try
            {
                action();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}