using System;
using System.Threading.Tasks;

namespace InexRef.EventSourcing.Tests.Common.SpecificationFramework
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

        public static async Task<Exception> AsyncException(Func<Task> action)
        {
            try
            {
                await action();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static async Task<Exception> AsyncException(Action action)
        {
            try
            {
                action();
                await Task.CompletedTask;
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}