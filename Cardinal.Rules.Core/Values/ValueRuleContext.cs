using System.Collections.Generic;
using Cardinal.Rules.Core.Values;
using Sitecore.Rules;

namespace Cardinal.Rules.Core
{
    /// <summary>
    ///     The lucene rule context.
    /// </summary>
    public class ValueRuleContext : RuleContext, IValueRuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueRuleContext" /> class.
        /// </summary>
        public ValueRuleContext()
            : this(null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValueRuleContext" /> class.
        /// </summary>
        /// <param name="values">
        ///     The values.
        /// </param>
        /// <param name="defaultValues">
        ///     The default values.
        /// </param>
        public ValueRuleContext(Dictionary<string, string> values, Dictionary<string, string> defaultValues)
        {
            Values = values;
            DefaultValues = defaultValues;
        }

        /// <summary>
        ///     Gets or sets the default values.
        /// </summary>
        private Dictionary<string, string> DefaultValues { get; set; }

        /// <summary>
        ///     Gets or sets the values.
        /// </summary>
        private Dictionary<string, string> Values { get; set; }

        /// <summary>
        ///     The get value.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public virtual string GetValue(string key)
        {
            string valueToReturn = null;
            if (Values != null && Values.ContainsKey(key))
            {
                valueToReturn = Values[key];
            }

            if (DefaultValues != null && valueToReturn == null && DefaultValues.ContainsKey(key))
            {
                valueToReturn = DefaultValues[key];
            }

            // if key is a parameters send it back even if null.
            if (key.StartsWith("<") && key.EndsWith(">"))
            {
                if (valueToReturn == null)
                {
                    return null;
                }

                return valueToReturn;
            }

            if (valueToReturn == null)
            {
                return key;
            }

            return valueToReturn;
        }
    }
}