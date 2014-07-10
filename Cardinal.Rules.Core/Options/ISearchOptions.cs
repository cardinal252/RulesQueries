namespace Cardinal.Rules.Core.Options
{
    /// <summary>
    /// The Search Options interface.
    /// </summary>
    public interface ISearchOptions
    {
        /// <summary>
        /// Gets the start index.
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        int Length { get; }
    }
}