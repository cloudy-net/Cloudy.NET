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
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Integrations.JsonDotNet
{
    internal class BsonReaderAdapter : JsonReaderBase
    {
        // private fields
        private readonly IBsonReader _wrappedReader;

        // constructors
        public BsonReaderAdapter(IBsonReader wrappedReader)
        {
            _wrappedReader = wrappedReader;
        }

        // public properties
        public IBsonReader WrappedReader
        {
            get { return _wrappedReader; }
        }

        // public methods
        /// <inheritdoc/>
        public override void Close()
        {
            base.Close();

            if (CloseInput)
            {
                _wrappedReader.Close();
            }
        }

        /// <inheritdoc/>
        public override bool Read()
        {
            switch (_wrappedReader.State)
            {
                case BsonReaderState.Closed:
                case BsonReaderState.Done:
                    return false;

                case BsonReaderState.EndOfArray:
                    _wrappedReader.ReadEndArray();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.EndArray);
                    return true;

                case BsonReaderState.EndOfDocument:
                    _wrappedReader.ReadEndDocument();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.EndObject);
                    return true;

                case BsonReaderState.Initial:
                case BsonReaderState.Type:
                    if (_wrappedReader.State == BsonReaderState.Initial && _wrappedReader.IsAtEndOfFile())
                    {
                        SetCurrentToken(Newtonsoft.Json.JsonToken.None);
                        return false;
                    }
                    else
                    {
                        _wrappedReader.ReadBsonType();
                        return Read();
                    }

                case BsonReaderState.Name:
                    var name = _wrappedReader.ReadName();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.PropertyName, name);
                    return true;

                case BsonReaderState.Value:
                    ReadValue();
                    return true;

                default:
                    throw new Newtonsoft.Json.JsonReaderException(string.Format("Unexpected IBsonReader state: {0}.", _wrappedReader.State));
            }
        }

        // private methods
        private void ReadValue()
        {
            object jsonDotNetValue;
            switch (_wrappedReader.GetCurrentBsonType())
            {
                case BsonType.Array:
                    _wrappedReader.ReadStartArray();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.StartArray);
                    return;

                case BsonType.Binary:
                    var bsonBinaryData = _wrappedReader.ReadBinaryData();
                    switch (bsonBinaryData.SubType)
                    {
                        case BsonBinarySubType.UuidLegacy:
                            var guidRepresentation = GuidRepresentation.Unspecified;
                            var bsonReader = _wrappedReader as BsonReader;
                            if (bsonReader != null)
                            {
                                guidRepresentation = bsonReader.Settings.GuidRepresentation;
                            }
                            jsonDotNetValue = GuidConverter.FromBytes(bsonBinaryData.Bytes, guidRepresentation);
                            break;

                        case BsonBinarySubType.UuidStandard:
                            jsonDotNetValue = GuidConverter.FromBytes(bsonBinaryData.Bytes, GuidRepresentation.Standard);
                            break;

                        default:
                            jsonDotNetValue = bsonBinaryData.Bytes;
                            break;
                    }
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Bytes, jsonDotNetValue, bsonBinaryData);
                    return;

                case BsonType.Boolean:
                    var booleanValue = _wrappedReader.ReadBoolean();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Boolean, booleanValue, (BsonBoolean)booleanValue);
                    return;

                case BsonType.DateTime:
                    var bsonDateTime = new BsonDateTime(_wrappedReader.ReadDateTime());
                    if (bsonDateTime.IsValidDateTime)
                    {
                        jsonDotNetValue = bsonDateTime.ToUniversalTime();
                    }
                    else
                    {
                        jsonDotNetValue = bsonDateTime.MillisecondsSinceEpoch;
                    }
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Date, jsonDotNetValue, bsonDateTime);
                    return;

                case BsonType.Document:
                    _wrappedReader.ReadStartDocument();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.StartObject);
                    return;

                case BsonType.Double:
                    var bsonDouble = new BsonDouble(_wrappedReader.ReadDouble());
                    switch (FloatParseHandling)
                    {
                        case Newtonsoft.Json.FloatParseHandling.Decimal:
                            jsonDotNetValue = Convert.ToDecimal(bsonDouble);
                            break;

                        case Newtonsoft.Json.FloatParseHandling.Double:
                            jsonDotNetValue = bsonDouble.Value;
                            break;

                        default:
                            throw new NotSupportedException(string.Format("Unexpected FloatParseHandling value: {0}.", FloatParseHandling));
                    }
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Float, jsonDotNetValue, bsonDouble);
                    return;

                case BsonType.Int32:
                    var bsonInt32 = (BsonInt32)_wrappedReader.ReadInt32();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Integer, (long)bsonInt32.Value, bsonInt32);
                    return;

                case BsonType.Int64:
                    var bsonInt64 = (BsonInt64)_wrappedReader.ReadInt64();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Integer, bsonInt64.Value, bsonInt64);
                    return;

                case BsonType.JavaScript:
                    {
                        var code = _wrappedReader.ReadJavaScript();
                        var bsonJavaScript = new BsonJavaScript(code);
                        SetCurrentToken(Newtonsoft.Json.JsonToken.String, code, bsonJavaScript);
                    }
                    return;

                case BsonType.JavaScriptWithScope:
                    {
                        var code = _wrappedReader.ReadJavaScriptWithScope();
                        var context = BsonDeserializationContext.CreateRoot(_wrappedReader);
                        var scope = BsonDocumentSerializer.Instance.Deserialize<BsonDocument>(context);
                        var bsonJavaScriptWithScope = new BsonJavaScriptWithScope(code, scope);
                        SetCurrentToken(Newtonsoft.Json.JsonToken.String, code, bsonJavaScriptWithScope);
                    }
                    return;

                case BsonType.MaxKey:
                    _wrappedReader.ReadMaxKey();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Undefined, null, BsonMaxKey.Value);
                    return;

                case BsonType.MinKey:
                    _wrappedReader.ReadMinKey();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Undefined, null, BsonMinKey.Value);
                    return;

                case BsonType.Null:
                    _wrappedReader.ReadNull();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Null, null, BsonNull.Value);
                    return;

                case BsonType.ObjectId:
                    var bsonObjectId = new BsonObjectId(_wrappedReader.ReadObjectId());
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Bytes, bsonObjectId.Value.ToByteArray(), bsonObjectId);
                    return;

                case BsonType.RegularExpression:
                    var bsonRegularExpression = _wrappedReader.ReadRegularExpression();
                    var pattern = bsonRegularExpression.Pattern;
                    var options = bsonRegularExpression.Options;
                    jsonDotNetValue = "/" + pattern.Replace("/", "\\/") + "/" + options;
                    SetCurrentToken(Newtonsoft.Json.JsonToken.String, jsonDotNetValue, bsonRegularExpression);
                    return;

                case BsonType.String:
                    var stringValue = _wrappedReader.ReadString();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.String, stringValue, (BsonString)stringValue);
                    return;

                case BsonType.Symbol:
                    var bsonSymbol = BsonSymbolTable.Lookup(_wrappedReader.ReadSymbol());
                    SetCurrentToken(Newtonsoft.Json.JsonToken.String, bsonSymbol.Name, bsonSymbol);
                    return;

                case BsonType.Timestamp:
                    var bsonTimestamp = new BsonTimestamp(_wrappedReader.ReadTimestamp());
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Integer, bsonTimestamp.Value, bsonTimestamp);
                    return;

                case BsonType.Undefined:
                    _wrappedReader.ReadUndefined();
                    SetCurrentToken(Newtonsoft.Json.JsonToken.Undefined, null, BsonUndefined.Value);
                    return;

                default:
                    var message = string.Format("Unexpected BsonType: {0}.", _wrappedReader.GetCurrentBsonType());
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }
    }
}
