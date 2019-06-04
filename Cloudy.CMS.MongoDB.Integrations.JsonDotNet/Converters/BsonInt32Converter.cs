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
    /// Represents a JsonConverter for BsonInt32 values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonInt32Converter : JsonConverterBase<BsonInt32>
    {
        #region static
        private static readonly BsonInt32Converter __instance = new BsonInt32Converter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonInt32Converter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonInt32Converter"/>.
        /// </value>
        public static BsonInt32Converter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.Int32)
            {
                return (BsonInt32)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Float:
                case Newtonsoft.Json.JsonToken.Integer:
                case Newtonsoft.Json.JsonToken.String:
                    var intValue = Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo);
                    return (BsonInt32)intValue;

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                default:
                    var message = string.Format("Error reading BsonInt32. Unexpected token: {0}.", reader.TokenType);
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
                var bsonInt32 = (BsonInt32)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteInt32(bsonInt32.Value);
                }
                else
                {
                    writer.WriteValue((long)bsonInt32.Value);
                }
            }
        }
    }
}
