#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
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

using System.Threading.Tasks;
using InexRef.EventSourcing.Contracts.Messages;
using InexRef.EventSourcing.Account.Contract.Public.Types;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Account.Domain.Tests
{
    public class when_I_add_one_pound_to_an_account_containing_one_pound : AggregateRootTestBase<AccountAggregateRoot>
    {
        protected override async Task Given() => await Subject.AddAmount(MessageMetadata.CreateDefault(), MonetaryAmount.Create(1.00M));

        protected override async Task When() => await Subject.AddAmount(MessageMetadata.CreateDefault(), MonetaryAmount.Create(1.00M));

        [Then]
        public void the_balance_is_two_pounds() => Subject.Balance.ShouldBe(Balance.FromDecimal(2.0M));
    }
}