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
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Integrations.JsonDotNet.Converters
{
    /// <summary>
    /// Represents a JsonConverter for BsonJavaScriptWithScope values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonJavaScriptWithScopeConverter : JsonConverterBase<BsonJavaScriptWithScope>
    {
        #region static
        private static readonly BsonJavaScriptWithScopeConverter __instance = new BsonJavaScriptWithScopeConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonJavaScriptWithScopeConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonJavaScriptWithScopeConverter"/>.
        /// </value>
        public static BsonJavaScriptWithScopeConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.JavaScriptWithScope)
            {
                return (BsonJavaScriptWithScope)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader, serializer);

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                default:
                    var message = string.Format("Error reading BsonJavaScriptWithScope. Unexpected token: {0}.", reader.TokenType);
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
                var bsonJavaScriptWithScope = (BsonJavaScriptWithScope)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteJavaScriptWithScope(bsonJavaScriptWithScope.Code);
                    var serializationContext = BsonSerializationContext.CreateRoot(adapter.WrappedWriter);
                    BsonDocumentSerializer.Instance.Serialize(serializationContext, bsonJavaScriptWithScope.Scope);
                }
                else
                {
                    WriteExtendedJson(writer, bsonJavaScriptWithScope, serializer);
                }
            }
        }

        // private methods
        private BsonJavaScriptWithScope ReadExtendedJson(Newtonsoft.Json.JsonReader reader, Newtonsoft.Json.JsonSerializer serializer)
        {
            ReadExpectedPropertyName(reader, "$code");
            var code = reader.ReadAsString();
            ReadExpectedPropertyName(reader, "$scope");
            ReadStartObject(reader);
            var scope = serializer.Deserialize<BsonDocument>(reader);
            ReadEndObject(reader);

            return new BsonJavaScriptWithScope(code, scope);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, BsonJavaScriptWithScope value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$code");
            writer.WriteValue(value.Code);
            writer.WritePropertyName("$scope");
            serializer.Serialize(writer, value.Scope, typeof(BsonDocument));
            writer.WriteEndObject();
        }
    }
}
