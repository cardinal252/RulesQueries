namespace Cardinal.Rules.Core.Builders
{
    /// <summary>
    ///     The Lucene Query visitor interface.
    /// </summary>
    public interface IValueQueryVisitor<in TBuilder>
    {
        /// <summary>
        ///     The add to query.
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        void VisitQuery<TContext>(TBuilder queryBuilder, TContext ruleContext) where TContext : IValueRuleContext;
    }
}