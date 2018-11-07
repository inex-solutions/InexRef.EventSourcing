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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace InexRef.EventSourcing.Contracts
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
                var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(ValueObjectMember.FromFieldInfo);
                var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(ValueObjectMember.FromPropertyInfo);

                return fields.Concat(properties);
            }
        }
    }
}