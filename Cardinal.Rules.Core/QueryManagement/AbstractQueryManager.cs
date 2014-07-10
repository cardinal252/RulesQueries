using System.Collections.Generic;
using System.Linq;
using Cardinal.Core.Data;
using Cardinal.Core.Items;
using Cardinal.Rules.Core.Options;
using Cardinal.Rules.Core.Results;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace Cardinal.Rules.Core.QueryManagement
{
    public abstract class AbstractQueryManager : IQueryManager
    {
        /// <summary>
        /// Gets the item manager
        /// </summary>
        protected IItemManager ItemManager { get; private set; }

        /// <summary>
        /// Gets the sitecore helper
        /// </summary>
        protected ISitecoreHelper SitecoreHelper { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractQueryManager" /> class.
        /// </summary>
        /// <param name="itemManager">
        /// The item Manager.
        /// </param>
        /// <param name="sitecoreHelper">
        /// The sitecore Helper.
        /// </param>
        protected AbstractQueryManager(
            IItemManager itemManager,
            ISitecoreHelper sitecoreHelper)
        {
            ItemManager = itemManager;
            SitecoreHelper = sitecoreHelper;
        }

        /// <summary>
        ///     The get default values.
        /// </summary>
        /// <param name="queryItem">
        ///     The query item.
        /// </param>
        /// <returns>
        ///     The <see cref="Dictionary{TKey,TValue}" />.
        /// </returns>
        public virtual Dictionary<string, string> GetDefaultValues(Item queryItem)
        {
            IEnumerable<Item> children = queryItem.GetLanguageChildren();
            Item[] defaultValues = children.ToArray();
            if (defaultValues.Length == 0)
            {
                return null;
            }

            var valuesDictionary = new Dictionary<string, string>();

            foreach (Item child in defaultValues)
            {
                if (valuesDictionary.ContainsKey(child.Name) == false)
                {
                    valuesDictionary.Add(string.Format("#{0}#", child.Name), child["value"]);
                }
            }

            return valuesDictionary;
        }

        public abstract IRulesSearchResult<Item> GetResults(Item queryItem, ISearchOptions options, Dictionary<string, string> values = null,
            Dictionary<string, string> defaultValues = null);

        public abstract IRulesSearchResult<Item> GetRulesSearchResult<T>(RuleList<ValueRuleContext> rulesList, ISearchOptions options, Dictionary<string, string> values = null,
            Dictionary<string, string> defaultValues = null) where T : class;
    }
}
