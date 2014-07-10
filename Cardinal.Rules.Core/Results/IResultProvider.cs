using Cardinal.Rules.Core.Options;

namespace Cardinal.Rules.Core.Results
{
    public interface IResultProvider<T>
    {
        IRulesSearchResult<T> GetResults(ISearchOptions options, ValueRuleContext context);
    }
}
