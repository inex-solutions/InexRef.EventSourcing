#region Copyright & Licence
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
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