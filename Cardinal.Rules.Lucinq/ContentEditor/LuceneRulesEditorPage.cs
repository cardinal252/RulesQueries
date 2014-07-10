using System.Diagnostics.CodeAnalysis;
using Cardinal.Core.Data;
using Cardinal.Core.Items;
using Cardinal.Core.Logging;
using Cardinal.Rules.Core;
using Cardinal.Rules.Core.ContentEditor;
using Cardinal.Rules.Core.Options;
using Cardinal.Rules.Core.QueryManagement;
using Cardinal.Rules.Core.Results;
using Cardinal.Rules.Lucinq.Data;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace Cardinal.Rules.Lucinq.ContentEditor
{
    /// <summary>
    /// The lucene specific rules editor page.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LuceneRulesEditorPage : AbstractRulesEditorPage
    {
        private string lastQuery;

        private readonly IQueryManager queryManager = new QueryManager(new LucinqSearchRepository(new ContentDatabaseHelper()), new ItemManager(new ItemRepository(), new SitecoreLog()), new SitecoreHelper());

        protected override IQueryManager QueryManager
        {
            get { return queryManager; }
        }

        protected override IRulesSearchResult<Item> GetResults(RuleList<ValueRuleContext> rulesList, ValueRuleContext context)
        {
            int numberToReturn;
            if (NumberOfResults == null || !int.TryParse(NumberOfResults.Text, out numberToReturn))
            {
                numberToReturn = 20;
            }

            IRulesSearchResult<Item> result = QueryManager.GetRulesSearchResult<Item>(rulesList, new SearchOptions(0, numberToReturn));

            return result;
        }

        protected override void PerformPostQueryActions()
        {
            Results.InnerHtml += string.Format("<br/><p><strong>Query:</strong> {0}</p>", lastQuery);
        }
    }
}