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
    /// Represents a JsonConverter for BsonJavaScript values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonJavaScriptConverter : JsonConverterBase<BsonJavaScript>
    {
        #region static
        private static readonly BsonJavaScriptConverter __instance = new BsonJavaScriptConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonJavaScriptConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonJavaScriptConverter"/>.
        /// </value>
        public static BsonJavaScriptConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.JavaScript)
            {
                return (BsonJavaScript)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader, serializer);

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    return new BsonJavaScript((string)reader.Value);

                default:
                    var message = string.Format("Error reading BsonJavaScript. Unexpected token: {0}.", reader.TokenType);
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
                var bsonJavaScript = (BsonJavaScript)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteJavaScript(bsonJavaScript.Code);
                }
                else
                {
                    WriteExtendedJson(writer, bsonJavaScript, serializer);
                }
            }
        }

        // private methods
        private BsonJavaScript ReadExtendedJson(Newtonsoft.Json.JsonReader reader, Newtonsoft.Json.JsonSerializer serializer)
        {
            ReadExpectedPropertyName(reader, "$code");
            var code = reader.ReadAsString();
            ReadEndObject(reader);

            return new BsonJavaScript(code);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, BsonJavaScript value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$code");
            writer.WriteValue(value.Code);
            writer.WriteEndObject();
        }
    }
}
