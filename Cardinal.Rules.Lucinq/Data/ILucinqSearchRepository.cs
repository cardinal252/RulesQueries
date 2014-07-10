using Cardinal.Rules.Core.Options;
using Lucinq.Interfaces;
using Lucinq.SitecoreIntegration.Querying;
using Lucinq.SitecoreIntegration.Querying.Interfaces;

namespace Cardinal.Rules.Lucinq
{

    using Sitecore.Data.Items;

    /// <summary>
    /// The Lucene Search Repository interface.
    /// </summary>
    public interface ILucinqSearchRepository
    {
        /// <summary>
        /// Gets the results from a query builder execution
        /// </summary>
        /// <param name="queryBuilder">
        /// The query builder.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="SitecoreSearchResult"/>.
        /// </returns>
        IItemResult<Item> GetResults(ISitecoreQueryBuilder queryBuilder, ISearchOptions options);
    }
}
