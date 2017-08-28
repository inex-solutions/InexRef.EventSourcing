using System.Threading.Tasks;
using EventFlow.Configuration;

namespace Rob.ValuationMonitoring.WindowsHost
{
    public interface ITest
    {
        void Init(IRootResolver resolver);

        Task Execute();
    }
}