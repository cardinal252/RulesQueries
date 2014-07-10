using System.Diagnostics.CodeAnalysis;
using Cardinal.Core.Data;
using Cardinal.Core.IoC;
using Cardinal.Rules.Lucinq.Conditions;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data;
using Sitecore.Rules;

namespace Cardinal.Rules.Core.Conditions
{
    /// <summary>
    ///     The complex where item condition.
    /// </summary>
    /// <typeparam name="T">
    ///     The rule context type
    /// </typeparam>
    public class ComplexWhereItemCondition<T> : WhereItemCondition<T> where T : RuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ComplexWhereItemCondition{T}" /> class.
        /// </summary>
        /// <param name="databaseHelper">
        ///     The content manager.
        /// </param>
        public ComplexWhereItemCondition(IDatabaseHelper databaseHelper)
            : base(databaseHelper)
        {
        }

        [ExcludeFromCodeCoverage]
        public ComplexWhereItemCondition()
            : this(ContainerManagerFactory.GetContainerManager().Resolve<IDatabaseHelper>())
        {
        }

        /// <summary>
        ///     The visit query.
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule context.
        /// </param>
        public override void VisitQuery<TContext>(ISitecoreQueryBuilder queryBuilder, TContext ruleContext)
        {
            ID id;
            string value = Value;
            value = ruleContext.GetValue(value);
            if (string.IsNullOrEmpty(value))
            {
            }
            if (!ID.TryParse(value, out id))
            {
                return;
            }

            queryBuilder.Field(GetFieldName(), id, Matches);
        }

    }
}