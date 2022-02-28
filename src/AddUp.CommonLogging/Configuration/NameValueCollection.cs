using System;
using System.Collections.Generic;

namespace AddUp.CommonLogging.Configuration
{
    /// <summary>
    /// Substitute NameValueCollection in System.Collections.Specialized.
    /// </summary>
    public sealed class NameValueCollection : Dictionary<string, string>
    {
        public NameValueCollection() : base(StringComparer.OrdinalIgnoreCase) { }

        /// <summary>
        /// Gets the values (only a single one) for the specified key (configuration name)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>an array with one value, or null if no value exist</returns>
        public string[] GetValues(string key)
        {
            if (TryGetValue(key, out string value) && value != null)
                return new[] { value };
            return new string[0];
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <value>
        /// The value corrsponding to the key, or null if no value exist
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>value store for the key</returns>
        public new string this[string key]
        {
            get
            {
                if (TryGetValue(key, out string value))
                    return value;
                return null;
            }
            set => base[key] = value;
        }
    }
}