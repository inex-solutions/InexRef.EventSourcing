using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InexRef.EventSourcing.Domain
{
    public abstract class ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;

            foreach (var member in MembersToCompare)
            {
                if (typeof(IEnumerable).IsAssignableFrom(member.Type)
                    && !typeof(IEnumerable<char>).IsAssignableFrom(member.Type))
                {
                    throw new NotSupportedException("Value<T> automatic equality checking does not support IEnumerables (except strings) - please provide an overriden Equals implementation");
                }

                var thisVal = member.GetValue(this);
                var otherVal = member.GetValue(obj);

                if (ReferenceEquals(thisVal, null)
                    ^ ReferenceEquals(otherVal, null))
                {
                    return false;
                }

                if (thisVal != null
                    && !thisVal.Equals(otherVal))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 17;

                foreach (var member in MembersToCompare)
                {
                    var value = member.GetValue(this);
                    hashCode = (hashCode * 397) ^ (value != null ? value.GetHashCode() : 0);
                }

                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{GetType().Name} : " + string.Join(",", MembersToCompare.Select(m => $"{m.Name}={m.GetValue(this)}"));
        }

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !Equals(left, right);
        }

        protected virtual IEnumerable<ValueObjectMember> MembersToCompare
        {
            get
            {
                var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public).Select(ValueObjectMember.FromFieldInfo);
                var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(ValueObjectMember.FromPropertyInfo);

                return fields.Concat(properties);
            }
        }

        protected class ValueObjectMember
        {
            public string Name { get; }
            public Type Type { get; }
            public Func<object, object> GetValue { get; }

            private ValueObjectMember(string name, Type type, Func<object, object> getValue)
            {
                Name = name;
                Type = type;
                GetValue = getValue;
            }

            public static ValueObjectMember FromFieldInfo(FieldInfo item)
                => new ValueObjectMember(item.Name, item.FieldType, item.GetValue);

            public static ValueObjectMember FromPropertyInfo(PropertyInfo item)
                => new ValueObjectMember(item.Name, item.PropertyType, item.GetValue);
        }
    }
}