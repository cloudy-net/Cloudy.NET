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
    /// Represents a JsonConverter for BsonNull values.
    /// </summary>
    /// <seealso cref="MongoDB.Integrations.JsonDotNet.Converters.JsonConverterBase{T}" />
    public class BsonNullConverter : JsonConverterBase<BsonNull>
    {
        #region static
        private static readonly BsonNullConverter __instance = new BsonNullConverter();

        /// <summary>
        /// Gets a pre-created instance of a <see cref="BsonNullConverter"/>.
        /// </summary>
        /// <value>
        /// A <see cref="BsonNullConverter"/>.
        /// </value>
        public static BsonNullConverter Instance
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
                    return BsonNull.Value;

                default:
                    var message = string.Format("Error reading BsonNull. Unexpected token: {0}.", reader.TokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
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
                writer.WriteNull();
            }
        }
    }
}
