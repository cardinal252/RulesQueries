namespace Cardinal.Rules.Core.Values
{
    /// <summary>
    ///     The LuceneValue interface.
    /// </summary>
    public interface IValueProvider
    {
        /// <summary>
        ///     The lucene query value.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        string Value { get; set; }
    }
}