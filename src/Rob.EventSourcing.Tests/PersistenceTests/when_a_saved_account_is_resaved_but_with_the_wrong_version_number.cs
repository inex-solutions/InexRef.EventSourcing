#region Copyright & License
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

using System;
using Rob.EventSourcing.Persistence;
using Rob.EventSourcing.Tests.SpecificationTests;
using Shouldly;

namespace Rob.EventSourcing.Tests.PersistenceTests
{
    public class when_a_saved_account_is_resaved_but_with_the_wrong_version_number : AggregateRepositoryTestBase
    {
        private Exception CaughtException;

        public when_a_saved_account_is_resaved_but_with_the_wrong_version_number(string persistenceProvider) : base(persistenceProvider) { }

        protected override void Given()
        {
            ReloadedAccountAggregateRoot = new NonDisposingAccountAggregateRoot(AggregateId);
            ReloadedAccountAggregateRoot.AddAmount(2.00M);

            // intermediate save should cause a concurrency error when we save below
            Subject.Save(ReloadedAccountAggregateRoot);

            ReloadedAccountAggregateRoot.AddAmount(5.00M);
        }

        protected override void When() => CaughtException = Catch.Exception (() => Subject.Save(ReloadedAccountAggregateRoot));

        [Then]
        public void a_EventStoreConcurrencyException_should_be_thrown() => CaughtException.ShouldBeOfType<EventStoreConcurrencyException>();
    }
}