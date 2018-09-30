using System;
using InexRef.EventSourcing.Domain;
using InexRef.EventSourcing.Tests.Common.SpecificationFramework;
using Shouldly;

namespace InexRef.EventSourcing.Tests.Domain
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
        protected MonetaryAmount Value1;
        protected MonetaryAmount Value2;
        protected bool EqualsResult;
        protected int GetHashCodeResult;
    }

    public class when_equality_checking_a_value_object_instance_to_null : ValueObjectTestBase
    {

        protected override void Given() => Value1 = MonetaryAmount.Create(1.00M, "GBP");

        protected override void When() => EqualsResult = Value1.Equals(null);

        [Then]
        public void equals_returns_false() => EqualsResult.ShouldBeFalse();
    }

    public class when_equality_checking_a_value_object_instance_to_itself : ValueObjectTestBase
    {

        protected override void Given() => Value1 = MonetaryAmount.Create(1.00M, "GBP");

        protected override void When() => EqualsResult = Value1.Equals(Value1);

        [Then]
        public void equals_returns_true() => EqualsResult.ShouldBeTrue();
    }

    public class when_equality_checking_a_value_object_instance_to_a_different_but_identical_instance : ValueObjectTestBase
    {

        protected override void Given()
        {
            Value1 = MonetaryAmount.Create(1.00M, "GBP");
            Value2 = MonetaryAmount.Create(1.00M, "GBP");
        }

        protected override void When() => EqualsResult = Value1.Equals(Value2);

        [Then]
        public void equals_returns_true() => EqualsResult.ShouldBeTrue();
    }

    public class when_equality_checking_a_value_object_instance_to_a_different_instance_with_different_values_on_its_first_field : ValueObjectTestBase
    {

        protected override void Given()
        {
            Value1 = MonetaryAmount.Create(1.00M, "GBP");
            Value2 = MonetaryAmount.Create(2.00M, "GBP");
        }

        protected override void When() =>  EqualsResult = Value1.Equals(Value2);

        [Then]
        public void equals_returns_false() => EqualsResult.ShouldBeFalse();
    }

    public class when_equality_checking_a_value_object_instance_to_a_different_instance_with_different_values_on_its_second_field : ValueObjectTestBase
    {

        protected override void Given()
        {
            Value1 = MonetaryAmount.Create(1.00M, "GBP");
            Value2 = MonetaryAmount.Create(1.00M, "USD");
        }

        protected override void When() => EqualsResult = Value1.Equals(Value2);

        [Then]
        public void equals_returns_false() => EqualsResult.ShouldBeFalse();
    }

    public class when_equality_checking_a_value_object_instance_to_a_different_instance_with_different_values_on_all_fields : ValueObjectTestBase
    {

        protected override void Given()
        {
            Value1 = MonetaryAmount.Create(1.00M, "GBP");
            Value2 = MonetaryAmount.Create(2.00M, "USD");
        }

        protected override void When() => EqualsResult = Value1.Equals(Value2);

        [Then]
        public void equals_returns_false() => EqualsResult.ShouldBeFalse();
    }

    public class when_equality_checking_a_value_object_instance_to_a_different_but_identical_instance_with_null_as_one_field_value : ValueObjectTestBase
    {

        protected override void Given()
        {
            Value1 = MonetaryAmount.Create(1.00M, null);
            Value2 = MonetaryAmount.Create(1.00M, null);
        }

        protected override void When() => EqualsResult = Value1.Equals(Value2);

        [Then]
        public void equals_returns_true() => EqualsResult.ShouldBeTrue();
    }

    public class when_getting_hash_code_for_an_instance : ValueObjectTestBase
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

    public class when_equality_checking_a_value_object_which_contains_an_enumerable_field : ValueObjectTestBase
    {
        protected override void When() => 
            CaughtException = Catch.Exception(
                () => EqualsResult = ValueObjectWithEnumerable.Create(1,2,3).Equals(ValueObjectWithEnumerable.Create(1,2,3)));

        [Then]
        public void a_NotSupported_Exception_is_thrown() => CaughtException.ShouldBeOfType<NotSupportedException>();
    }

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
