using System;
using System.Diagnostics.CodeAnalysis;
using Cardinal.Core.Data;
using Cardinal.Core.IoC;
using Cardinal.Rules.Core.Actions;
using Lucinq.SitecoreIntegration.Extensions;
using Lucinq.SitecoreIntegration.Querying.Interfaces;
using Sitecore.Data;
using Sitecore.Rules;

namespace Cardinal.Rules.Lucinq.Actions
{
    /// <summary>
    ///     The sort action.
    /// </summary>
    /// <typeparam name="T">
    ///     The rule context type
    /// </typeparam>
    public class SortAction<T> : AbstractRulesAction<T, ISitecoreQueryBuilder>
        where T : RuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SortAction{T}" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SortAction()
            : this(ContainerManagerFactory.GetContainerManager().Resolve<IDatabaseHelper>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SortAction{T}" /> class.
        /// </summary>
        /// <param name="databaseHelper">
        ///     The content manager.
        /// </param>
        public SortAction(IDatabaseHelper databaseHelper)
        {
            DatabaseHelper = databaseHelper;
        }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        ///     Gets or sets the field.
        /// </summary>
        public string FieldId { get; set; }

        /// <summary>
        ///     Gets the content manager.
        /// </summary>
        protected IDatabaseHelper DatabaseHelper { get; private set; }

        /// <summary>
        ///     Gets the field name
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public virtual string GetFieldName()
        {
            ID fieldId;
            if (!ID.TryParse(FieldId, out fieldId))
            {
                throw new NullReferenceException("The field id was not in a correct format.");
            }

            var item = DatabaseHelper.GetDatabase().GetItem(fieldId);

            if (string.IsNullOrEmpty(item["Name"]))
            {
                throw new NullReferenceException("The field name was not specified");
            }

            return item.Name;
        }

        /// <summary>
        ///     Visits the query.
        /// </summary>
        /// <param name="queryBuilder">
        ///     The query builder.
        /// </param>
        /// <param name="ruleContext">
        ///     The rule Context.
        /// </param>
        public override void VisitQuery<TContext>(ISitecoreQueryBuilder queryBuilder, TContext ruleContext)
        {
            if (string.IsNullOrEmpty(FieldId))
            {
                return;
            }

            bool descending = Direction == "{582B22C5-3AE2-4239-A879-03901B0F2585}";

            string fieldName = SitecoreQueryBuilderExtensions.GetEncodedFieldName(GetFieldName());

            queryBuilder.Sort(fieldName, descending);
        }
    }
}