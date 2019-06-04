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
    /// Represents a JsonConverter for BsonArray values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonArrayConverter : JsonConverterBase<BsonArray>
    {
        #region static
        private static readonly BsonArrayConverter __instance = new BsonArrayConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonArrayConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonArrayConverter"/>.
        /// </value>
        public static BsonArrayConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.StartArray:
                    return ReadArray(reader, serializer);

                default:
                    var message = string.Format("Error reading BsonArray. Unexpected token: {0}.", reader.TokenType);
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
                WriteArray(writer, (BsonArray)value, serializer);
            }
        }

        // private methods
        private BsonArray ReadArray(Newtonsoft.Json.JsonReader reader, Newtonsoft.Json.JsonSerializer serializer)
        {
            var array = new BsonArray();
            while (reader.Read() && reader.TokenType != Newtonsoft.Json.JsonToken.EndArray)
            {
                var item = serializer.Deserialize<BsonValue>(reader);
                array.Add(item);
            }
            return array;
        }

        private void WriteArray(Newtonsoft.Json.JsonWriter writer, BsonArray array, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var item in array)
            {
                serializer.Serialize(writer, item, typeof(BsonValue));
            }
            writer.WriteEndArray();
        }
    }
}
