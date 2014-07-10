using System.Collections.Generic;

namespace Cardinal.Rules.Core.Results
{
    public class RulesSearchResult<T> : IRulesSearchResult<T>
    {
        public RulesSearchResult(List<T> results, int hits)
        {
            TotalHits = hits;
            Results = results;
        } 

        public int TotalHits { get; private set; }

        public IList<T> Results { get; private set; }
    }
}
