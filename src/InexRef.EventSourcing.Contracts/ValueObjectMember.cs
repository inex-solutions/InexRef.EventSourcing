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
using System.Reflection;

namespace InexRef.EventSourcing.Contracts
{
    public class ValueObjectMember
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