#region Copyright & License
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

using System;
using InexRef.EventSourcing.Contracts;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Tests
{
    public class MonetaryAmount : ValueObject<MonetaryAmount>
    {
        private MonetaryAmount(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public decimal Amount { get; }
        public string Currency { get; }

        public static MonetaryAmount Create(decimal amount, string currency)
            => new MonetaryAmount(amount, currency);
    }

    public class ValueObjectTestBase : SpecificationBase
    {
        protected MonetaryAmount Value1 { get; set; }
        protected MonetaryAmount Value2 { get; set; }
        protected bool EqualsResult { get; set; }
        protected int GetHashCodeResult { get; set; }
    }

    public class when_equality_checking_a_value_object : ValueObjectTestBase
    {
        protected override void Given() => Value1 = MonetaryAmount.Create(1.00M, "GBP");

        [Then]
        public void comparing_to_null_returns_false() 
            => Value1.Equals(null).ShouldBeFalse();

        [Then]
        public void comparing_to_itself_returns_true()
            // ReSharper disable once EqualExpressionComparison
            => Value1.Equals(Value1).ShouldBeTrue();

        [Then]
        public void comparing_to_a_different_but_identical_instance_returns_true()
            => Value1.Equals(MonetaryAmount.Create(1.00M, "GBP")).ShouldBeTrue();

        [Then]
        public void comparing_to_a_different_instance_with_a_different_value_on_its_first_field_returns_false()
            => Value1.Equals(MonetaryAmount.Create(2.00M, "GBP")).ShouldBeFalse();

        [Then]
        public void comparing_to_a_different_instance_with_a_different_value_on_its_second_field_returns_false()
            => Value1.Equals(MonetaryAmount.Create(1.00M, "USD")).ShouldBeFalse();

        [Then]
        public void comparing_to_a_different_instance_with_different_values_on_all_fields_returns_false()
            => Value1.Equals(MonetaryAmount.Create(2.00M, "USD")).ShouldBeFalse();

        [Then]
        public void comparing_to_a_different_but_identical_instance_with_null_as_one_field_value_returns_true()
            // ReSharper disable once EqualExpressionComparison
            => MonetaryAmount.Create(1.00M, null).Equals(MonetaryAmount.Create(1.00M, null)).ShouldBeTrue();
    }

    public class when_equality_checking_a_value_object_which_contains_an_enumerable_field : ValueObjectTestBase
    {
        protected override void When() =>
            CaughtException = Catch.Exception(
                // ReSharper disable once EqualExpressionComparison
                () => EqualsResult = ValueObjectWithEnumerable.Create(1, 2, 3).Equals(ValueObjectWithEnumerable.Create(1, 2, 3)));

        [Then]
        public void a_NotSupported_Exception_is_thrown() => CaughtException.ShouldBeOfType<NotSupportedException>();

        public class ValueObjectWithEnumerable : ValueObject<ValueObjectWithEnumerable>
        {
            private ValueObjectWithEnumerable(params int[] numbers)
            {
                Numbers = numbers;
            }

            public int[] Numbers { get; }


            public static ValueObjectWithEnumerable Create(params int[] numbers)
                => new ValueObjectWithEnumerable(numbers);
        }
    }

    public class when_getting_hash_code_for_a_value_object : ValueObjectTestBase
    {

        protected override void Given()
        {
            Value1 = MonetaryAmount.Create(1.00M, "GBP");
            Value2 = MonetaryAmount.Create(2.00M, "USD");
        }

        protected override void When() => GetHashCodeResult = Value1.GetHashCode();

        [Then]
        public void get_hash_code_returns_the_same_for_an_identical_instance() 
            => GetHashCodeResult.ShouldBe(MonetaryAmount.Create(1.00M, "GBP").GetHashCode());

        [Then]
        public void get_hash_code_returns_the_difference_value_to_a_an_instance_with_different_values()
            => GetHashCodeResult.ShouldNotBe(MonetaryAmount.Create(2.00M, "GBP").GetHashCode());
    }
}
