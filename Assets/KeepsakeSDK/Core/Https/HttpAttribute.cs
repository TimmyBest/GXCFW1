using System;
using System.Linq;

namespace KeepsakeSDK.Core.Https
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpAttribute : Attribute
    {
        public string Url { get; }

        public HttpAttribute(string url)
        {
            Url = url;
        }

        public static HttpAttribute GetAttributeCustom<T>(string method) where T : class
        {
            try
            {
                return ((HttpAttribute)typeof(T).GetMethod(method).GetCustomAttributes(typeof(HttpAttribute), false).FirstOrDefault());
            }
            catch (SystemException)
            {
                return null;
            }
        }
    }
}