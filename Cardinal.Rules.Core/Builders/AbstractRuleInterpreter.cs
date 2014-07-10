using System.Collections.Generic;
using Cardinal.Rules.Core.Enums;
using Sitecore.Rules;
using Sitecore.Rules.Actions;
using Sitecore.Rules.Conditions;

namespace Cardinal.Rules.Core.Builders
{
    /// <summary>
    ///     The rule query adapter.
    /// </summary>
    /// <typeparam name="T">
    ///     The context type
    /// </typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    public abstract class AbstractRuleInterpreter<T, TBuilder> : IRuleQueryBuilder
        where T : RuleContext, IValueRuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractRuleInterpreterInterpreter{T,TBuilder}" /> class.
        /// </summary>
        /// <param name="ruleList">
        ///     The rule list.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        /// <param name="builder"></param>
        protected AbstractRuleInterpreter(RuleList<T> ruleList, T ruleContext, TBuilder builder)
        {
            Builder = builder;
            RuleList = ruleList;
            RuleContext = ruleContext;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RuleQueryBuilder{T}" /> class.
        /// </summary>
        /// <param name="ruleList">
        ///     The rule list.
        /// </param>
        /// <param name="builder"></param>
        protected AbstractRuleInterpreter(RuleList<T> ruleList, TBuilder builder)
            : this(ruleList, null, builder)
        {
        }

        public TBuilder Builder { get; private set; }

        /// <summary>
        ///     Gets or sets the rule context.
        /// </summary>
        protected T RuleContext { get; set; }

        /// <summary>
        ///     Gets the rule list.
        /// </summary>
        protected RuleList<T> RuleList { get; private set; }

        /// <summary>
        ///     Gets the query builder.
        /// </summary>
        public virtual void Process()
        {
            foreach (var rule in RuleList.Rules)
            {
                AddToBuilder(rule.Condition);
                AddToBuilder(rule.Actions);
            }
        }

        /// <summary>
        ///     The add and condition.
        /// </summary>
        /// <param name="condition">
        ///     The condition.
        /// </param>
        /// <param name="conditionType">
        /// The query condition type
        /// </param>
        protected abstract void AddAndCondition(RuleCondition<T> condition, QueryConditionType conditionType);

        /// <summary>
        ///     Adds a lucene action to the builder
        /// </summary>
        /// <param name="action">
        ///     The action
        /// </param>
        protected void AddValueAction(RuleAction<T> action)
        {
            var luceneQueryAction = action as IValueQueryVisitor<TBuilder>;
            if (luceneQueryAction == null)
            {
                return;
            }

            luceneQueryAction.VisitQuery(Builder, RuleContext);
        }

        /// <summary>
        ///     Adds the query condition.
        /// </summary>
        /// <param name="condition">
        ///     The condition.
        /// </param>
        /// <param name="queryConditionType"></param>
        protected abstract void AddValueCondition(RuleCondition<T> condition, QueryConditionType queryConditionType);

        /// <summary>
        ///     The add not condition.
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="condition">
        ///     The condition.
        /// </param>
        private void AddNotCondition(RuleCondition<T> condition)
        {
            var notCondition = condition as NotCondition<T>;
            if (notCondition == null)
            {
                return;
            }

            AddToBuilder(notCondition.Operand, QueryConditionType.MustNot);
        }

        /// <summary>
        ///     The add or condition.
        /// </summary>
        /// <param name="condition">
        ///     The condition.
        /// </param>
        /// <param name="queryConditionType"></param>
        protected abstract void AddOrCondition(RuleCondition<T> condition, QueryConditionType queryConditionType);

        /// <summary>
        ///     Adds the query to the builder.
        /// </summary>
        /// <param name="condition">
        ///     The condition.
        /// </param>
        /// <param name="queryConditionType"></param>
        protected virtual void AddToBuilder(
            RuleCondition<T> condition,
            QueryConditionType queryConditionType = QueryConditionType.Must)
        {
            AddAndCondition(condition, queryConditionType);
            AddOrCondition(condition, queryConditionType);
            AddValueCondition(condition, queryConditionType);
            AddNotCondition(condition);
        }

        /// <summary>
        ///     Adds the actions to the builder.
        /// </summary>
        /// <param name="actions">
        ///     The actions.
        /// </param>
        private void AddToBuilder(List<RuleAction<T>> actions)
        {
            if (actions == null || actions.Count == 0)
            {
                return;
            }

            actions.ForEach(AddValueAction);
        }
    }
}