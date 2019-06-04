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
using System.Collections.Generic;
using System.Globalization;
using MongoDB.Bson;

namespace MongoDB.Integrations.JsonDotNet.Converters
{
    /// <summary>
    /// Represents a JsonConverter for BsonValue values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonValueConverter : JsonConverterBase<BsonValue>
    {
        #region static
        // private static properties
        private static readonly Dictionary<Type, BsonType> __bsonValueTypeToBsonTypeMap;
        private static readonly BsonValueConverter __instance = new BsonValueConverter();

        // static constructor
        static BsonValueConverter()
        {
            __bsonValueTypeToBsonTypeMap = new Dictionary<Type, BsonType>
            {
                { typeof(BsonArray), BsonType.Array },
                { typeof(BsonBinaryData), BsonType.Binary },
                { typeof(BsonBoolean), BsonType.Boolean },
                { typeof(BsonDateTime), BsonType.DateTime },
                { typeof(BsonDocument), BsonType.Document },
                { typeof(BsonDouble), BsonType.Double },
                { typeof(BsonInt32), BsonType.Int32 },
                { typeof(BsonInt64), BsonType.Int64 },
                { typeof(BsonJavaScript), BsonType.JavaScript },
                { typeof(BsonJavaScriptWithScope), BsonType.JavaScriptWithScope },
                { typeof(BsonMaxKey), BsonType.MaxKey },
                { typeof(BsonMinKey), BsonType.MinKey },
                { typeof(BsonNull), BsonType.Null },
                { typeof(BsonObjectId), BsonType.ObjectId },
                { typeof(BsonRegularExpression), BsonType.RegularExpression },
                { typeof(BsonString), BsonType.String },
                { typeof(BsonSymbol), BsonType.Symbol },
                { typeof(BsonTimestamp), BsonType.Timestamp },
                { typeof(BsonUndefined), BsonType.Undefined }
            };
        }

        // public static properties
        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonValueConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonValueConverter"/>.
        /// </value>
        public static BsonValueConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(BsonValue).IsAssignableFrom(objectType);
        }

        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            BsonType bsonType;
            if (objectType == typeof(BsonValue))
            {
                var adapter = reader as BsonReaderAdapter;
                if (adapter != null && adapter.BsonValue != null)
                {
                    bsonType = adapter.BsonValue.BsonType;
                }
                else
                {
                    bsonType = MapTokenTypeToBsonType(reader.TokenType);
                }
            }
            else
            {
                bsonType = MapObjectTypeToBsonType(objectType);
            }

