using System;
using System.Collections.Generic;
using System.Text;

namespace Seq.App.Telegram.Advanced
{
    public static class DictionaryUtils
    {
        /// <summary>
        /// Flattens a nested dictionary into a single-level dictionary using dot notation.
        /// Example: { "User": { "Name": "John" } } → { "User.Name": "John" }.
        /// </summary>
        /// <param name="source">The source dictionary to flatten.</param>
        /// <param name="parentKey">The parent key used for recursion (leave empty).</param>
        /// <returns>A flattened dictionary with dot-separated keys.</returns>
        public static Dictionary<string, object> Flatten(
            Dictionary<string, object> source,
            string parentKey = "")
        {
            var result = new Dictionary<string, object>();

            foreach (var kvp in source)
            {
                var fullKey = string.IsNullOrEmpty(parentKey)
                    ? kvp.Key
                    : $"{parentKey}.{kvp.Key}";

                if (kvp.Value is Dictionary<string, object> nested)
                {
                    foreach (var child in Flatten(nested, fullKey))
                        result[child.Key] = child.Value;
                }
                else
                {
                    result[fullKey] = kvp.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// A streaming version that avoids building intermediate dictionaries.
        /// Useful for very large or deep nested structures.
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> FlattenLazy(
            Dictionary<string, object> source,
            string parentKey = "")
        {
            foreach (var kvp in source)
            {
                var fullKey = string.IsNullOrEmpty(parentKey)
                    ? kvp.Key
                    : $"{parentKey}.{kvp.Key}";

                if (kvp.Value is Dictionary<string, object> nested)
                {
                    foreach (var child in FlattenLazy(nested, fullKey))
                        yield return child;
                }
                else
                {
                    yield return new KeyValuePair<string, object>(fullKey, kvp.Value);
                }
            }
        }
    }
}
