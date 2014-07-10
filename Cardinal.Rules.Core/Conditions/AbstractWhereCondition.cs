using System;
using Cardinal.Core.Data;
using Sitecore.Data;
using Sitecore.Rules;

namespace Cardinal.Rules.Core.Conditions
{
    /// <summary>
    /// The abstract where condition.
    /// </summary>
    /// <typeparam name="T">
    ///  The rule context type
    /// </typeparam>
    /// <typeparam name="TBuilder">
    /// The builder type</typeparam>
    public abstract class AbstractWhereCondition<T, TBuilder> : AbstractQueryCondition<T, TBuilder>
        where T : RuleContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractWhereCondition{T,TBuilder}" /> class.
        /// </summary>
        /// <param name="databaseHelper">
        ///     The content manager.
        /// </param>
        protected AbstractWhereCondition(IDatabaseHelper databaseHelper)
        {
            DatabaseHelper = databaseHelper;
        }

        /// <summary>
        ///     Gets or sets the field.
        /// </summary>
        public string FieldId { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

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
    }
}