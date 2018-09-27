#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using InexRef.EventSourcing.Persistence;
using InexRef.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace InexRef.EventSourcing.Tests.PersistenceTests
{
    public class when_a_saved_account_containing_two_pounds_is_loaded_has_five_pounds_added_and_saved : AggregateRepositoryTestBase
    {
        public when_a_saved_account_containing_two_pounds_is_loaded_has_five_pounds_added_and_saved(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given()
        {
            var aggregate = AggregateRootFactory.Create<AccountAggregateRoot>();
            aggregate.InitialiseAccount(AggregateId, "account-id");
            aggregate.AddAmount(2.00M);
            Subject.Save(aggregate);

            ReloadedAccountAggregateRoot = Subject.Get(AggregateId);
            ReloadedAccountAggregateRoot.AddAmount(5.00M);
            Subject.Save(ReloadedAccountAggregateRoot);
        }

        protected override void When() => ReloadedAccountAggregateRoot = Subject.Get(AggregateId);

        [Then]
        public void then_when_reloaded_the_account_contains_seven_pounds() => ReloadedAccountAggregateRoot.Balance.ShouldBe(7.00M);
    }

    public class when_get_is_called_for_a_non_existent_aggregate : AggregateRepositoryTestBase
    {
        public when_get_is_called_for_a_non_existent_aggregate(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given()
        {
        }

        protected override void When() => CaughtException = Catch.Exception(() => Subject.Get(AggregateId));

        [Then]
        public void an_aggregate_not_found_exception_is_thrown() => CaughtException.ShouldBeOfType<AggregateNotFoundException>();
    }

    public class when_GetOrCreateNew_is_called_for_a_non_existent_aggregate : AggregateRepositoryTestBase
    {
        public when_GetOrCreateNew_is_called_for_a_non_existent_aggregate(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given()
        {
        }

        protected override void When() => ReloadedAccountAggregateRoot = Subject.GetOrCreateNew(AggregateId, null);

        [Then]
        public void no_exception_is_thrown_and_a_new_aggregate_is_returned() => ReloadedAccountAggregateRoot
            .ShouldNotBeNull();
    }
}