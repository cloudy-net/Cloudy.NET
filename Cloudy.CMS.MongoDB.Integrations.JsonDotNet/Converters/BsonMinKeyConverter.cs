/* Copyright 2015-2016 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using MongoDB.Bson;

namespace MongoDB.Integrations.JsonDotNet.Converters
{
    /// <summary>
    /// Represents a JsonConverter for BsonMinKey values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonMinKeyConverter : JsonConverterBase<BsonMinKey>
    {
        #region static
        private static readonly BsonMinKeyConverter __instance = new BsonMinKeyConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonMinKeyConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonMinKeyConverter"/>.
        /// </value>
        public static BsonMinKeyConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.MinKey)
            {
                return (BsonMinKey)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader);

                default:
                    var message = string.Format("Error reading BsonMinKey. Unexpected token: {0}.", reader.TokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        /// <inheritdoc/>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteMinKey();
                }
                else
                {
                    WriteExtendedJson(writer);
                }
            }
        }

        // private methods
        private BsonMinKey ReadExtendedJson(Newtonsoft.Json.JsonReader reader)
        {
            ReadExpectedPropertyName(reader, "$minKey");
            reader.Skip();
            ReadEndObject(reader);

            return BsonMinKey.Value;
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$minKey");
            writer.WriteValue(1);
            writer.WriteEndObject();
        }
    }
}
