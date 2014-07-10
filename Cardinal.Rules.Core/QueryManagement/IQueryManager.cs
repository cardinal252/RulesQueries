using System.Collections.Generic;
using Cardinal.Rules.Core.Options;
using Cardinal.Rules.Core.Results;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace Cardinal.Rules.Core.QueryManagement
{
    /// <summary>
    ///     The Query Manager interface.
    /// </summary>
    public interface IQueryManager
    {
        /// <summary>
        /// Gets the default values.
        /// </summary>
        /// <param name="queryItem">
        /// The query item.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary{TKey,TValue}" />.
        /// </returns>
        Dictionary<string, string> GetDefaultValues(Item queryItem);

        /// <summary>
        /// Gets the results of the query.
        /// </summary>
        /// <typeparam name="T">
        /// The type to return the results as
        /// </typeparam>
        /// <param name="queryItem">
        /// The query Item.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="defaultValues">
        /// The default Values.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}" />.
        /// </returns>
        IRulesSearchResult<Item> GetResults(
            Item queryItem,
            ISearchOptions options,
            Dictionary<string, string> values = null,
            Dictionary<string, string> defaultValues = null);

        /// <summary>
        /// Gets the results directly from the rule list
        /// </summary>
        /// <typeparam name="T">The type of result</typeparam>
        /// <param name="rulesList">The rule list</param>
        /// <param name="options">The options</param>
        /// <param name="values">The values</param>
        /// <param name="defaultValues">The default values</param>
        /// <returns></returns>
        IRulesSearchResult<Item> GetRulesSearchResult<T>(
            RuleList<ValueRuleContext> rulesList,
            ISearchOptions options,
            Dictionary<string, string> values = null,
            Dictionary<string, string> defaultValues = null) where T : class;
    }
}