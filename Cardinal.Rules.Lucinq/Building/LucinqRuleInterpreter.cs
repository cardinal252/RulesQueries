using Cardinal.Rules.Core;
using Cardinal.Rules.Core.Builders;
using Cardinal.Rules.Core.Enums;
using Cardinal.Rules.Lucinq.Conditions;
using Lucinq.Enums;
using Lucinq.SitecoreIntegration.Querying;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Rules;
using Sitecore.Rules.Actions;
using Sitecore.Rules.Conditions;

namespace Cardinal.Rules.Lucinq.Building
{
    /// <summary>
    ///     The rule query adapter.
    /// </summary>
    /// <typeparam name="T">
    ///     The context type
    /// </typeparam>
    public class LucinqRuleInterpreter<T> : AbstractRuleInterpreter<T, ISitecoreQueryBuilder> where T : RuleContext, IValueRuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RuleQueryBuilder{T}" /> class.
        /// </summary>
        /// <param name="ruleList">
        ///     The rule list.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        public LucinqRuleInterpreter(RuleList<T> ruleList, T ruleContext)
            : base(ruleList, ruleContext, new SitecoreQueryBuilder())
        {
        }

        /// <summary>
        ///     The add and condition.
        /// </summary>
        /// <param name="condition">
        ///     The condition.
        /// </param>
        /// <param name="conditionType">
        ///     The condition type.
        /// </param>
        protected override void AddAndCondition(RuleCondition<T> condition, QueryConditionType conditionType)
        {
            var binaryCondition = condition as AndCondition<T>;
            if (binaryCondition == null)
            {
                return;
            }

            ISitecoreQueryBuilder group = Builder.Group(GetLucinqMatches(conditionType));
            AddToBuilder(binaryCondition.LeftOperand);
            AddToBuilder(binaryCondition.RightOperand);

            if (group.Queries.Count == 0 && group.Groups.Count == 0)
            {
                Builder.Groups.Remove(group);
            }
        }

        /// <summary>
        ///     Adds a lucene action to the builder
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder
        /// </param>
        /// <param name="action">
        ///     The action
        /// </param>
        protected void AddLuceneAction(ISitecoreQueryBuilder queryBuilder, RuleAction<T> action)
        {
            var luceneQueryAction = action as IValueQueryVisitor<ISitecoreQueryBuilder>;
            if (luceneQueryAction == null)
            {
                return;
            }

            luceneQueryAction.VisitQuery(queryBuilder, RuleContext);
        }

        protected override void AddValueCondition(RuleCondition<T> condition, QueryConditionType queryConditionType)
        {
            var luceneQueryCondition = condition as ILuceneQueryConditionVisitor;
            if (luceneQueryCondition == null)
            {
                return;
            }

            luceneQueryCondition.Matches = GetLucinqMatches(queryConditionType);
            luceneQueryCondition.VisitQuery(Builder, RuleContext);
        }

        protected override void AddOrCondition(RuleCondition<T> condition, QueryConditionType queryConditionType)
        {
            var binaryCondition = condition as OrCondition<T>;
            if (binaryCondition == null)
            {
                return;
            }

            ISitecoreQueryBuilder group = Builder.Group(GetLucinqMatches(queryConditionType));
            AddToBuilder(binaryCondition.LeftOperand, QueryConditionType.Optional);
            AddToBuilder(binaryCondition.RightOperand, QueryConditionType.Optional);

            if (group.Queries.Count == 0 && group.Groups.Count == 0)
            {
                Builder.Groups.Remove(group);
            }
        }

        private Matches GetLucinqMatches(QueryConditionType conditionType)
        {
            switch (conditionType)
            {
                case QueryConditionType.MustNot:
                    return Matches.Never;
                case QueryConditionType.Optional:
                    return Matches.Sometimes;

                default:
                    return Matches.NotSet;
            }
        }
    }
}