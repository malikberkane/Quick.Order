using Newtonsoft.Json;
using System;

namespace MalikBerkane.MvvmToolkit
{
    public static class Utils
    {
        public static string FormatPluralOrSingular(this string word, bool isPlural = false)
        {
            return isPlural ? word + "s" : word;
        }


        public static T Clone<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }

    public class TinyIoC
    {

    }

}
