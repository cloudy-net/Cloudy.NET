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
    /// Represents a JsonConverter for ObjectId values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class ObjectIdConverter : JsonConverterBase<ObjectId>
    {
        #region static
        private static readonly ObjectIdConverter __instance = new ObjectIdConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="ObjectIdConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="ObjectIdConverter"/>.
        /// </value>
        public static ObjectIdConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.ObjectId)
            {
                return ((BsonObjectId)adapter.BsonValue).Value;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Bytes:
                    return new ObjectId((byte[])reader.Value);

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader);

                default:
                    var message = string.Format("Error reading ObjectId. Unexpected token: {0}.", reader.TokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        /// <inheritdoc/>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var objectId = (ObjectId)value;

            var adapter = writer as BsonWriterAdapter;
            if (adapter != null)
            {
                adapter.WriteObjectId(objectId);
            }
            else
            {
                var jsonDotNetBsonWriter = writer as Newtonsoft.Json.Bson.BsonWriter;
                if (jsonDotNetBsonWriter != null)
                {
                    jsonDotNetBsonWriter.WriteObjectId(objectId.ToByteArray());
                }
                else
                {
                    WriteExtendedJson(writer, objectId);
                }
            }
        }

        // private methods
        private ObjectId ReadExtendedJson(Newtonsoft.Json.JsonReader reader)
        {
            ReadExpectedPropertyName(reader, "$oid");
            var hex = ReadStringValue(reader);
            ReadEndObject(reader);

            return ObjectId.Parse(hex);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, ObjectId value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$oid");
            writer.WriteValue(BsonUtils.ToHexString(value.ToByteArray()));
            writer.WriteEndObject();
        }
    }
}
