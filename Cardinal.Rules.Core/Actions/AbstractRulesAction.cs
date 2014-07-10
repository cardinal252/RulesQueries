using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cardinal.Rules.Core.Builders;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Cardinal.Rules.Core.Actions
{
    /// <summary>
    ///     The lucene query action.
    /// </summary>
    /// <typeparam name="T">
    /// The rule context type
    /// </typeparam>
    /// <typeparam name="TBuilder">
    /// The query builder type
    /// </typeparam>
    public abstract class AbstractRulesAction<T, TBuilder> : RuleAction<T>, IValueQueryVisitor<TBuilder> where T : RuleContext
    {
        /// <summary>
        ///     Gets or sets the values.
        /// </summary>
        public Dictionary<string, string> Values { get; set; }

        /// <summary>
        ///     Applies the rule action
        /// </summary>
        /// <param name="ruleContext">
        ///     The rule context.
        /// </param>
        [ExcludeFromCodeCoverage]
        public override void Apply(T ruleContext)
        {
            throw new NotImplementedException(
                "This query action type is not designed for the rules engine, use the query adapter");
        }

        /// <summary>
        ///     The visit query.
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        public abstract void VisitQuery<TContext>(TBuilder queryBuilder, TContext ruleContext) where TContext : IValueRuleContext;
    }
}