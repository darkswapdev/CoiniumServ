#region License
// 
//     MIT License
//
//     CoiniumServ - Crypto Currency Mining Pool Server Software
//     Copyright (C) 2013 - 2017, CoiniumServ Project
//     Hüseyin Uslu, shalafiraistlin at gmail dot com
//     https://github.com/bonesoul/CoiniumServ
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// 
#endregion


using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoiniumServ.Daemon.Converters
{
    /// <summary>
    /// Custom json converter for all fields in getinfo() and getmininginfo() that doesn't adhere to standard naming convention.
    /// Property names are converted to lowercase and empty spaces are replaced appropriately.
    /// </summary>
    public class PropertyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var lowerCaseReader = new LowerCasePropertyNameJsonReader(new StringReader(reader.Value.ToString()));
            return serializer.Deserialize(lowerCaseReader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public class LowerCasePropertyNameJsonReader : JsonTextReader
    {
        public LowerCasePropertyNameJsonReader(TextReader textPCReader)
            : base(textPCReader)
        {
        }

        public override object Value
        {
            get
            {
                if (TokenType == JsonToken.PropertyName)
                {
                    if ((string)base.Value == "proof of work" || (string)base.Value == "Proof of Work" || (string)base.Value == "proof of stake" || (string)base.Value == "Proof of Stake")
                        return Regex.Replace(((string)base.Value).ToLower(), " ", "-"); // first convert to lowercase then replace spaces
                    else
                        return (string)base.Value;
                }

                return base.Value;
            }
        }
    }
}