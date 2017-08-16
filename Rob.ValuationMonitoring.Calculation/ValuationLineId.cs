using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace Rob.ValuationMonitoring.Calculation
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class ValuationLineId : SingleValueObject<string>, IIdentity
    {
        public ValuationLineId(string value) : base(value)
        {
        }
    }
}