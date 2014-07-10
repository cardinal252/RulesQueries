using Cardinal.Rules.Core.Builders;
using Lucinq.Enums;
using Lucinq.SitecoreIntegration.Querying.Interfaces;

namespace Cardinal.Rules.Lucinq.Conditions
{
    /// <summary>
    /// The LuceneQueryConditionVisitor interface.
    /// </summary>
    public interface ILuceneQueryConditionVisitor : IValueQueryVisitor<ISitecoreQueryBuilder>
    {
        /// <summary>
        ///     Gets or sets the matches.
        /// </summary>
        Matches Matches { get; set; }
    }
}