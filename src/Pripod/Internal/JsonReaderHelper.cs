using Pripod.Internal.Utf8Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pripod.Internal
{
    internal static class JsonReaderHelper
    {
        public delegate T Create<T>(ref JsonReader reader);
        public static readonly IReadOnlyDictionary<string, string> EmptyStringDictionary = new Dictionary<string, string>();

        public static T[] ReadAsArray<T>(ref JsonReader reader, Create<T> factory)
        {
            reader.ReadIsBeginArrayWithVerify();

            var items = new List<T>();
            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.BeginObject:
                        items.Add(factory(ref reader));
                        break;
                    case JsonToken.EndArray:
                        reader.ReadNext();
                        return items.ToArray();
                }
            }
        }

        public static IReadOnlyDictionary<string, string> ReadAsStringDictionary(ref JsonReader reader)
        {
            var dict = new Dictionary<string, string>();
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            dict[propName] = reader.ReadString();
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return dict;
                }
            }
        }
    }
}
