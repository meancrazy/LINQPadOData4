using System;
using System.Globalization;

namespace OData4.Builder
{
    /// <summary>
    /// Utility class to produce culture-oriented representation of an object as a string.
    /// </summary>
    public class ToStringInstanceHelper
    {
        private IFormatProvider _formatProviderField = CultureInfo.InvariantCulture;
        /// <summary>
        /// Gets or sets format provider to be used by ToStringWithCulture method.
        /// </summary>
        public IFormatProvider FormatProvider
        {
            get
            {
                return _formatProviderField;
            }
            set
            {
                if ((value != null))
                {
                    _formatProviderField = value;
                }
            }
        }
        /// <summary>
        /// This is called from the compile/run appdomain to convert objects within an expression block to a string
        /// </summary>
        public string ToStringWithCulture(object objectToConvert)
        {
            if ((objectToConvert == null))
            {
                throw new ArgumentNullException("objectToConvert");
            }
            var t = objectToConvert.GetType();
            var method = t.GetMethod("ToString", new[] {
                typeof(IFormatProvider)});
            if ((method == null))
            {
                return objectToConvert.ToString();
            }
            return ((string)(method.Invoke(objectToConvert, new object[] {
                _formatProviderField })));
        }
    }
}