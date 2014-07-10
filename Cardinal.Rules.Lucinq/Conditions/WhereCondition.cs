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
    ///     The where condition.
    /// </summary>
    /// <typeparam name="T">
    ///     The rule context
    /// </typeparam>
    public class WhereCondition<T> : AbstractWhereCondition<T, ISitecoreQueryBuilder>, ILuceneQueryConditionVisitor
        where T : RuleContext
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="WhereCondition{T}" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public WhereCondition()
            : this(ContainerManagerFactory.GetContainerManager().Resolve<IDatabaseHelper>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereCondition{T}" /> class.
        /// </summary>
        /// <param name="databaseHelper">
        ///     The content manager.
        /// </param>
        public WhereCondition(IDatabaseHelper databaseHelper)
            : base(databaseHelper)
        {
        }

        /// <summary>
        ///     Gets or sets the operator id.
        /// </summary>
        public string OperatorId { get; set; }

        public Matches Matches { get; set; }

        /// <summary>
        ///     Adds the condition to the query
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        public override void VisitQuery<TContext>(ISitecoreQueryBuilder queryBuilder, TContext ruleContext)
        {
            string fieldName = GetFieldName();

            WhereConditionOperator whereOperator = GetOperator();

            string value = Value;

            value = ruleContext.GetValue(value);

            if (value.Contains(" "))
            {
                queryBuilder.Keyword(fieldName, Value, this.Matches);
                return;
            }

            switch (whereOperator)
            {
                case WhereConditionOperator.StartsWith:
                    value = "*" + value;
                    break;
                case WhereConditionOperator.Contains:
                    value = string.Format("*{0}*", value);
                    break;
                case WhereConditionOperator.EndsWith:
                    value = value + "*";
                    break;
                default:

                    // required for stylecop compliance.
                    break;
            }

            ID id;
            if (ID.TryParse(Value, out id))
            {
                queryBuilder.Field(fieldName, id, this.Matches);
                return;
            }

            queryBuilder.Field(fieldName, value, this.Matches);
        }

        /// <summary>
        ///     The get operator.
        /// </summary>
        /// <returns>
        ///     The <see cref="WhereConditionOperator" />.
        /// </returns>
        protected WhereConditionOperator GetOperator()
        {
            if (OperatorId == Constants.LuceneOperatorConstants.IsEqualTo)
            {
                return WhereConditionOperator.Equals;
            }

            if (OperatorId == Constants.LuceneOperatorConstants.Contains)
            {
                return WhereConditionOperator.Contains;
            }

            if (OperatorId == Constants.LuceneOperatorConstants.StartsWith)
            {
                return WhereConditionOperator.StartsWith;
            }

            if (OperatorId == Constants.LuceneOperatorConstants.EndsWith)
            {
                return WhereConditionOperator.EndsWith;
            }

            return WhereConditionOperator.Unknown;
        }
    }
}