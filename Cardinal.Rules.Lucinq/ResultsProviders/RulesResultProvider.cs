using Cardinal.Rules.Core;
using Cardinal.Rules.Core.Options;
using Cardinal.Rules.Core.Results;
using Cardinal.Rules.Lucinq.Building;
using Lucinq.Core.Interfaces;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace Cardinal.Rules.Lucinq.ResultsProviders
{
    public class RulesResultProvider : IResultProvider<Item>
    {
        private readonly RuleList<ValueRuleContext> rulesList;

        private readonly ILucinqSearchRepository lucinqSearchRepository;

        public RulesResultProvider(RuleList<ValueRuleContext> rulesList, ILucinqSearchRepository lucinqSearchRepository)
        {
            this.rulesList = rulesList;
            this.lucinqSearchRepository = lucinqSearchRepository;
        }

        public IRulesSearchResult<Item> GetResults(ISearchOptions options, ValueRuleContext context)
        {
            if (rulesList == null)
            {
                return null;
            }

            LucinqRuleInterpreter<ValueRuleContext> adapter = new LucinqRuleInterpreter<ValueRuleContext>(rulesList, context);
            ISitecoreQueryBuilder queryBuilder = adapter.Builder;

            IItemResult<Item> results = lucinqSearchRepository.GetResults(queryBuilder, options);

            IRulesSearchResult<Item> searchResult =
                new RulesSearchResult<Item>(
                    results.Items,
                    results.TotalHits);
            return searchResult;
        }
    }
}
