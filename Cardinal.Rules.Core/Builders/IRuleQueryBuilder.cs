namespace Cardinal.Rules.Core.Builders
{
    /// <summary>
    ///     The Rule Query Adapter interface.
    /// </summary>
    public interface IRuleQueryBuilder
    {
        /// <summary>
        ///     Gets the query builder.
        /// </summary>
        /// <returns>
        ///     The <see cref="IQueryBuilder" />.
        /// </returns>
        void Process();
    }
}