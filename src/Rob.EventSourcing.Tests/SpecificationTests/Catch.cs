using System;

namespace Rob.EventSourcing.Tests.SpecificationTests
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