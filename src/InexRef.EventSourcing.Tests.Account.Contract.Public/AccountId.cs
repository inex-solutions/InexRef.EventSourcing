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
using InexRef.EventSourcing.Contracts;
using Newtonsoft.Json;

namespace InexRef.EventSourcing.Tests.Account.Messages
{
    [JsonConverter(typeof(AccountId.AccountIdJsonConverter))]
    public class AccountId : IIdentifier<AccountId>
    {
        private readonly string _accountId;

        [JsonConstructor]
        private AccountId(string accountId)
        {
            _accountId = accountId;
        }

        public static explicit operator AccountId(string idValue) => AccountId.Parse(idValue);

        public static AccountId Parse(string idValue) => new AccountId(idValue);

        public static implicit operator string(AccountId accountId) => accountId._accountId;

        public static AccountId Null => new AccountId(string.Empty);

        #region Json Serialization

        public class AccountIdJsonConverter : JsonConverter<AccountId>
        {
            public override void WriteJson(JsonWriter writer, AccountId value, JsonSerializer serializer)
                => writer.WriteValue(value);

            public override AccountId ReadJson(JsonReader reader, Type objectType, AccountId existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
                => AccountId.Parse((string)reader.Value);
        }

        #endregion

        #region Equality / Comparers

        public bool Equals(AccountId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_accountId, other._accountId);
        }



        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType())
                return false;

            return Equals((AccountId) obj);
        }

        public override int GetHashCode()
        {
            return (_accountId != null ? _accountId.GetHashCode() : 0);
        }

        public static bool operator ==(AccountId left, AccountId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AccountId left, AccountId right)
        {
            return !Equals(left, right);
        }

        public int CompareTo(AccountId other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(_accountId, other._accountId, StringComparison.Ordinal);
        }

        #endregion
    }
}