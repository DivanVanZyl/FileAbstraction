using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAbstraction.Data
{
    internal static class DataOperationExensions
    {
        public static byte[] ObjectToByteArray<T>(this T o)
        {
            return Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(o, GetJsonSerializerOptions()));
        }
        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
    }
}
