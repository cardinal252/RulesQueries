namespace Cardinal.Rules.Core
{
    /// <summary>
    ///     The ValueProvider interface.
    /// </summary>
    public interface IValueRuleContext
    {
        /// <summary>
        ///     The get value.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <returns>
        ///     The value
        /// </returns>
        string GetValue(string key);
    }
}