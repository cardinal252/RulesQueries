using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cardinal.Rules.Core.Builders;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Cardinal.Rules.Core.Conditions
{
    public abstract class AbstractQueryCondition<T, TBuilder> : RuleCondition<T>, IValueQueryVisitor<TBuilder>
        where T : RuleContext
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the values.
        /// </summary>
        public Dictionary<string, string> Values { get; set; }

        #endregion

        /// <summary>
        ///     Method to add to query
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        public abstract void VisitQuery<TContext>(TBuilder queryBuilder, TContext ruleContext) where TContext : IValueRuleContext;

        /// <summary>
        ///     Evaluates the rule - unused
        /// </summary>
        /// <param name="ruleContext">
        ///     The rule context.
        /// </param>
        /// <param name="stack">
        ///     The stack.
        /// </param>
        [ExcludeFromCodeCoverage]
        public override void Evaluate(T ruleContext, RuleStack stack)
        {
        }
    }
}