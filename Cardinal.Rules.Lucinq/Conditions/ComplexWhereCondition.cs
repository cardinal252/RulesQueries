using Cardinal.Core.Data;
using Cardinal.Core.IoC;
using Cardinal.Rules.Core;
using Cardinal.Rules.Lucinq.Conditions;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Rules;

namespace Cardinal.Rules.Lucinq.Conditions
{
    /// <summary>
    ///     The complex where condition.
    /// </summary>
    /// <typeparam name="T">
    ///     The rule context type
    /// </typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    public class ComplexWhereCondition<T> : WhereCondition<T>
        where T : RuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ComplexWhereCondition{T,TBuilder}" /> class.
        /// </summary>
        public ComplexWhereCondition()
            : this(ContainerManagerFactory.GetContainerManager().Resolve<IDatabaseHelper>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ComplexWhereCondition{T,TBuilder}" /> class.
        /// </summary>
        /// <param name="contentManager">
        ///     The content manager.
        /// </param>
        public ComplexWhereCondition(IDatabaseHelper contentManager)
            : base(contentManager)
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
            string value = ruleContext.GetValue(Value);
            if (string.IsNullOrEmpty(value) == false)
            {
                base.VisitQuery(queryBuilder, ruleContext);
            }
        }
    }
}