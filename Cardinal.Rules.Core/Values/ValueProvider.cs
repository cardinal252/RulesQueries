namespace Cardinal.Rules.Core.Values
{
    /// <summary>
    /// The lucene string value.
    /// </summary>
    public class ValueProvider : IValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueProvider" /> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public ValueProvider(string value)
        {
            Value = value;
        }


        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }
    }
}