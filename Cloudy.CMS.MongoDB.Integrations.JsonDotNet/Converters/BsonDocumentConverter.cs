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
    /// Represents a JsonConverter for BsonDocument values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonDocumentConverter : JsonConverterBase<BsonDocument>
    {
        #region static
        private static readonly BsonDocumentConverter __instance = new BsonDocumentConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonDocumentConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonDocumentConverter"/>.
        /// </value>
        public static BsonDocumentConverter Instance
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

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadDocument(reader, serializer);

                default:
                    var message = string.Format("Error reading BsonDocument. Unexpected token: {0}.", reader.TokenType);
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
                WriteDocument(writer, (BsonDocument)value, serializer);
            }
        }

        // private methods
        private BsonDocument ReadDocument(Newtonsoft.Json.JsonReader reader, Newtonsoft.Json.JsonSerializer serializer)
        {
            var document = new BsonDocument();
            while (reader.Read() && reader.TokenType != Newtonsoft.Json.JsonToken.EndObject)
            {
                var name = (string)reader.Value;
                ReadToken(reader);
                var value = serializer.Deserialize<BsonValue>(reader);
                document.Add(name, value);
            }
            return document;
        }

        private void WriteDocument(Newtonsoft.Json.JsonWriter writer, BsonDocument document, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var element in document)
            {
                writer.WritePropertyName(element.Name);
                serializer.Serialize(writer, element.Value, typeof(BsonValue));
            }
            writer.WriteEndObject();
        }
    }
}
