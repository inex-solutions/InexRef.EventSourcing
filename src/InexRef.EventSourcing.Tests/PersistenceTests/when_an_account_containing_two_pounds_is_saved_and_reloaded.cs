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

using InexRef.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace InexRef.EventSourcing.Tests.PersistenceTests
{
    public class when_an_account_containing_two_pounds_is_saved_and_reloaded : AggregateRepositoryTestBase
    {
        public when_an_account_containing_two_pounds_is_saved_and_reloaded(string testFixtureOptions) : base(testFixtureOptions) { }

        protected override void Given()
        {
            var aggregate = AggregateRootFactory.Create<AccountAggregateRoot>();
            aggregate.InitialiseAccount(AggregateId, "account-id");
            aggregate.AddAmount(2.00M);
            Subject.Save(aggregate);
        }

        protected override void When() => ReloadedAccountAggregateRoot = Subject.Get(AggregateId);

        [Then]
        public void the_reloaded_account_contains_two_pounds() => ReloadedAccountAggregateRoot.Balance.ShouldBe(2.00M);
    }
}