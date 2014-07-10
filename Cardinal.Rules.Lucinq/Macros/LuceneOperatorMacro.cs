using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Sitecore.Rules.RuleMacros;
using Sitecore.Text;

namespace Cardinal.Rules.Lucinq.Macros
{
    /// <summary>
    ///     The operator macro.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LuceneOperatorMacro : MacroBase, IRuleMacro
    {
        /// <summary>
        ///     The execute.
        /// </summary>
        /// <param name="element">
        ///     The element.
        /// </param>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="parameters">
        ///     The parameters.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public void Execute(XElement element, string name, UrlString parameters, string value)
        {
            GetSelectionItemOptions(
                element,
                name,
                parameters,
                value,
                "Select Operator",
                "Select the operator to use in this query.",
                Constants.LuceneOperators);
        }
    }
}