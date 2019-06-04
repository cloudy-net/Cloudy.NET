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
using System.Globalization;
using MongoDB.Bson;

namespace MongoDB.Integrations.JsonDotNet.Converters
{
    /// <summary>
    /// Represents a JsonConverter for BsonDateTime values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonDateTimeConverter : JsonConverterBase<BsonDateTime>
    {
        #region static
        private static readonly BsonDateTimeConverter __instance = new BsonDateTimeConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonDateTimeConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonDateTimeConverter"/>.
        /// </value>
        public static BsonDateTimeConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.DateTime)
            {
                return (BsonDateTime)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Date:
                    var dateTime = (DateTime)reader.Value;
                    var millisecondsSinceEpoch = BsonUtils.ToMillisecondsSinceEpoch(dateTime);
                    return new BsonDateTime(millisecondsSinceEpoch);

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader);

                default:
                    var message = string.Format("Error reading BsonDateTime. Unexpected token: {0}.", reader.TokenType);
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
                var bsonDateTime = (BsonDateTime)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    var millisecondsSinceEpoch = bsonDateTime.MillisecondsSinceEpoch;
                    adapter.WriteDateTime(millisecondsSinceEpoch);
                }
                else
                {
                    if (bsonDateTime.IsValidDateTime)
                    {
                        writer.WriteValue(bsonDateTime.ToUniversalTime());
                    }
                    else
                    {
                        WriteExtendedJson(writer, bsonDateTime);
                    }
                }
            }
        }

        // private methods
        private BsonDateTime ReadExtendedJson(Newtonsoft.Json.JsonReader reader)
        {
            long millisecondsSinceEpoch;

            ReadExpectedPropertyName(reader, "$date");
            ReadToken(reader);
            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Date:
                    {
                        var dateTime = (DateTime)reader.Value;
                        millisecondsSinceEpoch = BsonUtils.ToMillisecondsSinceEpoch(dateTime);
                    }
                    break;

                case Newtonsoft.Json.JsonToken.StartObject:
                    {
                        ReadExpectedPropertyName(reader, "$numberLong");
                        var formattedString = ReadStringValue(reader);
                        ReadEndObject(reader);
                        millisecondsSinceEpoch = long.Parse(formattedString, NumberFormatInfo.InvariantInfo);
                    }
                    break;

                case Newtonsoft.Json.JsonToken.String:
                    {
                        var formattedString = (string)reader.Value;
                        var dateTime = DateTime.Parse(formattedString, DateTimeFormatInfo.InvariantInfo);
                        millisecondsSinceEpoch = BsonUtils.ToMillisecondsSinceEpoch(dateTime);
                    }
                    break;

                default:
                    var message = string.Format("Error reading BsonDateTime. Unexpected token: {0}.", reader.TokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
            ReadEndObject(reader);

            return new BsonDateTime(millisecondsSinceEpoch);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, BsonDateTime value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$date");
            writer.WriteStartObject();
            writer.WritePropertyName("$numberLong");
            writer.WriteValue(value.MillisecondsSinceEpoch.ToString(NumberFormatInfo.InvariantInfo));
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