            switch (bsonType)
            {
                case BsonType.Array: return BsonArrayConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Binary: return BsonBinaryDataConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Boolean: return BsonBooleanConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.DateTime: return BsonDateTimeConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Document: 
                    var document = (BsonDocument)BsonDocumentConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                    return ParseExtendedJson(document);
                case BsonType.Double: return BsonDoubleConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Int32: return BsonInt32Converter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Int64: return BsonInt64Converter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.JavaScript: return BsonJavaScriptConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.JavaScriptWithScope: return BsonJavaScriptWithScopeConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.MaxKey: return BsonMaxKeyConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.MinKey: return BsonMinKeyConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Null: return BsonNullConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.ObjectId: return BsonObjectIdConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.RegularExpression: return BsonRegularExpressionConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.String: return BsonStringConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Symbol: return BsonSymbolConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Timestamp: return BsonTimestampConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                case BsonType.Undefined: return BsonUndefinedConverter.Instance.ReadJson(reader, objectType, existingValue, serializer);
                default:
                    var message = string.Format("Unexpected BsonType: {0}.", bsonType);
                    throw new Newtonsoft.Json.JsonSerializationException(message);
            }
        }

        /// <inheritdoc/>
        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull(); // TODO: does C# null need to round trip?
            }
            else
            {
                var bsonType = ((BsonValue)value).BsonType;
                switch (bsonType)
                {
                    case BsonType.Array: BsonArrayConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Binary: BsonBinaryDataConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Boolean: BsonBooleanConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.DateTime: BsonDateTimeConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Document: BsonDocumentConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Double: BsonDoubleConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Int32: BsonInt32Converter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Int64: BsonInt64Converter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.JavaScript: BsonJavaScriptConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.JavaScriptWithScope: BsonJavaScriptWithScopeConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.MaxKey: BsonMaxKeyConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.MinKey: BsonMinKeyConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Null: BsonNullConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.ObjectId: BsonObjectIdConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.RegularExpression: BsonRegularExpressionConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.String: BsonStringConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Symbol: BsonSymbolConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Timestamp: BsonTimestampConverter.Instance.WriteJson(writer, value, serializer); break;
                    case BsonType.Undefined: BsonUndefinedConverter.Instance.WriteJson(writer, value, serializer); break;
                    default:
                        var message = string.Format("Unexpected BsonType: {0}.", bsonType);
                        throw new Newtonsoft.Json.JsonSerializationException(message);
                }
            }
        }

        // private methods
        private BsonType MapObjectTypeToBsonType(Type objectType)
        {
            BsonType bsonType;
            if (__bsonValueTypeToBsonTypeMap.TryGetValue(objectType, out bsonType))
            {
                return bsonType;
            }

            var message = string.Format("Error reading BsonValue. Unexpected objectType type: {0}.", objectType.FullName);
            throw new Newtonsoft.Json.JsonReaderException(message);
        }

        private BsonType MapTokenTypeToBsonType(Newtonsoft.Json.JsonToken tokenType)
        {
            switch (tokenType)
            {
                case Newtonsoft.Json.JsonToken.Boolean: return BsonType.Boolean;
                case Newtonsoft.Json.JsonToken.Bytes: return BsonType.Binary;
                case Newtonsoft.Json.JsonToken.Date: return BsonType.DateTime;
                case Newtonsoft.Json.JsonToken.Float: return BsonType.Double;
                case Newtonsoft.Json.JsonToken.Null: return BsonType.Null;
                case Newtonsoft.Json.JsonToken.StartArray: return BsonType.Array;
                case Newtonsoft.Json.JsonToken.String: return BsonType.String;
                case Newtonsoft.Json.JsonToken.Undefined: return BsonType.Undefined;
                case Newtonsoft.Json.JsonToken.Integer: return BsonType.Int64;
                case Newtonsoft.Json.JsonToken.StartObject: return BsonType.Document;

                default:
                    var message = string.Format("Error reading BsonValue. Unexpected token: {0}.", tokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        private BsonValue ParseExtendedJson(BsonDocument document)
        {
            if (document.ElementCount > 0)
            {
                switch (document.GetElement(0).Name)
                {
                    case "$binary":
                        if (document.ElementCount == 2 && document.GetElement(1).Name == "$type")
                        {
                            var bytes = Convert.FromBase64String(document[0].AsString);
                            var subType = (BsonBinarySubType)(int)BsonUtils.ParseHexString(document[1].AsString)[0];
                            return new BsonBinaryData(bytes, subType);
                        }
                        break;

                    case "$code":
                        if (document.ElementCount == 1)
                        {
                            var code = document[0].AsString;
                            return new BsonJavaScript(code);
                        }
                        else if (document.ElementCount == 2 && document.GetElement(1).Name == "$scope")
                        {
                            var code = document[0].AsString;
                            var scope = document[1].AsBsonDocument;
                            return new BsonJavaScriptWithScope(code, scope);
                        }
                        break;

                    case "$date":
                        if (document.ElementCount == 1)
                        {
                            switch (document[0].BsonType)
                            {
                                case BsonType.DateTime:
                                    return document[0].AsBsonDateTime;

                                case BsonType.Document:
                                    {
                                        var dateDocument = document[0].AsBsonDocument;
                                        if (dateDocument.ElementCount == 1 && dateDocument.GetElement(0).Name == "$numberLong")
                                        {
                                            var formattedString = dateDocument[0].AsString;
                                            var millisecondsSinceEpoch = long.Parse(formattedString, NumberFormatInfo.InvariantInfo);
                                            return new BsonDateTime(millisecondsSinceEpoch);
                                        }
                                    }
                                    break;

                                case BsonType.Double:
                                case BsonType.Int32:
                                case BsonType.Int64:
                                    {
                                        var millisecondsSinceEpoch = document[0].ToInt64();
                                        return new BsonDateTime(millisecondsSinceEpoch);
                                    }

                                case BsonType.String:
                                    {
                                        var formattedString = document[0].AsString;
                                        var dateTime = DateTime.Parse(formattedString, DateTimeFormatInfo.InvariantInfo);
                                        return new BsonDateTime(dateTime);
                                    }

                            }
                        }
                        break;

                    case "$maxKey":
                        if (document.ElementCount == 1)
                        {
                            return BsonMaxKey.Value;
                        }
                        break;

                    case "$minKey":
                        if (document.ElementCount == 1)
                        {
                            return BsonMinKey.Value;
                        }
                        break;

                    case "$oid":
                        if (document.ElementCount == 1)
                        {
                            var hexBytes = document[0].AsString;
                            var objectId = ObjectId.Parse(hexBytes);
                            return new BsonObjectId(objectId);
                        }
                        break;

                    case "$regex":
                        if (document.ElementCount == 2 && document.GetElement(1).Name == "$options")
                        {
                            var pattern = document[0].AsString;
                            var options = document[1].AsString;
                            return new BsonRegularExpression(pattern, options);
                        }
                        break;

                    case "$symbol":
                        if (document.ElementCount == 1)
                        {
                            var name = document[0].AsString;
                            return BsonSymbolTable.Lookup(name);
                        }
                        break;

                    case "$timestamp":
                        if (document.ElementCount == 1)
                        {
                            var timestampDocument = document[0].AsBsonDocument;
                            var timestamp = timestampDocument[0].ToInt32();
                            var increment = timestampDocument[1].ToInt32();
                            return new BsonTimestamp(timestamp, increment);
                        }
                        break;
                }
            }

            return document;
        }
    }
}
