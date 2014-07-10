using System.Collections.Generic;

namespace Cardinal.Rules.Core.Results
{
    /// <summary>
    /// The Search Result interface.
    /// </summary>
    /// <typeparam name="T">
    /// The type of search objects returned
    /// </typeparam>
    public interface IRulesSearchResult<T>
    {
        /// <summary>
        /// The total matches found
        /// </summary>
        int TotalHits { get; }

        /// <summary>
        /// Gets the results.
        /// </summary>
        IList<T> Results { get; }
    }
}
