using System.Diagnostics.CodeAnalysis;
using Cardinal.Core.Data;
using Cardinal.Rules.Core.Options;
using Lucinq.Core.Interfaces;
using Lucinq.SitecoreIntegration.Querying;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Configuration;
using Sitecore.Data.Items;

namespace Cardinal.Rules.Lucinq.Data
{
    /// <summary>
    /// The lucene search repository.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LucinqSearchRepository : ILucinqSearchRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LucinqSearchRepository"/> class.
        /// </summary>
        /// <param name="databaseHelper">
        /// The database manager.
        /// </param>
        public LucinqSearchRepository(IDatabaseHelper databaseHelper)
        {
            DatabaseHelper = databaseHelper;
        }

        /// <summary>
        /// Gets the database manager.
        /// </summary>
        protected IDatabaseHelper DatabaseHelper { get; private set; }

        /// <summary>
        /// The get results.
        /// </summary>
        /// <param name="queryBuilder">
        /// The query builder.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <returns>
        /// The <see cref="ISearchResult"/>.
        /// </returns>
        public IItemResult<Item> GetResults(ISitecoreQueryBuilder queryBuilder, ISearchOptions options)
        {
            var search = new SitecoreSearch(GetIndexName());
            var result = search.Execute(queryBuilder);

            int end = options.StartIndex + options.Length - 1;
            return result.GetRange(options.StartIndex, end);
        }

        /// <summary>
        ///     Gets the current index name
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected virtual string GetIndexName()
        {
            return Settings.IndexFolder + "\\" + string.Format(Settings.GetSetting("CR_IndexNameFormat", "sitecore_{0}_index"), Sitecore.Context.ContentDatabase.Name.ToLowerInvariant());
        }
    }
}
