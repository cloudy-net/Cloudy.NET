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


namespace MongoDB.Integrations.JsonDotNet
{
    internal static class JsonTokenExtensions
    {
        public static bool IsPrimitive(this Newtonsoft.Json.JsonToken token)
        {
            switch (token)
            {
                case Newtonsoft.Json.JsonToken.Integer:
                case Newtonsoft.Json.JsonToken.Float:
                case Newtonsoft.Json.JsonToken.String:
                case Newtonsoft.Json.JsonToken.Boolean:
                case Newtonsoft.Json.JsonToken.Undefined:
                case Newtonsoft.Json.JsonToken.Null:
                case Newtonsoft.Json.JsonToken.Date:
                case Newtonsoft.Json.JsonToken.Bytes:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsScalar(this Newtonsoft.Json.JsonToken token)
        {
            switch (token)
            {
                case Newtonsoft.Json.JsonToken.EndArray:
                case Newtonsoft.Json.JsonToken.EndConstructor:
                case Newtonsoft.Json.JsonToken.EndObject:
                case Newtonsoft.Json.JsonToken.StartArray:
                case Newtonsoft.Json.JsonToken.StartConstructor:
                case Newtonsoft.Json.JsonToken.StartObject:
                    return false;
                   
                default:
                    return true;
            }
        }
    }
}
