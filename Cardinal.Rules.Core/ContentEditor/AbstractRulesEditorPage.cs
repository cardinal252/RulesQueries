using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using Cardinal.Rules.Core.QueryManagement;
using Cardinal.Rules.Core.Results;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Shell.Applications.Dialogs.RulesEditor;
using Sitecore.Shell.Applications.Rules.RulesEditor;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;

namespace Cardinal.Rules.Core.ContentEditor
{
    /// <summary>
    ///     The lucene rules editor page.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class AbstractRulesEditorPage : RulesEditorPage
    {
        /// <summary>
        ///     Gets or sets the results scrollbox
        /// </summary>
        protected Scrollbox Results { get; set; }

        /// <summary>
        ///     Gets or sets the number of results textbox
        /// </summary>
        protected TextBox NumberOfResults { get; set; }

        /// <summary>
        ///     Gets or sets the rules.
        /// </summary>
        protected string Rules
        {
            get { return ViewState["Rules"] as string ?? string.Empty; }

            set
            {
                Assert.ArgumentNotNull(value, "value");
                ViewState["Rules"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets the test button
        /// </summary>
        protected HyperLink TestButton { get; set; }


        /// <summary>
        ///     The on initialization event.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetupTestAction();
        }

        /// <summary>
        ///     The on load.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            Results.InnerHtml = "Please click 'Test' to test your query.";
            base.OnLoad(e);
        }

        /// <summary>
        ///     The setup test action.
        /// </summary>
        protected virtual void SetupTestAction()
        {
            string str = StringUtil.EscapeQuote("TestQuery()");
            TestButton.Attributes["href"] = "#";
            TestButton.Attributes["onclick"] = "javascript:return scForm.postRequest('','','','" + str + "')";
        }

        /// <summary>
        /// The test query.
        /// </summary>
        protected virtual void TestQuery()
        {
            RulesEditorOptions options = RulesEditorOptions.Parse();

            // RenderRules(rulesDefinition.Document);

            if (string.IsNullOrEmpty(Rules))
            {
                Results.InnerHtml = "There is currently no rule available to test.";
                return;
            }

            ValueRuleContext context = new ValueRuleContext(null, new Dictionary<string, string>()); // QueryManager.GetDefaultValues(contextItem);
            RuleList<ValueRuleContext> rulesList = RuleFactory.ParseRules<ValueRuleContext>(Sitecore.Context.ContentDatabase, Rules);

            rulesList.Run(context);

            IRulesSearchResult<Item> result = GetResults(rulesList, context);

            if (result.Results != null && result.Results.Count > 0)
            {
                Results.InnerHtml = "<p>No results were found for this query</p>";
            }

            Results.InnerHtml = string.Format("<p><strong>Total Hits:</strong> {0}</p>", result.TotalHits);

            if (result.Results != null)
            {
                Results.InnerHtml += "<ul>";
                foreach (Item item in result.Results)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    Results.InnerHtml +=
                        string.Format(
                            "<li><span class=\"title\">{0}</span><br/>Item Path: {1}<br/> Item Name:{2}<br/>Template:{3}<br/>Item Id:{4}</li>",
                            item["Short Title"],
                            item.Paths.FullPath,
                            item.Name,
                            item.TemplateName,
                            item.ID);
                }

                Results.InnerHtml += "</ul>";
            }

            PerformPostQueryActions();

            SheerResponse.SetReturnValue(true);
        }

        protected abstract IQueryManager QueryManager { get; }

        protected abstract IRulesSearchResult<Item> GetResults(RuleList<ValueRuleContext> rulesList, ValueRuleContext context);

        protected abstract void PerformPostQueryActions();
    }
}