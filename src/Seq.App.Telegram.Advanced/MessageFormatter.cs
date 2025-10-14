using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Seq.Apps;
using Seq.Apps.LogEvents;
using Serilog;

namespace Seq.App.Telegram.Advanced
{
    public class MessageFormatter
    {
        static readonly Regex PlaceholdersRegex = new Regex("(\\[(?<key>[^\\[\\]]+?)(\\:(?<format>[^\\[\\]]+?))?\\])", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        static readonly Dictionary<string, string> EscapeList = new Dictionary<string, string>
        {
            { "_", "\\_" },
            { "*", "\\*" },
            { "[", "\\[" },
            { "`", "\\`" }
        };

        public MessageFormatter(ILogger log, string baseUrl, string messageTemplate, bool flattenProperties)
        {
            Log = log;
            MessageTemplate = messageTemplate ?? "[RenderedMessage]";
            BaseUrl = baseUrl;
            FlattenProperties = flattenProperties;
        }

        public ILogger Log { get; }
        public string MessageTemplate { get; }
        public string BaseUrl { get; }
        public bool FlattenProperties { get; }

        public string GenerateMessageText(Event<LogEventData> evt)
        {
            var message = SubstitutePlaceholders(MessageTemplate, evt);

            if (string.IsNullOrWhiteSpace(BaseUrl) == false)
            {
                var link = $"{BaseUrl}/#/events?filter=@Id%3D%3D'{evt.Id}'&show=expanded";
                message += $" [link]({link})";
            }

            return message;
        }

        string SubstitutePlaceholders(string messageTemplateToUse, Event<LogEventData> evt)
        {
            var data = evt.Data;
            var eventType = evt.EventType;
            var level = data.Level;

            Dictionary<string, object> placeholders = data.Properties?.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase)
                ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (FlattenProperties)
            {
                placeholders = DictionaryUtils.Flatten(placeholders);
            }

            AddValueIfKeyDoesntExist(placeholders, "Level", level);
            AddValueIfKeyDoesntExist(placeholders, "EventType", eventType);
            AddValueIfKeyDoesntExist(placeholders, "RenderedMessage", data.RenderedMessage);

            return PlaceholdersRegex.Replace(messageTemplateToUse, m =>
            {
                var key = m.Groups["key"].Value.ToLower();
                var format = m.Groups["format"].Value;

                KeyValuePair<string, object> prop = placeholders.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.InvariantCultureIgnoreCase));
                return prop.Value != null ? FormatValue(prop.Value, format) : m.Value;
            });
        }

        string FormatValue(object value, string format)
        {
            var rawValue = value?.ToString() ?? "(Null)";

            foreach (var escape in EscapeList)
            {
                rawValue = rawValue.Replace(escape.Key, escape.Value);
            }

            if (string.IsNullOrWhiteSpace(format))
                return rawValue;
            try
            {
                return string.Format(format, rawValue);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not format message: {value} {format}", value, format);
            }

            return rawValue;
        }

        static void AddValueIfKeyDoesntExist(IDictionary<string, object> placeholders, string key, object value)
        {
            if (!placeholders.ContainsKey(key))
                placeholders.Add(key, value);
        }
    }
}
