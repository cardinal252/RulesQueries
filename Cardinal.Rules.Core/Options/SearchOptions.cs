namespace Cardinal.Rules.Core.Options
{
    /// <summary>
    ///     The search options.
    /// </summary>
    public class SearchOptions : ISearchOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchOptions"/> class.
        /// </summary>
        /// <param name="startIndex">
        /// The start index.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <param name="sortDefinition">
        /// The sort definition.
        /// </param>
        public SearchOptions(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchOptions"/> class.
        /// </summary>
        /// <param name="sortDefinition">
        /// The sort definition.
        /// </param>
        public SearchOptions()
            : this(0, int.MaxValue - 1)
        {
        }

        /// <summary>
        ///     Gets the length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        ///     Gets the start index.
        /// </summary>
        public int StartIndex { get; private set; }
    }
}
