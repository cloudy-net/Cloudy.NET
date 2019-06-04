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
    /// Represents a JsonConverter for BsonSymbol values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonSymbolConverter : JsonConverterBase<BsonSymbol>
    {
        #region static
        private static readonly BsonSymbolConverter __instance = new BsonSymbolConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonSymbolConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonArrayConverter"/>.
        /// </value>
        public static BsonSymbolConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.Symbol)
            {
                return (BsonSymbol)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    return BsonSymbolTable.Lookup((string)reader.Value);

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader);

                default:
                    var message = string.Format("Error reading BsonSymbol. Unexpected token: {0}.", reader.TokenType);
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
                var symbol = (BsonSymbol)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteSymbol(symbol.Name);
                }
                else
                {
                    WriteExtendedJson(writer, symbol);
                }
            }
        }

        // private methods
        private BsonSymbol ReadExtendedJson(Newtonsoft.Json.JsonReader reader)
        {
            ReadExpectedPropertyName(reader, "$symbol");
            var name = ReadStringValue(reader);
            ReadEndObject(reader);

            return BsonSymbolTable.Lookup(name);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, BsonSymbol value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$symbol");
            writer.WriteValue(value.Name);
            writer.WriteEndObject();
        }
    }
}
