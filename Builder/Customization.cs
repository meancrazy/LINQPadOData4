using System.Globalization;

namespace OData4.Builder
{
    public static class Customization
    {
        /// <summary>
        /// Changes the text to use upper camel case, which upper case for the first character.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <returns>The converted text in upper camel case</returns>
        internal static string CustomizeNaming(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (text.Length == 1)
            {
                return char.ToUpperInvariant(text[0]).ToString(CultureInfo.InvariantCulture);
            }

            return char.ToUpperInvariant(text[0]) + text.Substring(1);
        }

        /// <summary>
        /// Changes the namespace to use upper camel case, which upper case for the first character of all segments.
        /// </summary>
        /// <param name="fullNamespace">Namespace to convert.</param>
        /// <returns>The converted namespace in upper camel case</returns>
        internal static string CustomizeNamespace(string fullNamespace)
        {
            if (string.IsNullOrEmpty(fullNamespace))
            {
                return fullNamespace;
            }

            var segs = fullNamespace.Split('.');
            var upperNamespace = string.Empty;
            var n = segs.Length;
            for (var i = 0; i < n; ++i)
            {
                upperNamespace += CustomizeNaming(segs[i]);
                upperNamespace += (i == n - 1 ? string.Empty : ".");
            }

            return upperNamespace;
        }
    }
}