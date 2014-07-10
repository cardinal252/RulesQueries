using System.Diagnostics.CodeAnalysis;
using Cardinal.Core.Data;
using Cardinal.Core.IoC;
using Cardinal.Rules.Core;
using Cardinal.Rules.Core.Conditions;
using Lucinq.Core.Enums;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data;
using Sitecore.Rules;

namespace Cardinal.Rules.Lucinq.Conditions
{
    /// <summary>
    ///     The item descends from condition.
    /// </summary>
    /// <typeparam name="T">
    ///     The rule context type
    /// </typeparam>
    public class WhereItemCondition<T> : AbstractWhereCondition<T, ISitecoreQueryBuilder>, ILuceneQueryConditionVisitor
        where T : RuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WhereItemCondition{T}" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public WhereItemCondition()
            : this(ContainerManagerFactory.GetContainerManager().Resolve<IDatabaseHelper>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WhereItemCondition{T}" /> class.
        /// </summary>
        /// <param name="databaseHelper">
        ///     The content manager.
        /// </param>
        public WhereItemCondition(IDatabaseHelper databaseHelper)
            : base(databaseHelper)
        {
        }

        public Matches Matches { get; set; }

        /// <summary>
        ///     Adds the item to the query.
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        public override void VisitQuery<TContext>(ISitecoreQueryBuilder queryBuilder, TContext ruleContext)
        {
            ID id;
            string value = Value;
            value = ruleContext.GetValue(value);
            if (!ID.TryParse(value, out id))
            {
                return;
            }

            queryBuilder.Field(GetFieldName(), id, this.Matches);
        }
    }
}