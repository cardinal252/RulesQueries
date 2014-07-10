using Cardinal.Rules.Core;
using Cardinal.Rules.Core.Conditions;
using Lucinq.Core.Enums;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data;
using Sitecore.Rules;

namespace Cardinal.Rules.Lucinq.Conditions
{
    /// <summary>
    ///     The item descends from condition.
    /// </summary>
    /// <typeparam name="T">
    ///     The rule context type
    /// </typeparam>
    public class ItemChildOfCondition<T> : AbstractQueryCondition<T, ISitecoreQueryBuilder>, ILuceneQueryConditionVisitor
        where T : RuleContext
    {
        /// <summary>
        ///     Gets or sets the field.
        /// </summary>
        public string DataSource { get; set; }


        public Matches Matches { get; set; }

        /// <summary>
        ///     Adds the item to the query.
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        public override void VisitQuery<TContext>(ISitecoreQueryBuilder queryBuilder, TContext ruleContext)
        {
            ID id;
            if (!ID.TryParse(DataSource, out id))
            {
                return;
            }

            queryBuilder.ChildOf(id, Matches);
        }
    }
}