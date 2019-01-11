using System.Collections.Generic;
using System.Threading.Tasks;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Account.DomainHost.Tests.Querying
{
    public class when_an_account_is_accessed_before_it_has_been_initialised : AccountDomainTestBase
    {
        public when_an_account_is_accessed_before_it_has_been_initialised(string hostingFlavour) : base(hostingFlavour) { }

        [Then]
        public async Task the_repository_throws_a_KeyNotFoundException() => 
            (await Catch.AsyncException(async () => await Repository.GetByNaturalKey(NaturalId))).ShouldBeOfType<KeyNotFoundException>();

        [Then]
        public void the_read_model_throws_a_KeyNotFoundException() =>
            Catch.Exception(() => BalanceReadModel[NaturalId].ShouldBe(0.00M)).ShouldBeOfType<KeyNotFoundException>();

        [Then]
        public void no_events_were_sent()
            => RecordedMessages.RecordedEvents.ShouldBeEmpty();
    }
}