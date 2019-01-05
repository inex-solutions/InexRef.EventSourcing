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