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
using System.Linq;
using MongoDB.Bson;

namespace MongoDB.Integrations.JsonDotNet.Converters
{
    /// <summary>
    /// Represents a JsonConverter for BsonBinaryData values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{BsonBinaryData}" />
    public class BsonBinaryDataConverter : JsonConverterBase<BsonBinaryData>
    {
        #region static
        private static readonly BsonBinaryDataConverter __instance = new BsonBinaryDataConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonBinaryDataConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonBinaryDataConverter"/>.
        /// </value>
        public static BsonBinaryDataConverter Instance
        {
            get { return __instance; }
        }
        #endregion

        // public methods
        /// <inheritdoc/>
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var adapter = reader as BsonReaderAdapter;
            if (adapter != null && adapter.BsonValue != null && adapter.BsonValue.BsonType == BsonType.Binary)
            {
                return (BsonBinaryData)adapter.BsonValue;
            }

            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Bytes:
                    if (reader.Value is Guid)
                    {
                        var message = "Reading a Guid from a Json.NET JsonReader is not supported.";
                        throw new Newtonsoft.Json.JsonReaderException(message);
                    }
                    else
                    {
                        return new BsonBinaryData((byte[])reader.Value);
                    }

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.StartObject:
                    return ReadExtendedJson(reader, serializer);

                default:
                    {
                        var message = string.Format("Error reading BsonBinaryData. Unexpected token: {0}.", reader.TokenType);
                        throw new Newtonsoft.Json.JsonReaderException(message);
                    }
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
                var binaryData = (BsonBinaryData)value;

                var adapter = writer as BsonWriterAdapter;
                if (adapter != null)
                {
                    adapter.WriteBinaryData(binaryData);
                }
                else
                {
                    var jsonDotNetBsonWriter = writer as Newtonsoft.Json.Bson.BsonWriter;
                    if (jsonDotNetBsonWriter != null && binaryData.SubType == BsonBinarySubType.Binary)
                    {
                        jsonDotNetBsonWriter.WriteValue(binaryData.Bytes);
                    }
                    else
                    {
                        WriteExtendedJson(writer, binaryData, serializer);
                    }
                }
            }
        }

        // private methods
        private BsonBinaryData ReadExtendedJson(Newtonsoft.Json.JsonReader reader, Newtonsoft.Json.JsonSerializer serializer)
        {
            ReadExpectedPropertyName(reader, "$binary");
            var bytes = Convert.FromBase64String(ReadStringValue(reader));
            ReadExpectedPropertyName(reader, "$type");
            BsonBinarySubType subType;
            ReadToken(reader);
            switch (reader.TokenType)
            {
                case Newtonsoft.Json.JsonToken.Integer:
                    subType = (BsonBinarySubType)(reader.Value is int ? (int)reader.Value : Convert.ToInt32(reader.Value, NumberFormatInfo.InvariantInfo));
                    break;
                
                case Newtonsoft.Json.JsonToken.String:
                    subType = (BsonBinarySubType)BsonUtils.ParseHexString((string)reader.Value).Single();
                    break;

                default:
                    var message = string.Format("Cannot read BsonBinarySubType from token: {0}.", reader.TokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
            ReadEndObject(reader);

            GuidRepresentation guidRepresentation;
            switch (subType)
            {
                case BsonBinarySubType.UuidLegacy: guidRepresentation = GuidRepresentation.CSharpLegacy; break;
                case BsonBinarySubType.UuidStandard: guidRepresentation = GuidRepresentation.Standard; break;
                default: guidRepresentation = GuidRepresentation.Unspecified; break;
            }

            return new BsonBinaryData(bytes, subType, guidRepresentation);
        }

        private void WriteExtendedJson(Newtonsoft.Json.JsonWriter writer, BsonBinaryData binaryData, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$binary");
            writer.WriteValue(Convert.ToBase64String(binaryData.Bytes));
            writer.WritePropertyName("$type");
            writer.WriteValue(((int)binaryData.SubType).ToString("x2"));
            writer.WriteEnd();
        }
    }
}
