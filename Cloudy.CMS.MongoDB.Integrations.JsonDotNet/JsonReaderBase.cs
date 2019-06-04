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
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace MongoDB.Integrations.JsonDotNet
{
    internal abstract class JsonReaderBase : Newtonsoft.Json.JsonReader
    {
        // private fields
        private BsonValue _bsonValue;
        private MongoPosition _currentPosition;
        private Stack<MongoPosition> _positionsStack;
        private Newtonsoft.Json.JsonToken _tokenType;
        private object _value;

        // constructors
        protected JsonReaderBase()
        {
            _currentPosition = new MongoPosition(MongoContainerType.None);
            _positionsStack = new Stack<MongoPosition>();
        }

        // public properties
        public BsonValue BsonValue
        {
            get { return _bsonValue; }
        }

        /// <inheritdoc/>
        public override int Depth
        {
            get
            {
                var depth = _positionsStack.Count();

                // depth shouldn't increase until after the start token
                if (TokenType == Newtonsoft.Json.JsonToken.StartObject || TokenType == Newtonsoft.Json.JsonToken.StartArray)
                {
                    depth -= 1;
                }

                return depth;
            }
        }

        /// <inheritdoc/>
        public override string Path
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var position in _positionsStack.Reverse().Concat(new[] { _currentPosition }))
                {
                    if (position.PropertyName != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(".");
                        }
                        sb.Append(position.PropertyName);
                    }

                    if (position.HasIndex && position.Position >= 0)
                    {
                        sb.Append(string.Format("[{0}]", position.Position));
                    }
                }
                return sb.ToString();
            }
        }

        /// <inheritdoc/>
        public override Newtonsoft.Json.JsonToken TokenType
        {
            get { return _tokenType; }
        }

        /// <inheritdoc/>
        public override object Value
        {
            get { return _value; }
        }

        /// <inheritdoc/>
        public override Type ValueType
        {
            get { return _value != null ? _value.GetType() : null; }
        }

        // protected properties
        [Obsolete("This protected property is not supported.", true)]
        protected new State CurrentState
        {
            get { throw new NotSupportedException(); }
        }

        // public methods
        /// <inheritdoc/>
        public override byte[] ReadAsBytes()
        {
            if (!ReadSkippingComments())
            {
                ReplaceCurrentToken(Newtonsoft.Json.JsonToken.None);
                return null;
            }

            byte[] bytes;
            switch (_tokenType)
            {
                case Newtonsoft.Json.JsonToken.Bytes:
                    return (byte[])_value;

                case Newtonsoft.Json.JsonToken.EndArray:
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    var stringValue = (string)Value;
                    if (stringValue.Length == 0)
                    {
                        bytes = new byte[0];
                    }
                    else
                    {
                        bytes = Convert.FromBase64String(stringValue);
                    }
                    ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Bytes, bytes, _bsonValue);
                    return bytes;

                case Newtonsoft.Json.JsonToken.StartArray:
                    bytes = ReadBytesArray();
                    ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Bytes, bytes, null);
                    return bytes;

                case Newtonsoft.Json.JsonToken.StartObject:
                    bytes = ReadBytesWrappedInTypeObject();
                    ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Bytes, bytes, null);
                    return bytes;

                default:
                    var message = string.Format("Error reading bytes. Unexpected token: {0}.", _tokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        /// <inheritdoc/>
        public override DateTime? ReadAsDateTime()
        {
            if (!ReadSkippingComments())
            {
                ReplaceCurrentToken(Newtonsoft.Json.JsonToken.None);
                return null;
            }

            string message;
            switch (_tokenType)
            {
                case Newtonsoft.Json.JsonToken.Date:
                    return (DateTime)_value;

                case Newtonsoft.Json.JsonToken.EndArray:
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    var stringValue = (string)Value;
                    if (string.IsNullOrEmpty(stringValue))
                    {
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Null);
                        return null;
                    }

                    // TODO: handle all the different ways Json.NET handles DateTimes and TimeZones
                    DateTime dateTime;
                    if (DateTime.TryParse(stringValue, Culture, DateTimeStyles.RoundtripKind, out dateTime))
                    {
                        dateTime = BsonUtils.ToUniversalTime(dateTime);
                        SetCurrentToken(Newtonsoft.Json.JsonToken.Date, dateTime, _bsonValue);
                        return dateTime;
                    }
                    message = string.Format("Could not convert string to DateTime: {0}.", stringValue);
                    throw new Newtonsoft.Json.JsonReaderException(message);

                default:
                    message = string.Format("Error reading date. Unexpected token: {0}.", _tokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        /// <inheritdoc/>
        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            if (!ReadSkippingComments())
            {
                ReplaceCurrentToken(Newtonsoft.Json.JsonToken.None);
                return null;
            }

            DateTimeOffset dateTimeOffset;
            string message;
            switch (_tokenType)
            {
                case Newtonsoft.Json.JsonToken.Date:
                    if (_value is DateTime)
                    {
                        dateTimeOffset = new DateTimeOffset((DateTime)_value);
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Date, dateTimeOffset, _bsonValue);
                    }
                    return (DateTimeOffset)_value;

                case Newtonsoft.Json.JsonToken.EndArray:
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    var stringValue = (string)Value;
                    if (string.IsNullOrEmpty(stringValue))
                    {
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Null);
                        return null;
                    }

                    // TODO: handle all the different ways Json.NET handles DateTimes and TimeZones
                    if (DateTimeOffset.TryParse(stringValue, Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
                    {
                        SetCurrentToken(Newtonsoft.Json.JsonToken.Date, dateTimeOffset, _bsonValue);
                        return dateTimeOffset;
                    }
                    message = string.Format("Could not convert string to DateTimeOffset: {0}.", stringValue);
                    throw new Newtonsoft.Json.JsonReaderException(message);

                default:
                    message = string.Format("Error reading date. Unexpected token: {0}.", _tokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        /// <inheritdoc/>
        public override decimal? ReadAsDecimal()
        {
            if (!ReadSkippingComments())
            {
                ReplaceCurrentToken(Newtonsoft.Json.JsonToken.None);
                return null;
            }

            decimal decimalValue;
            string message;
            switch (_tokenType)
            {
                case Newtonsoft.Json.JsonToken.EndArray:
                    return null;

                case Newtonsoft.Json.JsonToken.Float:
                case Newtonsoft.Json.JsonToken.Integer:
                    if (!(_value is decimal))
                    {
                        decimalValue = Convert.ToDecimal(_value, CultureInfo.InvariantCulture);
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Float, decimalValue, _bsonValue);
                    }
                    return (decimal)_value;

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    var stringValue = (string)_value;
                    if (string.IsNullOrEmpty(stringValue))
                    {
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Null);
                        return null;
                    }
                    if (decimal.TryParse(stringValue, NumberStyles.Number, Culture, out decimalValue))
                    {
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Float, decimalValue, _bsonValue);
                        return decimalValue;
                    }
                    message = string.Format("Could not convert string to decimal: {0}.", stringValue);
                    throw new Newtonsoft.Json.JsonReaderException(message);

                default:
                    message = string.Format("Error reading decimal. Unexpected token: {0}.", _tokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        /// <inheritdoc/>
        public override int? ReadAsInt32()
        {
            if (!ReadSkippingComments())
            {
                ReplaceCurrentToken(Newtonsoft.Json.JsonToken.None);
                return null;
            }

            switch (_tokenType)
            {
                case Newtonsoft.Json.JsonToken.EndArray:
                    return null;

                case Newtonsoft.Json.JsonToken.Float:
                case Newtonsoft.Json.JsonToken.Integer:
                    if (!(Value is int))
                    {
                        var intValue = Convert.ToInt32(Value, CultureInfo.InvariantCulture);
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Integer, intValue);
                    }
                    return (int)Value;

                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    var stringValue = (string)Value;
                    if (string.IsNullOrEmpty(stringValue))
                    {
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Null);
                        return null;
                    }
                    else
                    {
                        int intValue;
                        if (!int.TryParse(stringValue, NumberStyles.Integer, Culture, out intValue))
                        {
                            var message = string.Format("Could not convert string to integer: {0}.", stringValue);
                            throw new Newtonsoft.Json.JsonReaderException(message);
                        }
                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.Integer, intValue);
                        return intValue;
                    }

                default:
                    {
                        var message = string.Format("Error reading integer. Unexpected token: {0}.", _tokenType);
                        throw new Newtonsoft.Json.JsonReaderException(message);
                    }
            }
        }

        /// <inheritdoc/>
        public override string ReadAsString()
        {
            if (!ReadSkippingComments())
            {
                ReplaceCurrentToken(Newtonsoft.Json.JsonToken.None);
                return null;
            }

            switch (_tokenType)
            {
                case Newtonsoft.Json.JsonToken.EndArray:
                case Newtonsoft.Json.JsonToken.Null:
                    return null;

                case Newtonsoft.Json.JsonToken.String:
                    return (string)Value;

                default:
                    if (_tokenType.IsPrimitive() && Value != null)
                    {
                        string stringValue;
                        var formattableValue = Value as IFormattable;
                        if (formattableValue != null)
                        {
                            stringValue = formattableValue.ToString(null, Culture);
                        }
                        else
                        {
                            stringValue = Value.ToString();
                        }

                        ReplaceCurrentToken(Newtonsoft.Json.JsonToken.String, stringValue, _bsonValue);
                        return stringValue;
                    }

                    var message = string.Format("Error reading string. Unexpected token: {0}.", _tokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        // protected methods
        protected virtual void ReplaceCurrentToken(Newtonsoft.Json.JsonToken tokenType, object value = null, BsonValue bsonValue = null)
        {
            if (!tokenType.IsScalar())
            {
                var message = string.Format("New tokenType must be scalar, not: {0}.", tokenType);
                throw new ArgumentException(message, "tokenType");
            }
            if (!_tokenType.IsScalar() && _tokenType != Newtonsoft.Json.JsonToken.EndArray && _tokenType != Newtonsoft.Json.JsonToken.EndObject)
            {
                var message = string.Format("Current tokenType must be scalar, not: {0}.", _tokenType);
                throw new InvalidOperationException(message);
            }

            _tokenType = tokenType;
            _value = value;
            _bsonValue = bsonValue;
        }

        protected virtual void SetCurrentToken(Newtonsoft.Json.JsonToken tokenType, object value = null, BsonValue bsonValue = null)
        {
            _tokenType = tokenType;
            _value = value;
            _bsonValue = bsonValue;

            switch (tokenType)
            {
                case Newtonsoft.Json.JsonToken.Boolean:
                case Newtonsoft.Json.JsonToken.Bytes:
                case Newtonsoft.Json.JsonToken.Date:
                case Newtonsoft.Json.JsonToken.Float:
                case Newtonsoft.Json.JsonToken.Integer:
                case Newtonsoft.Json.JsonToken.Null:
                case Newtonsoft.Json.JsonToken.Raw:
                case Newtonsoft.Json.JsonToken.String:
                case Newtonsoft.Json.JsonToken.Undefined:
                    UpdateIndex();
                    break;

                case Newtonsoft.Json.JsonToken.EndArray:
                    ValidateEndToken(tokenType, MongoContainerType.Array);
                    break;

                case Newtonsoft.Json.JsonToken.EndConstructor:
                    ValidateEndToken(tokenType, MongoContainerType.Constructor);
                    break;

                case Newtonsoft.Json.JsonToken.EndObject:
                    ValidateEndToken(tokenType, MongoContainerType.Object);
                    break;

                case Newtonsoft.Json.JsonToken.PropertyName:
                    _currentPosition.PropertyName = (string)value;
                    break;

                case Newtonsoft.Json.JsonToken.StartArray:
                    Push(MongoContainerType.Array);
                    break;

                case Newtonsoft.Json.JsonToken.StartConstructor:
                    Push(MongoContainerType.Constructor);
                    break;

                case Newtonsoft.Json.JsonToken.StartObject:
                    Push(MongoContainerType.Object);
                    break;

                case Newtonsoft.Json.JsonToken.Comment:
                case Newtonsoft.Json.JsonToken.None:
                    break;

                default:
                    var message = string.Format("Unexpected token type: {0}.", tokenType);
                    throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }

        [Obsolete("This protected method is not supported. Use SetCurrentToken instead.", true)]
        protected new void SetToken(Newtonsoft.Json.JsonToken newToken)
        {
            throw new NotSupportedException();
        }

        [Obsolete("This protected method is not supported. Use SetCurrentToken instead.", true)]
        protected new void SetToken(Newtonsoft.Json.JsonToken newToken, object value)
        {
            throw new NotSupportedException();
        }

        [Obsolete("This protected method is not supported.", true)]
        protected new void SetStateBasedOnCurrent()
        {
            throw new NotSupportedException();
        }

        // private methods
        private MongoContainerType Pop()
        {
            var poppedContainerType = _currentPosition.ContainerType;
            _currentPosition = _positionsStack.Pop();
            return poppedContainerType;
        }

        private void Push(MongoContainerType containerType)
        {
            UpdateIndex();

            _positionsStack.Push(_currentPosition);
            _currentPosition = new MongoPosition(containerType);

            // TODO: check for max depth exceeded
        }

        private byte[] ReadBytesArray()
        {
            var bytes = new List<byte>();

            var reachedEndArray = false;
            while (!reachedEndArray)
            {
                if (!Read())
                {
                    var message = "Unexpected end when reading bytes.";
                    throw new Newtonsoft.Json.JsonReaderException(message);
                }

                switch (_tokenType)
                {
                    case Newtonsoft.Json.JsonToken.Comment:
                        break;

                    case Newtonsoft.Json.JsonToken.EndArray:
                        reachedEndArray = true;
                        break;

                    case Newtonsoft.Json.JsonToken.Integer:
                        bytes.Add(Convert.ToByte(_value, CultureInfo.InvariantCulture));
                        break;

                    default:
                        var message = string.Format("Unexpected token when reading bytes: {0}.", _tokenType);
                        throw new Newtonsoft.Json.JsonReaderException(message);
                }
            }

            return bytes.ToArray();
        }

        private byte[] ReadBytesWrappedInTypeObject()
        {
            if (
                Read() && (_value as string) == "$type" &&
                Read() && (_value is string) && (_value as string).StartsWith("System.Byte[]", StringComparison.Ordinal) &&
                Read() && (_value as string) == "$value")
            {
                var bytes = ReadAsBytes();
                if (Read() && _tokenType == Newtonsoft.Json.JsonToken.EndObject)
                {
                    return bytes;
                }
            }

            var message = string.Format("Error reading bytes. Unexpected token: {0}.", Newtonsoft.Json.JsonToken.StartObject);
            throw new Newtonsoft.Json.JsonReaderException(message);
        }

        private bool ReadSkippingComments()
        {
            while (Read())
            {
                if (_tokenType != Newtonsoft.Json.JsonToken.Comment)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateIndex()
        {
            if (_currentPosition.HasIndex)
            {
                _currentPosition.Position++;
            }
        }

        private void ValidateEndToken(Newtonsoft.Json.JsonToken tokenType, MongoContainerType expectedContainerType)
        {
            var poppedContainerType = Pop();

            if (poppedContainerType != expectedContainerType)
            {
                var message = string.Format(
                    "JsonToken {0} is not valid for closing JsonType {1}.", tokenType, poppedContainerType);
                throw new Newtonsoft.Json.JsonReaderException(message);
            }
        }
    }
}
