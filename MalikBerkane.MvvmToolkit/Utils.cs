using System;
using System.Collections.Generic;
using System.Text;

namespace MalikBerkane.MvvmToolkit
{
    public static class Utils
    {
        public static string FormatPluralOrSingular(this string word, bool isPlural = false)
        {
            return isPlural ? word + "s" : word;
        }
    }

    public class TinyIoC
    {

    }

}
