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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using InexRef.EventSourcing.Contracts.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace InexRef.EventSourcing.Persistence.SqlServer.Persistence
{
    public class SqlEventStoreJsonSerializer : ISqlEventStoreJsonSerializer
    {
        private readonly JsonSerializer _serializer;

        public SqlEventStoreJsonSerializer()
        {
            var serializerSettings = GetBaseJsonSerializerSettings();
            serializerSettings.ContractResolver = IgnoreMetadataPropertyOnEventClassContractResolver.Instance;
            _serializer = JsonSerializer.Create(serializerSettings);
        }

        private JsonSerializerSettings GetBaseJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
        }

        public string Serialize(IEvent @event)
        {
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms, Encoding.UTF8))
            {
                _serializer.Serialize(writer, @event);
                writer.Flush();
                ms.Position = 0;

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public IEvent Deserialize(string eventPayload, string sourceCorrelationId, DateTime eventDateTime)
        {
            var messageMetadata = MessageMetadata.Create(sourceCorrelationId, eventDateTime);
            var deserializerSettings = GetBaseJsonSerializerSettings();
            deserializerSettings.Converters = new List<JsonConverter>
            {
                new AddMessageMetadataConverter(GetBaseJsonSerializerSettings, messageMetadata)
            };

            var deserializer = JsonSerializer.Create(deserializerSettings);

            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms, Encoding.UTF8))
            {
                writer.Write(eventPayload);
                writer.Flush();
                ms.Position = 0;

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return (IEvent)deserializer.Deserialize(reader, typeof(IEvent));
                }
            }
        }
    }

    public class IgnoreMetadataPropertyOnEventClassContractResolver : DefaultContractResolver
    {
        public static readonly IgnoreMetadataPropertyOnEventClassContractResolver Instance = new IgnoreMetadataPropertyOnEventClassContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (typeof(IEvent).IsAssignableFrom(property.DeclaringType) 
                && property.PropertyType == typeof(MessageMetadata))
            {
                property.ShouldSerialize = o => false;
            }

            return property;
        }

        
    }

    public class AddMessageMetadataConverter : JsonConverter
    {
        private readonly Func<JsonSerializerSettings> _getSerializerSettings;
        private readonly MessageMetadata _messageMetadata;

        public AddMessageMetadataConverter(Func<JsonSerializerSettings> getSerializerSettings, MessageMetadata messageMetadata)
        {
            _getSerializerSettings = getSerializerSettings;
            _messageMetadata = messageMetadata;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("This will never be called as CanWrite is false.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.Load(reader);
            if (jToken.Type != JTokenType.Object)
            {
                return jToken;
            }

            var typeAttributeValue = (string) jToken["$type"];
            if (typeAttributeValue == null)
            {
                return jToken;
            }

            ParseTypeNameAttribute(typeAttributeValue, out var typeName, out var assemblyName);
            var messageMetadataJToken = JToken.FromObject(_messageMetadata);
            jToken["MessageMetadata"] = messageMetadataJToken;

            var binder = new DefaultSerializationBinder();
            var type = binder.BindToType(assemblyName, typeName);

            if (type != null)
            {
                var innerSerializer = JsonSerializer.Create(_getSerializerSettings());
                var result = jToken.ToObject(type, innerSerializer);
                return result;
            }

            return jToken;
        }

        private static void ParseTypeNameAttribute(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
        {
            int? assemblyDelimiterIndex = GetAssemblyDelimiterIndex(fullyQualifiedTypeName);

            if (assemblyDelimiterIndex != null)
            {
                typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
                assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
            }
            else
            {
                typeName = fullyQualifiedTypeName;
                assemblyName = null;
            }
        }

        private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
        {
            int scope = 0;
            for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
            {
                char current = fullyQualifiedTypeName[i];
                switch (current)
                {
                    case '[':
                        scope++;
                        break;
                    case ']':
                        scope--;
                        break;
                    case ',':
                        if (scope == 0)
                            return i;
                        break;
                }
            }

            return null;
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            //    return typeof(MessageMetadata) == objectType;
            var result = typeof(IEvent).IsAssignableFrom(objectType);
            return result;
        }
    }
}