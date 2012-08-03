using System;

namespace TeamNotification_Library.Service.FileSystem
{
    public class Base64Encoder : IHandleEncoding
    {
        public string Decode(string encodedData)
        {
            var encodedDataAsBytes = Convert.FromBase64String(encodedData);
            return System.Text.Encoding.Unicode.GetString(encodedDataAsBytes);
        }

        public string Encode(string value)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.Unicode.GetBytes(value);
            return Convert.ToBase64String(toEncodeAsBytes);
        }
    }
}