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
    /// Represents a JsonConverter for BsonRegularExpression values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonRegularExpressionConverter : JsonConverterBase<BsonRegularExpression>
    {
        #region static
        private static readonly BsonRegularExpressionConverter __instance = new BsonRegularExpressionConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonRegularExpressionConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonRegularExpressionConverter"/>.
        /// </value>
        public static BsonRegularExpressionConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.RegularExpression)
            {
                return (BsonRegularExpression)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader);

                case Newtonsoft.Json.JsonToken.String:
                    return new BsonRegularExpression((string)reader.Value);

                default:
                    var message = string.Format("Error reading BsonRegularExpression. Unexpected token: {0}.", reader.TokenType);
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
                var bsonRegularExpression = (BsonRegularExpression)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteRegularExpression(bsonRegularExpression);
                }
                else
                {
                    var jsonDotNetBsonWriter = writer as Newtonsoft.Json.Bson.BsonWriter;
                    if (jsonDotNetBsonWriter != null)
                    {
                        jsonDotNetBsonWriter.WriteRegex(bsonRegularExpression.Pattern, bsonRegularExpression.Options);
                    }
                    else
                    {
                        WriteExtendedJson(writer, bsonRegularExpression);
                    }
                }
            }
        }

        // private methods
        private BsonRegularExpression ReadExtendedJson(Newtonsoft.Json.JsonReader reader)
        {
            ReadExpectedPropertyName(reader, "$regex");
            var pattern = ReadStringValue(reader);
            ReadExpectedPropertyName(reader, "$options");
            var options = ReadStringValue(reader);
            ReadEndObject(reader);

            return new BsonRegularExpression(pattern, options);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, BsonRegularExpression regularExpression)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$regex");
            writer.WriteValue(regularExpression.Pattern);
            writer.WritePropertyName("$options");
            writer.WriteValue(regularExpression.Options);
            writer.WriteEndObject();
        }
    }
}
