using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Cardinal.Core.Data;
using Cardinal.Core.IoC;
using Cardinal.Rules.Core.Conditions;
using Lucinq.SitecoreIntegration.Extensions;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Cardinal.Rules.Lucinq.Conditions
{
    /// <summary>
    ///     The complex where item condition.
    /// </summary>
    /// <typeparam name="T">
    ///     The rule context type
    /// </typeparam>
    public class ComplexWhereDateCondition<T> : WhereItemCondition<T> where T : RuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ComplexWhereItemCondition{T}" /> class.
        /// </summary>
        /// <param name="contentManager">
        ///     The content manager.
        /// </param>
        public ComplexWhereDateCondition(IDatabaseHelper databaseHelper)
            : base(databaseHelper)
        {
        }

        [ExcludeFromCodeCoverage]
        public ComplexWhereDateCondition()
            : this(ContainerManagerFactory.GetContainerManager().Resolve<IDatabaseHelper>())
        {
        }

        public string OperatorId { get; set; }
       
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
            DateTime dateTime;
            string value = Value;
            value = ruleContext.GetValue(value);
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (!DateTime.TryParse(value, CultureInfo.GetCultureInfo("en"), DateTimeStyles.None, out dateTime))
            {
                return;
            }

            string luceneDateTime = dateTime.ToString("yyyyMMdd");
            ConditionOperator conditionOperator = GetOperator();
            string fieldName = SitecoreQueryBuilderExtensions.GetEncodedFieldName(GetFieldName());
            switch (conditionOperator)
            {
                case ConditionOperator.Equal:
                    queryBuilder.Field(fieldName, luceneDateTime, Matches);
                    break;
                case ConditionOperator.GreaterThanOrEqual:
                    queryBuilder.TermRange(fieldName, luceneDateTime, DateTime.MaxValue.ToString("yyyyMMdd"), true, true,
                        Matches);
                    break;
                case ConditionOperator.GreaterThan:
                    queryBuilder.TermRange(fieldName, luceneDateTime, DateTime.MaxValue.ToString("yyyyMMdd"), false,
                        true, Matches);
                    break;
                case ConditionOperator.LessThanOrEqual:
                    queryBuilder.TermRange(fieldName, DateTime.MinValue.ToString("yyyyMMdd"), luceneDateTime, true, true,
                        Matches);
                    break;
                case ConditionOperator.LessThan:
                    queryBuilder.TermRange(fieldName, DateTime.MinValue.ToString("yyyyMMdd"), luceneDateTime, true,
                        false, Matches);
                    break;
            }
        }

        protected ConditionOperator GetOperator()
        {
            if (string.IsNullOrEmpty(OperatorId))
                return ConditionOperator.Unknown;
            switch (OperatorId)
            {
                case "{066602E2-ED1D-44C2-A698-7ED27FD3A2CC}":
                    return ConditionOperator.Equal;
                case "{814EF7D0-1639-44FD-AEEF-735B5AC14425}":
                    return ConditionOperator.GreaterThanOrEqual;
                case "{B88CD556-082E-4385-BB76-E4D1B565F290}":
                    return ConditionOperator.GreaterThan;
                case "{2E1FC840-5919-4C66-8182-A33A1039EDBF}":
                    return ConditionOperator.LessThanOrEqual;
                case "{E362A3A4-E230-4A40-A7C4-FC42767E908F}":
                    return ConditionOperator.LessThan;
                case "{3627ED99-F454-4B83-841A-A0194F0FB8B4}":
                    return ConditionOperator.NotEqual;
                default:
                    return ConditionOperator.Unknown;
            }
        }
    }
}