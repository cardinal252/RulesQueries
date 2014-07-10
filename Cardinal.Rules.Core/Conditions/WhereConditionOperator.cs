namespace Cardinal.Rules.Core.Conditions
{
    /// <summary>
    ///     The where condition operator.
    /// </summary>
    public enum WhereConditionOperator
    {
        /// <summary>
        ///     The unknown.
        /// </summary>
        Unknown,

        /// <summary>
        ///     The equals.
        /// </summary>
        Equals,

        /// <summary>
        ///     Contained within
        /// </summary>
        Contains,

        /// <summary>
        ///     Starts with.
        /// </summary>
        StartsWith,

        /// <summary>
        ///     The ends with.
        /// </summary>
        EndsWith
    }
}