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
    /// Represents a JsonConverter for BsonTimestamp values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonTimestampConverter : JsonConverterBase<BsonTimestamp>
    {
        #region static
        private static readonly BsonTimestampConverter __instance = new BsonTimestampConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonTimestampConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonArrayConverter"/>.
        /// </value>
        public static BsonTimestampConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.Timestamp)
            {
                return (BsonTimestamp)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Integer:
                    return new BsonTimestamp((long)reader.Value);

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader);

                default:
                    var message = string.Format("Error reading BsonTimestamp. Unexpected token: {0}.", reader.TokenType);
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
                var timestamp = (BsonTimestamp)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteTimestamp(timestamp.Value);
                }
                else
                {
                    WriteExtendedJson(writer, timestamp);
                }
            }
        }

        // private methods
        private BsonTimestamp ReadExtendedJson(Newtonsoft.Json.JsonReader reader)
        {
            ReadExpectedPropertyName(reader, "$timestamp");
            ReadStartObject(reader);
            ReadExpectedPropertyName(reader, "t");
            var timestamp = (int)reader.ReadAsInt32();
            ReadExpectedPropertyName(reader, "i");
            var increment = (int)reader.ReadAsInt32();
            ReadEndObject(reader);
            ReadEndObject(reader);

            return new BsonTimestamp(timestamp, increment);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, BsonTimestamp value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$timestamp");
            writer.WriteStartObject();
            writer.WritePropertyName("t");
            writer.WriteValue(value.Timestamp);
            writer.WritePropertyName("i");
            writer.WriteValue(value.Increment);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
