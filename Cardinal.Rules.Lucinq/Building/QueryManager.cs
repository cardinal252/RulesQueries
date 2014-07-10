using System;
using System.Collections.Generic;
using System.Linq;
using Cardinal.Core.Data;
using Cardinal.Core.Items;
using Cardinal.Rules.Core;
using Cardinal.Rules.Core.Options;
using Cardinal.Rules.Core.QueryManagement;
using Cardinal.Rules.Core.Results;
using Cardinal.Rules.Lucinq.Building;
using Lucinq.Core.Interfaces;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace Cardinal.Rules.Lucinq
{
    /// <summary>
    ///     The query runner.
    /// </summary>
    public class QueryManager : AbstractQueryManager
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryManager" /> class.
        /// </summary>
        /// <param name="lucinqSearchRepository">
        ///     The search repository.
        /// </param>
        /// <param name="itemManager">
        ///     The item Manager.
        /// </param>
        /// <param name="sitecoreHelper">
        ///     The sitecore Helper.
        /// </param>
        public QueryManager(
            ILucinqSearchRepository lucinqSearchRepository,
            IItemManager itemManager,
            ISitecoreHelper sitecoreHelper) : base(itemManager, sitecoreHelper)
        {
            LucinqSearchRepository = lucinqSearchRepository;
        }

        /// <summary>
        ///     Gets the search repository.
        /// </summary>
        protected ILucinqSearchRepository LucinqSearchRepository { get; private set; }

        /// <summary>
        ///     Gets the results of the query.
        /// </summary>
        /// <typeparam name="T">
        ///     The type to return the results as
        /// </typeparam>
        /// <param name="queryItem">
        ///     The query Item.
        /// </param>
        /// <param name="options">
        ///     The options.
        /// </param>
        /// <param name="values">
        ///     The values.
        /// </param>
        /// <param name="defaultValues">
        ///     The default Values.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable{T}" />.
        /// </returns>
        public override IRulesSearchResult<Item> GetResults(
            Item queryItem,
            ISearchOptions options,
            Dictionary<string, string> values = null,
            Dictionary<string, string> defaultValues = null)
        {
            return IsRulesQuery(queryItem)
                ? GetRulesSearchResult<Item>(queryItem, options ?? new SearchOptions(0, 10), values, defaultValues)
                : GetMultilistSearchResult(queryItem, options ?? new SearchOptions(0, 10));
        }

        /// <summary>
        /// Gets whether its a rules query to execute.
        /// </summary>
        /// <param name="queryItem">The query item</param>
        /// <returns>The rules query</returns>
        protected virtual bool IsRulesQuery(Item queryItem)
        {
            return ItemManager.IsDerivedFromTemplate(
                queryItem, Constants.Templates.BasicQuery) ||
                   ItemManager.IsDerivedFromTemplate(
                       queryItem,
                       Constants.Templates.SystemQuery);
        }

        /// <summary>
        /// Gets the multilist search result
        /// </summary>
        /// <typeparam name="T">
        /// The type to return
        /// </typeparam>
        /// <param name="queryItem">
        /// The query item
        /// </param>
        /// <param name="options">
        /// The query options
        /// </param>
        /// <returns>
        /// The search result
        /// </returns>
        private IRulesSearchResult<Item> GetMultilistSearchResult(Item queryItem, ISearchOptions options)
        {
            if (String.IsNullOrEmpty(queryItem["Items"]))
            {
                return null;
            }

            MultilistField multilistField = new MultilistField(queryItem.Fields["Items"]);

            Item[] items = multilistField.GetItems();

            if (options != null)
            {
                items = items.Skip(options.StartIndex).Take(options.Length).ToArray();
            }


            if (!items.Any())
            {
                return null;
            }

            IRulesSearchResult<Item> searchResult = new RulesSearchResult<Item>(items.ToList(), items.Length);
            return searchResult;
        }

        /// <summary>
        /// Gets the rules search result
        /// </summary>
        /// <typeparam name="T">
        /// The type to return
        /// </typeparam>
        /// <param name="queryItem">
        /// The query item
        /// </param>
        /// <param name="options">
        /// The options
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="defaultValues">
        /// The default Values.
        /// </param>
        /// <returns>
        /// The search result
        /// </returns>
        protected virtual IRulesSearchResult<Item> GetRulesSearchResult<T>(
            Item queryItem,
            ISearchOptions options,
            Dictionary<string, string> values = null,
            Dictionary<string, string> defaultValues = null) where T : class
        {
            if (defaultValues == null)
            {
                defaultValues = GetDefaultValues(queryItem);
            }

            RuleList<ValueRuleContext> rulesList = SitecoreHelper.GetRules<ValueRuleContext>(queryItem, "Rule");

            return rulesList == null
                ? null
                : GetRulesSearchResult<Item>(rulesList, options, values, defaultValues);
        }

        public override IRulesSearchResult<Item> GetRulesSearchResult<T>(
            RuleList<ValueRuleContext> rulesList,
            ISearchOptions options,
            Dictionary<string, string> values = null,
            Dictionary<string, string> defaultValues = null)
        {
            ValueRuleContext context = new ValueRuleContext(values, defaultValues);

            if (rulesList == null)
            {
                return null;
            }

            LucinqRuleInterpreter<ValueRuleContext> adapter = new LucinqRuleInterpreter<ValueRuleContext>(rulesList, context);
            adapter.Process();
            ISitecoreQueryBuilder queryBuilder = adapter.Builder;

            IItemResult<Item> results = LucinqSearchRepository.GetResults(queryBuilder, options);

            IRulesSearchResult<Item> searchResult =
                new RulesSearchResult<Item>(
                    results.Items,
                    results.TotalHits);
            return searchResult;
        }
    }
}